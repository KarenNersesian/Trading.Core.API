using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace Types.FinancialLive
{
    [DataContract]
    public class SubscribeRequest
    {
        [DataMember(Name = "subscribeInfo")]
        [JsonPropertyName("subscribeInfo")]
        public SubscribeInfo SubscribeInfo { get; set; }
    }
}
