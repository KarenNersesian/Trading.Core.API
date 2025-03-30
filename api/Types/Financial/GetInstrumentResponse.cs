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
    public class GetInstrumentResponse
    {
        [DataMember(Name = "instrument")]
        [JsonPropertyName("instrument")]
        public Instrument Instrument { get; set; }
    }
}
