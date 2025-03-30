using Implementation.SocketProviders.Binance;
using Microsoft.Extensions.DependencyInjection;
using Interfaces;
using Implementation.Financial;
using Implementation.FinancialLive;
using Types;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace Implementation
{
    public static class TradingCoreApiDIExtensions
    {
        public static IServiceCollection AddTradingCoreApiServices(this IServiceCollection services)
        {
            services.Configure<TradingCoreApiOptions>("TradingCoreApiOptions");
            services.AddSignalR();
            services.AddSingleton<PriceHub>();
            services.AddSingleton<BinanceWebSocketClient>();
            services.AddScoped<IFinancialService, FinancialService>();
            services.AddScoped<IFinancialLiveService, FinancialLiveService>();

            return services;
        }

        public static IServiceCollection Configure<T>(this IServiceCollection services, string sectionName) where T : class
        {
            services.AddOptions();
            return services.AddSingleton((Func<IServiceProvider, IConfigureOptions<T>>)((IServiceProvider sp) => new ConfigureOption<T>(sp.GetRequiredService<IConfiguration>().GetSection(sectionName))));
        }

        private sealed class ConfigureOption<T> : IConfigureOptions<T> where T : class
        {
            private readonly IConfiguration _configuration;

            public ConfigureOption(IConfiguration configuration)
            {
                _configuration = configuration;
            }

            void IConfigureOptions<T>.Configure(T options)
            {
                _configuration.Bind(options);
            }
        }
    }
}
