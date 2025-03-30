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
    public class InstrumentInfo
    {
        [DataMember(Name = "instrument")]
        [JsonPropertyName("instrument")]
        public string Instrument { get; set; }

        [DataMember(Name = "price")]
        [JsonPropertyName("price")]
        public decimal Price { get; set; }
    }
}
