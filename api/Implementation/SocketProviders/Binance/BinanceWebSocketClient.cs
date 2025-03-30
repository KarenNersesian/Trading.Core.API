using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Concurrent;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Implementation.SocketProviders.Binance
{
    public class BinanceWebSocketClient
    {
        private readonly IHubContext<PriceHub> _hubContext;
        private ClientWebSocket _client;
        private readonly Uri _binanceUri = new Uri("wss://stream.binance.com:443/ws/btcusdt");
        private readonly ConcurrentDictionary<string, bool> _subscriptions = new();
        private readonly ILogger<BinanceWebSocketClient> _logger;

        public BinanceWebSocketClient(IHubContext<PriceHub> hubContext, ILogger<BinanceWebSocketClient> logger)
        {
            _hubContext = hubContext;
            _client = new ClientWebSocket();
            _logger = logger;
        }

        public async Task ConnectAsync()
        {
            try
            {
                _client = new ClientWebSocket();
                await _client.ConnectAsync(_binanceUri, CancellationToken.None);
                _logger.LogInformation("Connected to Binance WebSocket.");
                _ = Task.Run(ReceiveMessagesAsync);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"WebSocket error: {ex.Message}. Reconnecting in 5 sec...");
                await Task.Delay(5000);
            }
        }

        public async Task SubscribeToInstrument(string instrument)
        {
            if (!_subscriptions.ContainsKey(instrument))
            {
                _subscriptions[instrument] = true;
                var subscribeMessage = new
                {
                    method = "SUBSCRIBE",
                    @params = new[] { $"{instrument.ToLower()}@aggTrade" },
                    id = 1
                };

                string message = JsonConvert.SerializeObject(subscribeMessage);
                await _client.SendAsync(Encoding.UTF8.GetBytes(message), WebSocketMessageType.Text, true, CancellationToken.None);
                Console.WriteLine($"Subscribed to {instrument}");
            }
        }

        public async Task UnsubscribeFromInstrument(string instrument)
        {
            if (_client.State == WebSocketState.Open)
            {
                var unsubscribeMessage = new
                {
                    method = "UNSUBSCRIBE",
                    @params = new[] { $"{instrument.ToLower()}@aggTrade" },
                    id = 1
                };
                string message = JsonConvert.SerializeObject(unsubscribeMessage);
                await _client.SendAsync(Encoding.UTF8.GetBytes(message), WebSocketMessageType.Text, true, CancellationToken.None);
            }
        }

        private async Task ReceiveMessagesAsync()
        {
            var buffer = new byte[4096];

            while (_client.State == WebSocketState.Open)
            {
                var result = await _client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                if (result.MessageType == WebSocketMessageType.Text)
                {
                    string jsonMessage = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    await OnMessageReceived(jsonMessage);
                }
            }
        }

        private async Task OnMessageReceived(string jsonMessage)
        {
            var update = JsonConvert.DeserializeObject<BinancePriceUpdate>(jsonMessage);
            if (update != null)
            {
                string instrument = update.Symbol.ToUpper();
                await _hubContext.Clients.Group(instrument).SendAsync("ReceivePriceUpdate", update.Price);
            }
        }
    }

    public class BinancePriceUpdate
    {
        [JsonProperty("s")] public string Symbol { get; set; }
        [JsonProperty("p")] public string Price { get; set; }
    }
}
