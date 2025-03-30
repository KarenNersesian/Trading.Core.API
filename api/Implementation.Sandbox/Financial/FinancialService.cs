using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics.Metrics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
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

        public FinancialService(ILogger<FinancialService> logger) : base(logger)
        {

        }

        public override async Task<GetInstrumentResponse> GetGetInstrument(GetInstrumentRequest request)
        {
            if (_prices.TryGetValue(request.Instrument.ToUpper(), out var price))
                return new GetInstrumentResponse{ Instrument = new InstrumentInfo() { Price = price, Instrument = request.Instrument } };

            // This is a sandbox implementation, need to throw more rebust exception
            throw new Exception();
        }

        public override async Task<GetInstrumentsResponse> GetGetInstruments(GetInstrumentsRequest request)
            => new GetInstrumentsResponse { Instruments = _prices.Select(x => new InstrumentInfo { Instrument = x.Key, Price = x.Value }).ToList() };
    }
}
