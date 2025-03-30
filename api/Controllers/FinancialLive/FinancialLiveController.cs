using Implementation.FinancialLive;
using Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Types.FinancialLive;

namespace Controllers.FinancialLive
{
    [ApiController]
    [Route("[controller]")]
    public class FinancialLiveController : ControllerBase
    {
        private readonly ILogger<FinancialLiveController> _logger;
        private readonly IFinancialLiveService _financialLiveService;

        public FinancialLiveController( 
            ILogger<FinancialLiveController> logger, 
            IFinancialLiveService financialLiveService)
        {
            _logger = logger;
            _financialLiveService = financialLiveService;
        }

        [HttpPost("subscribe")]
        public async Task<SubscribeResponse> Subscribe(SubscribeRequest request) => await _financialLiveService.Subscribe(request);

        [HttpPost("unSubscribe")]
        public async Task<UnSubscribeResponse> UnSubscribe(UnSubscribeRequest request) => await _financialLiveService.UnSubscribe(request);
    }
}
