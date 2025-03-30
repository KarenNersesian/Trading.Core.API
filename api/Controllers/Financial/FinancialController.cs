using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Controllers.Financial
{
    [ApiController]
    [Route("[controller]")]
    public class FinancialController : ControllerBase
    {
        private readonly ILogger<FinancialController> _logger;

        public FinancialController(ILogger<FinancialController> logger)
        {
            _logger = logger;
        }

    }
}
