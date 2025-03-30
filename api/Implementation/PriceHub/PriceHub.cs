using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;

namespace Implementation
{
    public class PriceHub : Hub
    {
        private readonly ILogger<PriceHub> _logger;

        public PriceHub(ILogger<PriceHub> logger)
        {
            _logger = logger;
        }

        public async Task SubscribeToInstrument(string instrument)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, instrument);
            await Clients.Caller.SendAsync("Subscribed", instrument);

            _logger.LogInformation($"Client {Context.ConnectionId} subscribed to {instrument}");
        }

        public async Task UnsubscribeFromInstrument(string instrument)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, instrument);
            await Clients.Caller.SendAsync("Unsubscribed", instrument);
            _logger.LogInformation($"Client {Context.ConnectionId} unsubscribed from {instrument}");
        }
    }
}
