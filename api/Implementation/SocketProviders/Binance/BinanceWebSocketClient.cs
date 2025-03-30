using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
                var bytes = Encoding.UTF8.GetBytes(message);
                await _client.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
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
            var buffer = new byte[1024 * 4];
            try
            {
                while (_client.State == WebSocketState.Open)
                {
                    var result = await _client.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                    var message = Encoding.UTF8.GetString(buffer, 0, result.Count);

                    _logger.LogInformation($"Received from Binance: {message}");
                    //await _hubContext.Clients.All.SendAsync("ReceivePriceUpdate", message);

                    var json = JObject.Parse(message);
                    string instrument = json["s"]?.ToString();
                    string price = json["p"]?.ToString();

                    if(!string.IsNullOrEmpty(instrument) && !string.IsNullOrEmpty(price))
                    {
                        await _hubContext.Clients.Group(instrument).SendAsync("ReceivePriceUpdate", price);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error receiving messages from Binance: {ex.Message}");
            }
        }
    }


    public class BinanceResultMessage
    {
        public string e { get; set; }
        public long E { get; set; }
        public string s { get; set; }
        public long a { get; set; }
        public string p { get; set; }
        public string q { get; set; }
        public long f { get; set; }
        public long l { get; set; }
        public long T { get; set; }
        public bool m { get; set; }
        public bool M { get; set; }
    }

}
