using Implementation.FinancialLive;
using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using Types.Financial;
using Types.FinancialLive;

namespace Controllers.Financial
{
    [ApiController]
    [Route("[controller]")]
    public class FinancialController : ControllerBase
    {
        private readonly ILogger<FinancialController> _logger;
        private readonly IFinancialService _financialService;

        public FinancialController(ILogger<FinancialController> logger, IFinancialService financialService)
        {
            _logger = logger;
            _financialService = financialService;
        }

        [HttpPost("getInstruments")]
        public async Task<GetInstrumentsResponse> GetGetInstruments(GetInstrumentsRequest request) => await _financialService.GetGetInstruments(request);

        [HttpPost("getInstrument")]
        public async Task<GetInstrumentResponse> GetGetInstrument(GetInstrumentRequest request) => await _financialService.GetGetInstrument(request);

    }
}
