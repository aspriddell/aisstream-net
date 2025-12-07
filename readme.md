# AISStream.NET
An [aisstream.io](https://aisstream.io) client library for .NET

## Overview
AISStream.NET provides a memory-efficient client library for receiving AIS data from [aisstream.io](https://aisstream.io) with support for:

- Custom bounding boxes (including global coverage)
- Filters for message types and MMSI numbers
- Automatic websocket reconnection
- Async processing for multiple concurrent readers

### Usage
> [!NOTE]
> You will need an aisstream.io API key, which you can obtain by signing up on their website with a GitHub account.

1. Install the package via [NuGet](https://www.nuget.org/packages/AISStream.NET)
2. Create an `AISEventReceiver` using your API key
3. Call `ConnectAsync` with your desired `AISSubscriptionRequestOptions`
4. Await events from the `EventStream`, retrieving the `AISEvent.Message` property for each event
5. Disconnect once finished using `DisconnectAsync`

```csharp
public static class Program 
{
    public static async Task Main(string[] args)
    {
        // use a cancellation token to break out of the event loop
        // if used in a worker, this token would be cancelled in the StopAsync method
        using var cancellationTokenSource = new CancellationTokenSource();
        
        // create event receiver with api key from aisstream.io
        var client = new AISEventReceiver("your_api_key_here");
        var options = new AISSubscriptionRequestOptions 
        {
            BoundingBoxes = [BoundingBox.World],
            FiltersMessageType = [AISMessageType.PositionReport, AISMessageType.ShipStaticData]
        };
        
        // start connection and begin reading events
        await client.ConnectAsync(options);
    
        try
        {
            await foreach (var aisEvent in client.EventStream.ReadAllAsync(cancellationTokenSource.CancellationToken))
            {
                ProcessMessage(aisEvent);
            }
        }
        catch (OperationCancelledException) when (cancellationTokenSource.IsCancellationRequested)
        {
            // expected when cancelling
        }
        
        // disconnect when done
        await client.DisconnectAsync();
    }
    
    private static void ProcessMessage(AISEvent event) 
    {
        switch (event.Message)
        {
            case AISPositionReportMessage positionReport:
                Console.WriteLine($"Position report from MMSI {positionReport.MMSI}: Lat {positionReport.Latitude}, Lon {positionReport.Longitude}");
                break;
    
            case AISShipStaticDataMessage staticData:
                Console.WriteLine($"Static data from MMSI {staticData.MMSI}: Name {staticData.ShipName}, Type {staticData.ShipType}");
                break;
    
            default:
                Console.WriteLine($"Received other AIS message type: {event.Message.GetType().Name}");
                break;
        }
    }
}
```

### License
This project is licensed under the Apache-2.0 License. See [license.md](license.md) for more details.
