using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Types.Financial.Tiigo
{
    public class CurrentTopOfBookResponse
    {
        public string ticker { get; set; }
        public DateTime quoteTimestamp { get; set; }
        public float bidPrice { get; set; }
        public float bidSize { get; set; }
        public decimal askPrice { get; set; }
        public float askSize { get; set; }
        public float midPrice { get; set; }
    }
}