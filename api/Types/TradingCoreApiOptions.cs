using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Types
{
    public class TradingCoreApiOptions
    {
        public bool UseSandbox { get; set; }
        public Tiigo Tiigo { get; set; }
    }

    public class Tiigo
    {
        public string ApiKey { get; set; }
        public string BaseUrl { get; set; }
    }
}
