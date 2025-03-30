using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Types.Financial
{
    [DataContract]
    public class GetInstrumentRequest
    {
        [DataMember(Name = "symbol")]
        [JsonPropertyName("symbol")]
        public string Symbol { get; set; }
    }
}
