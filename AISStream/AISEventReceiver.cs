// AISStream.NET - a high-performance aisstream.io client library for .NET
// Licensed under Apache-2.0 - see the license file for more information

using System.Net.WebSockets;
using System.Text.Json;
using System.Text.Json.Serialization;
using System.Threading.Channels;

namespace AISStream;

/// <summary>
/// Represents a client-side connection to the aisstream.io websocket for receiving events.
/// </summary>
public partial class AISEventReceiver : IDisposable
{
    private static readonly Uri WebsocketUrl = new("wss://stream.aisstream.io/v0/stream");

    private readonly string _apiKey;
    private readonly HttpMessageInvoker _invoker;
    private readonly Channel<AISEvent> _messagePipeline;

    private Task? _readTask;
    private ClientWebSocket? _webSocket;
    private CancellationTokenSource? _readTaskCancellation;

    private AISSubscriptionRequest? _lastSubscriptionRequest;
    
    private bool _disposed;

    /// <summary>
    /// Creates a new instance of the <see cref="AISEventReceiver"/> class.
    /// </summary>
    /// <param name="apiKey">The api key to use with connections to the aisstream service.</param>
    /// <param name="options">(optional) configuration for the <see cref="Channel"/> used to publish updates to</param>
    /// <param name="handler">(optional) custom <see cref="HttpMessageHandler"/> to use with the websocket</param>
    /// <param name="disposeHandler">(optional) if <see cref="handler"/> is set, whether to call <see cref="HttpMessageHandler.Dispose"/> when the current instance is disposed.</param>
    public AISEventReceiver(string apiKey, UnboundedChannelOptions? options = null, HttpMessageHandler? handler = null, bool disposeHandler = true)
    {
        ArgumentException.ThrowIfNullOrWhiteSpace(apiKey, nameof(apiKey));

        _apiKey = apiKey;
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

    /// <summary>
    /// Begin a connection to the AISStream service with the given subscription request.
    /// If a connection is already active this will send the new <see cref="request"/>, replacing the previous one.
    /// </summary>
    /// <param name="request">The subscription request information</param>
    /// <param name="cancellationToken">Cancellation token, where the task can be cancelled until the background read loop has started.</param>
    /// <returns>A task that completes when the connection has been created, a read task has been started and the subscription request has been sent.</returns>
    public Task ConnectAsync(AISSubscriptionRequest request, CancellationToken cancellationToken = default)
    {
        ObjectDisposedException.ThrowIf(_disposed, this);
        return InternalConnectAsync(request, cancellationToken);
    }

    /// <summary>
    /// Disconnects from the service.
    /// </summary>
    public async Task DisconnectAsync()
    {
        if (_disposed || _webSocket?.State != WebSocketState.Open)
        {
            return;
        }

        await _webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Disconnect", CancellationToken.None);
        await _readTaskCancellation!.CancelAsync();
    }

    private async Task InternalConnectAsync(AISSubscriptionRequest request, CancellationToken cancellationToken)
    {
        if (_disposed)
        {
            return;
        }

        if (_webSocket?.State is WebSocketState.CloseSent or WebSocketState.CloseReceived)
        {
            throw new Exception("The underlying websocket is currently closing.");
        }

        if (_webSocket?.State is null or WebSocketState.Closed or WebSocketState.Aborted)
        {
            _webSocket?.Dispose();
            _webSocket = new ClientWebSocket();
        }

        if (_webSocket.State != WebSocketState.Open)
        {
            await _webSocket.ConnectAsync(WebsocketUrl, _invoker, cancellationToken);

            _readTaskCancellation?.Cancel();
            _readTaskCancellation?.Dispose();
            _readTaskCancellation = new CancellationTokenSource();

            // start read task if one is not already running (as sometimes the read loop will invoke a reconnection)
            if (_readTask?.Status != TaskStatus.Running)
            {
                _readTask?.Dispose();
                _readTask = Task.Factory.StartNew(() => AsyncMessageLoop(_webSocket, _readTaskCancellation.Token), TaskCreationOptions.LongRunning);
            }
        }

        var req = AISAuthenticatedSubscriptionRequest.CreateAuthenticatedRequest(request, _apiKey);
        var serializedRequest = JsonSerializer.SerializeToUtf8Bytes(req, SerializerContext.Default.AISAuthenticatedSubscriptionRequest);

        await _webSocket.SendAsync(serializedRequest, WebSocketMessageType.Binary, WebSocketMessageFlags.EndOfMessage, cancellationToken);

        // store for reconnection purposes later on
        _lastSubscriptionRequest = req;
    }

    private async Task AsyncMessageLoop(ClientWebSocket socket, CancellationToken cancellation)
    {
        MemoryStream? messageAccumulator = null;
        var buffer = new byte[4096];

        try
        {
            while (socket.State == WebSocketState.Open && !cancellation.IsCancellationRequested)
            {
                ValueWebSocketReceiveResult result;

                try
                {
                    result = await socket.ReceiveAsync(buffer.AsMemory(), cancellation);
                }
                catch (WebSocketException e)
                {
                    // rethrow for any non-transient errors we can't resolve by reconnecting
                    if (e.WebSocketErrorCode is WebSocketError.HeaderError or WebSocketError.InvalidMessageType or WebSocketError.NotAWebSocket or WebSocketError.UnsupportedProtocol or WebSocketError.UnsupportedVersion)
                    {
                        throw;
                    }

                    if (!cancellation.IsCancellationRequested && _lastSubscriptionRequest != null)
                    {
                        await InternalConnectAsync(_lastSubscriptionRequest, cancellation);
                        continue;
                    }

                    return;
                }
                catch (OperationCanceledException)
                {
                    break;
                }

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await socket.CloseAsync(WebSocketCloseStatus.NormalClosure, "Closing", CancellationToken.None);

                    // user didn't ask for a disconnect, try reconnecting (and reuse this task)
                    if (!cancellation.IsCancellationRequested && _lastSubscriptionRequest != null)
                    {
                        await InternalConnectAsync(_lastSubscriptionRequest, cancellation);
                        continue;
                    }

                    break;
                }

                // non-segmented messages that fit in the buffer
                if (result.EndOfMessage && messageAccumulator == null)
                {
                    var message = JsonSerializer.Deserialize(buffer.AsSpan(0, result.Count), SerializerContext.Default.AISEvent);
                    if (message != null && (message.IsSupported || IncludeUnsupportedEvents))
                    {
                        await _messagePipeline.Writer.WriteAsync(message, CancellationToken.None);
                    }

                    continue;
                }

                // segmented messages/messages that exceed the buffer size
                if (messageAccumulator == null)
                {
                    messageAccumulator = new MemoryStream(buffer, 0, result.Count, true);
                    continue;
                }

                // write next block, then see if that was the end of the message
                messageAccumulator.Write(buffer.AsSpan(0, result.Count));

                if (result.EndOfMessage)
                {
                    messageAccumulator.Seek(0, SeekOrigin.Begin);

                    var message = JsonSerializer.Deserialize(messageAccumulator, SerializerContext.Default.AISEvent);
                    if (message != null && (message.IsSupported || IncludeUnsupportedEvents))
                    {
                        await _messagePipeline.Writer.WriteAsync(message, CancellationToken.None);
                    }

                    messageAccumulator.Dispose();
                    messageAccumulator = null;
                }
            }
        }
        finally
        {
            messageAccumulator?.Dispose();
        }
    }

    /// <summary>
    /// Releases resources used by this instance.
    /// </summary>
    public void Dispose()
    {
        if (_disposed)
        {
            return;
        }

        _readTaskCancellation?.Cancel();
        _readTaskCancellation?.Dispose();

        _readTask?.Wait();
        _messagePipeline.Writer.Complete();

        _invoker.Dispose();
        _webSocket?.Dispose();

        _disposed = true;
    }

    [JsonSerializable(typeof(AISEvent))]
    [JsonSerializable(typeof(AISMessage))]
    [JsonSerializable(typeof(AISAuthenticatedSubscriptionRequest))]
    [JsonSourceGenerationOptions(JsonSerializerDefaults.Web, AllowOutOfOrderMetadataProperties = true)]
    internal partial class SerializerContext : JsonSerializerContext;
}