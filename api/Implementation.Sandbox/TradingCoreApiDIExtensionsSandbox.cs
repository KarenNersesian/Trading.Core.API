using Implementation.Sandbox.Financial;
using Interfaces;
using Microsoft.Extensions.DependencyInjection;
using System.Data;

namespace Implementation.Sandbox
{
    public static class TradingCoreApiDIExtensionsSandbox
    {
        public static IServiceCollection AddTradingCoreApiDIExtensionsSandbox(this IServiceCollection services)
        {
            services.Remove<Implementation.Financial.FinancialService>()
                .AddScoped<IFinancialService, FinancialService>();

            return services;
        }

        private static IServiceCollection Remove<T>(this IServiceCollection services)
        {
            if (services.IsReadOnly)
            {
                throw new ReadOnlyException($"{nameof(services)} is read only");
            }

            var serviceDescriptor = services.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(T));
            if (serviceDescriptor != null) services.Remove(serviceDescriptor);

            return services;
        }
    }
}
