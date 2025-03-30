using Interfaces.Financial;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Types.Financial;

namespace Implementation.Financial
{
    public class FinancialService : IFinancialService
    {
        public async Task<GetInstrumentResponse> GetGetInstrument(GetInstrumentRequest request)
        {
            throw new NotImplementedException();
        }

        public async Task<GetInstrumentsResponse> GetGetInstruments(GetInstrumentsRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
