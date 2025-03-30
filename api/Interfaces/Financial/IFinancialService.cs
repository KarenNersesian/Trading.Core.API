using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Types.Financial;

namespace Interfaces
{
    public interface IFinancialService
    {
        public Task<GetInstrumentsResponse> GetGetInstruments(GetInstrumentsRequest request);
        public Task<GetInstrumentResponse> GetGetInstrument(GetInstrumentRequest request);
    }
}
