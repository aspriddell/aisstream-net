// AISStream.NET - a high-performance aisstream.io client library for .NET
// Licensed under Apache-2.0 - see the license file for more information

using System.Buffers;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading.Channels;
using Microsoft.Extensions.Logging;

namespace AISStream;

/// <summary>
/// Represents a client-side connection to the aisstream.io websocket for receiving events.
/// </summary>
public partial class AISEventReceiver : IAsyncDisposable, IDisposable
{
    private static readonly Uri WebsocketUrl = new("wss://stream.aisstream.io/v0/stream");

    private ClientWebSocket? _webSocket;
    private CancellationTokenSource? _cts;

    private AISSubscriptionRequestOptions? _subscriptionRequest;

    private volatile bool _isDisconnecting;

    private readonly string _apiKey;
    private readonly ILogger? _logger;
    private readonly HttpMessageInvoker _invoker;
    private readonly Channel<AISEvent> _messagePipeline;

    private readonly SemaphoreSlim _connectLock = new(1, 1);

    /// <summary>
    /// Creates a new instance of the <see cref="AISEventReceiver"/> class.
    /// </summary>
    /// <param name="apiKey">The api key to use with connections to the aisstream service.</param>
    /// <param name="options">(optional) configuration for the <see cref="Channel"/> used to publish updates to</param>
    /// <param name="handler">(optional) custom <see cref="HttpMessageHandler"/> to use with the websocket</param>
    /// <param name="disposeHandler">(optional) if <see cref="handler"/> is set, whether to call <see cref="HttpMessageHandler.Dispose"/> when the current instance is disposed.</param>
    /// <param name="logger">(optional) logger to receive diagnostic messages</param>
    public AISEventReceiver(string apiKey, UnboundedChannelOptions? options = null, HttpMessageHandler? handler = null, bool disposeHandler = true, ILogger logger = null)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(apiKey);

        _apiKey = apiKey;
        _logger = logger;
        _invoker = handler != null ? new HttpMessageInvoker(handler, disposeHandler) : new HttpMessageInvoker(new SocketsHttpHandler());
        _messagePipeline = Channel.CreateUnbounded<AISEvent>(options ?? new UnboundedChannelOptions
        {
            SingleWriter = true
        });
    }

    /// <summary>
    /// A (thread-safe, concurrent) channel of <see cref="AISEvent"/> objects received.
    /// </summary>
    public ChannelReader<AISEvent> EventStream => _messagePipeline.Reader;

    /// <summary>
    /// Whether to include events that are not supported by the current version of this library.
    /// </summary>
    public bool IncludeUnsupportedEvents { get; set; }

    public async Task ConnectAsync(AISSubscriptionRequestOptions options, CancellationToken cancellationToken = default)
    {
        await _connectLock.WaitAsync(cancellationToken).ConfigureAwait(false);

        try
        {
            if (_webSocket?.State != WebSocketState.Open)
            {
                _isDisconnecting = false;
            
                _cts?.Dispose();
                _webSocket?.Dispose();

                _cts = new CancellationTokenSource();
                _webSocket = new ClientWebSocket();

                _logger?.LogInformation("Performing websocket connection to {url}", WebsocketUrl);
                await _webSocket.ConnectAsync(WebsocketUrl, _invoker, cancellationToken).ConfigureAwait(false);
                
                _logger?.LogDebug("Starting read loop...");
                _ = Task.Factory.StartNew(() => ReadLoopAsync(_cts.Token), TaskCreationOptions.LongRunning);
            }

            _subscriptionRequest = options;

            var req = _subscriptionRequest.CreateRequest(_apiKey);
            var serializedRequest = JsonSerializer.SerializeToUtf8Bytes(req, AISSerializerContext.Default.AISSubscriptionRequestBody);

            _logger?.LogDebug("Sending subscription request to AISStream...");
            await _webSocket.SendAsync(serializedRequest, WebSocketMessageType.Binary, WebSocketMessageFlags.EndOfMessage, cancellationToken);

            _logger?.LogInformation("AISStream subscription request sent successfully.");
        }
        finally
        {
            _connectLock.Release();
        }
    }

    private async Task ReadLoopAsync(CancellationToken cancellationToken)
    {
        var buffer = new byte[4096];
        while (!_isDisconnecting && _webSocket?.State == WebSocketState.Open)
        {
            try
            {
                var result = await _webSocket.ReceiveAsync(buffer, cancellationToken).ConfigureAwait(false);
                
                // handle close request messages
                if (result.MessageType == WebSocketMessageType.Close)
                {
                    _logger?.LogDebug("Received close message from server, closing connection...");

                    await HandleDisconnectAsync();
                    break;
                }
                
                // messages that fit in a single payload
                if (result.EndOfMessage)
                {
                    await ProcessMessageAsync(buffer, result.Count);
                    continue;
                }

                var tempBuffer = ArrayPool<byte>.Shared.Rent(8192);

                try
                {
                    using var memoryStream = new MemoryStream(tempBuffer, 0, 0, true, true);

                    memoryStream.Write(buffer, 0, result.Count);
                    
                    while (!result.EndOfMessage)
                    {
                        result = await _webSocket.ReceiveAsync(buffer, cancellationToken).ConfigureAwait(false);
                        memoryStream.Write(buffer, 0, result.Count);
                    }

                    await ProcessMessageAsync(memoryStream.GetBuffer(), (int)memoryStream.Length);
                }
                finally
                {
                    ArrayPool<byte>.Shared.Return(tempBuffer);
                }
            }
            catch (OperationCanceledException)
            {
                _logger?.LogDebug("Read loop cancelled, closing connection...");

                await HandleDisconnectAsync();
                break;
            }
            catch (Exception e)
            {
                _logger?.LogWarning(e, "Exception in read loop ({message}), attempting to reconnect...", e.Message);

                await HandleDisconnectAsync();
                break;
            }
        }

        if (!_isDisconnecting)
        {
            _logger?.LogDebug("Read loop ended unexpectedly, attempting to reconnect...");
            await TryReconnectAsync();
        }
        else
        {
            _logger?.LogDebug("Read loop ended due to disconnect request.");
        }
    }

    private async Task TryReconnectAsync()
    {
        var request = _subscriptionRequest;
        if (request == null)
        {
            return;
        }
        
        const int maxAttempts = 5;
        var attempt = 0;

        while (attempt < maxAttempts && !_isDisconnecting)
        {
            try
            {
                _logger?.LogDebug("Attempting websocket reconnect (attempt {attempt})", attempt + 1);

                await ConnectAsync(request);
                break;
            }
            catch
            {
                var delay = Math.Pow(2, attempt++);

                _logger?.LogDebug("Reconnect attempt {attempt} failed, retrying in {delay} seconds...", attempt, delay);
                await Task.Delay(TimeSpan.FromSeconds(delay));
            }
        }
    }

    private async Task HandleDisconnectAsync()
    {
        if (_webSocket?.State != WebSocketState.Open)
        {
            return;
        }
        
        try
        {
            _logger?.LogDebug("Sending websocket close message...");
            await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);
        }
        catch
        {
            // ignored
        }
    }

    public async Task DisconnectAsync()
    {
        _isDisconnecting = true;
        _cts?.Cancel();

        if (_webSocket?.State == WebSocketState.Open)
        {
            _logger?.LogInformation("Disconnecting from AISStream...");
            await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Disconnect", CancellationToken.None);
        }

        _webSocket?.Dispose();
        _webSocket = null;
    }

    private async ValueTask ProcessMessageAsync(byte[] data, int length)
    {
        try
        {
            var message = JsonSerializer.Deserialize(data.AsSpan(0, length), AISSerializerContext.Default.AISEvent);
            if (message != null && (message.IsSupported || IncludeUnsupportedEvents))
            {
                await _messagePipeline.Writer.WriteAsync(message, CancellationToken.None);
            }
        }
        catch (Exception e)
        {
            _logger?.LogError(e, "Failed to deserialize incoming message: {Message}", e.Message);
            _logger?.LogDebug("Message content: {Content}", Encoding.UTF8.GetString(data.AsSpan(0, length)));
        }
    }

    public void Dispose()
    {
        _webSocket?.Dispose();
        _cts?.Dispose();
        _invoker.Dispose();
        _connectLock.Dispose();
    }

    public async ValueTask DisposeAsync()
    {
        if (_webSocket != null) await CastAndDispose(_webSocket);
        if (_cts != null) await CastAndDispose(_cts);

        await CastAndDispose(_invoker);
        await CastAndDispose(_connectLock);

        return;

        static async ValueTask CastAndDispose(IDisposable resource)
        {
            if (resource is IAsyncDisposable resourceAsyncDisposable)
                await resourceAsyncDisposable.DisposeAsync();
            else
                resource.Dispose();
        }
    }
}
