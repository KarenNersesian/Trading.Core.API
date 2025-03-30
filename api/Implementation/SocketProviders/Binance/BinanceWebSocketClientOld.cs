using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Implementation.SocketProviders.Binance
{
    public class BinanceWebSocketClientOld
    {
        private readonly ClientWebSocket _clientWebSocket = new();
        private readonly Uri _uri = new("wss://stream.binance.com:443/ws/btcusdt");
        private readonly IHubContext<PriceHub> _hubContext;
        private readonly ILogger<BinanceWebSocketClient> _logger;
        private string? _latestPrice;

        public BinanceWebSocketClientOld(IHubContext<PriceHub> hubContext, ILogger<BinanceWebSocketClient> logger)
        {
            _hubContext = hubContext;
            _logger = logger;
        }

        public async void Connect()
        {
            try
            {
                await _clientWebSocket.ConnectAsync(_uri, CancellationToken.None);
                await SendMessageAsync(new
                {
                    method = "SUBSCRIBE",
                    @params = new[] { "btcusdt@aggTrade" },
                    id = 1
                });
                _logger.LogInformation("Connected to Binance WebSocket.");
                _ = Task.Run(ReceiveMessagesAsync);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to connect to Binance WebSocket: {ex.Message}");
            }
        }

        private async Task SendMessageAsync(object message)
        {
            try
            {
                var json = Newtonsoft.Json.JsonConvert.SerializeObject(message);
                var bytes = Encoding.UTF8.GetBytes(json);
                await _clientWebSocket.SendAsync(new ArraySegment<byte>(bytes), WebSocketMessageType.Text, true, CancellationToken.None);
            }
            catch (Exception ex)
            {
                _logger.LogError($"Failed to send message to Binance: {ex.Message}");
            }
        }

        private async Task ReceiveMessagesAsync()
        {
            var buffer = new byte[1024 * 4];
            try
            {
                while (_clientWebSocket.State == WebSocketState.Open)
                {
                    var result = await _clientWebSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
                    var message = Encoding.UTF8.GetString(buffer, 0, result.Count);
                    _latestPrice = message;
                    _logger.LogInformation($"Received from Binance: {message}");
                    await _hubContext.Clients.All.SendAsync("ReceivePriceUpdate", message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError($"Error receiving messages from Binance: {ex.Message}");
            }
        }

        public string? GetLatestPrice(string instrument)
        {
            return _latestPrice;
        }
    }

}
