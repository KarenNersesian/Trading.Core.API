using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Types;
using Types.Financial;

namespace Implementation.Sandbox.Financial
{
    public class FinancialService : Implementation.Financial.FinancialService
    {
        private static readonly Dictionary<string, decimal> _prices = new()
        {
            { "EURUSD", 1.10m },
            { "USDJPY", 150.30m },
            { "BTCUSD", 65000.00m }
        };

        public FinancialService(ILogger<FinancialService> logger, IOptions<TradingCoreApiOptions> options) : base(logger, options)
        {

        }

        public override async Task<GetInstrumentsResponse> GetGetInstruments(GetInstrumentsRequest request)
            => new GetInstrumentsResponse { Instruments = _prices.Select(x => new InstrumentInfo { Instrument = x.Key, Price = x.Value }).ToList() };
    }
}
