using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Types.Financial;
using Types.FinancialLive;

namespace Implementation
{
    // This class contains extension methods for validating requests. For real wordl scenario the validations should be seperated to generic and specific validations with
    // complex Business Rules. Also Validation should be seperated with Technical and Business validations.
    public static class ValidationExtensions
    {
        public static void Validate(this SubscribeRequest request)
        {
            if (request.SubscribeInfo == null)
            {
                // This is a simple example, but in a real-world application, you would want to use a more robust validation library.
                throw new Exception("SubscribeInfo is required");
            }
        }

        public static void Validate(this UnSubscribeRequest request)
        {
            if (request.SubscribeInfo == null)
            {
                // This is a simple example, but in a real-world application, you would want to use a more robust validation library.
                throw new Exception("SubscribeInfo is required");
            }
        }

        public static void Validate(this GetInstrumentRequest request)
        {
            if (request.Instrument == null)
            {
                // This is a simple example, but in a real-world application, you would want to use a more robust validation library.
                throw new Exception("SubscribeInfo is required");
            }
        }
    }
}
