using Implementation.SocketProviders.Binance;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;

namespace Implementation
{
    public class PriceHub : Hub
    {
        private readonly ILogger<PriceHub> _logger;
        private readonly BinanceWebSocketClient _binanceClient;

        private static readonly ConcurrentDictionary<string, int> ActiveSubscriptions = new();

        public PriceHub(ILogger<PriceHub> logger, BinanceWebSocketClient binanceClient)
        {
            _logger = logger;
            _binanceClient = binanceClient;
        }

        public async Task SubscribeToInstrument(string instrument)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, instrument);
            await Clients.Caller.SendAsync("Subscribed", instrument);

            ActiveSubscriptions.AddOrUpdate(instrument, 1, (_, count) => count + 1);

            await _binanceClient.SubscribeToInstrument(instrument);

            _logger.LogInformation($"Client {Context.ConnectionId} subscribed to {instrument}");
        }

        public async Task UnsubscribeFromInstrument(string instrument)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, instrument);
            await Clients.Caller.SendAsync("Unsubscribed", instrument);
            _logger.LogInformation($"Client {Context.ConnectionId} unsubscribed from {instrument}");

            if (ActiveSubscriptions.TryGetValue(instrument, out int count) && count > 1)
            {
                ActiveSubscriptions[instrument] = count - 1;
            }
            else
            {
                ActiveSubscriptions.TryRemove(instrument, out _);
                await _binanceClient.UnsubscribeFromInstrument(instrument);
            }
        }
    }
}
