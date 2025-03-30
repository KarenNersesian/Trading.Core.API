using Implementation.SocketProviders.Binance;
using Microsoft.Extensions.DependencyInjection;
using Interfaces;
using Implementation.Financial;
using Implementation.FinancialLive;

namespace Implementation
{
    public static class TradingCoreApiDIExtensions
    {
        public static IServiceCollection AddTradingCoreApiServices(this IServiceCollection services)
        {
            services.AddSignalR();
            services.AddSingleton<PriceHub>();
            services.AddSingleton<BinanceWebSocketClient>();
            services.AddScoped<IFinancialService, FinancialService>();
            services.AddScoped<IFinancialLiveService, FinancialLiveService>();

            return services;
        }
    }
}
