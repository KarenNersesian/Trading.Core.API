using Interfaces;
using Microsoft.Extensions.Logging;
using Types.Financial;

namespace Implementation.Financial
{
    public class FinancialService : IFinancialService
    {
        protected readonly ILogger<FinancialService> _logger;

        public FinancialService(ILogger<FinancialService> logger)
        {
            _logger = logger;
        }

        public virtual async Task<GetInstrumentResponse> GetGetInstrument(GetInstrumentRequest request)
        {
            throw new NotImplementedException();
        }

        public virtual async Task<GetInstrumentsResponse> GetGetInstruments(GetInstrumentsRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
