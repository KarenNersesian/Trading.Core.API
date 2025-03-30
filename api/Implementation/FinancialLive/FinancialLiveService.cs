using Implementation.SocketProviders.Binance;
using Interfaces;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System.Collections.Concurrent;
using System.Diagnostics.Metrics;
using Types.Financial;
using Types.FinancialLive;

namespace Implementation.FinancialLive
{
    public class FinancialLiveService : IFinancialLiveService
    {
        private readonly IHubContext<PriceHub> _hubContext;
        private readonly BinanceWebSocketClient _binanceClient;
        private readonly ILogger<FinancialLiveService> _logger;
        private static readonly ConcurrentDictionary<string, int> ActiveSubscriptions = new();

        public FinancialLiveService(BinanceWebSocketClient binanceClient,
           IHubContext<PriceHub> hubContext,
           ILogger<FinancialLiveService> logger)
        {
            _binanceClient = binanceClient;
            _hubContext = hubContext;
            _logger = logger;
        }

        public async Task<SubscribeResponse> Subscribe(SubscribeRequest request)
        {
            request.Validate();

            await _hubContext.Groups.AddToGroupAsync(request.SubscribeInfo.ConnectionId.ToString(), request.SubscribeInfo.Instrument.ToUpper());

            ActiveSubscriptions.AddOrUpdate(request.SubscribeInfo.Instrument, 1, (_, count) => count + 1);

            await _binanceClient.SubscribeToInstrument(request.SubscribeInfo.Instrument);

            return new SubscribeResponse() { Status = true, SubscribeInfo = request.SubscribeInfo };
        }

        public async Task<UnSubscribeResponse> UnSubscribe(UnSubscribeRequest request)
        {
            request.Validate();

            request.SubscribeInfo.Instrument = request.SubscribeInfo.Instrument.ToUpper();

            await _hubContext.Groups.RemoveFromGroupAsync(request.SubscribeInfo.ConnectionId.ToString(), request.SubscribeInfo.Instrument);

            if (ActiveSubscriptions.TryGetValue(request.SubscribeInfo.Instrument, out int count) && count > 1)
            {
                ActiveSubscriptions[request.SubscribeInfo.Instrument] = count - 1;
            }
            else
            {
                ActiveSubscriptions.TryRemove(request.SubscribeInfo.Instrument, out _);
                await _binanceClient.UnsubscribeFromInstrument(request.SubscribeInfo.Instrument);
            }

            return new UnSubscribeResponse() { Status = true, SubscribeInfo = request.SubscribeInfo };
        }
    }
}
