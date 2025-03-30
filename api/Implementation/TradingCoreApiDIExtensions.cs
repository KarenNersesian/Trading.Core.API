using Implementation.SocketProviders.Binance;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.SignalR;
using System.Runtime.CompilerServices;

namespace Implementation
{
    public static class TradingCoreApiDIExtensions
    {
        public static IServiceCollection AddTradingCoreApiServices(this IServiceCollection services)
        {
            services.AddSignalR();
            services.AddSingleton<PriceHub>();
            services.AddSingleton<BinanceWebSocketClient>();

            return services;
        }
    }
}
