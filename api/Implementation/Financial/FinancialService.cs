using Interfaces;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Types;
using Types.Financial;
using Types.Financial.Tiigo;

namespace Implementation.Financial
{
    public class FinancialService : IFinancialService
    {
        protected readonly ILogger<FinancialService> _logger;
        protected readonly TradingCoreApiOptions _options;

        public FinancialService(ILogger<FinancialService> logger, IOptions<TradingCoreApiOptions> options)
        {
            _logger = logger;
            _options = options.Value;
        }

        public virtual async Task<GetInstrumentResponse> GetGetInstrument(GetInstrumentRequest request)
        {
            request.Validate();

            var url = $"{_options.Tiigo.BaseUrl}/tiingo/fx/top?tickers={request.Instrument.ToLower()},eurusd&token={_options.Tiigo.ApiKey}";

            using (var client = new HttpClient())
            {
                var requestTiigo = new HttpRequestMessage(HttpMethod.Get, url);
                requestTiigo.Headers.Accept.Add(new System.Net.Http.Headers.MediaTypeWithQualityHeaderValue("application/json"));

                var response = await client.SendAsync(requestTiigo);

                if (response.IsSuccessStatusCode)
                {
                    var jsonResponse = await response.Content.ReadAsStringAsync();
                    var currentTopOfBookResponse = Newtonsoft.Json.JsonConvert.DeserializeObject<List<CurrentTopOfBookResponse>>(jsonResponse);

                    return new GetInstrumentResponse()
                    {
                        Instrument = new InstrumentInfo()
                        {
                            Instrument = request.Instrument,
                            Price = currentTopOfBookResponse.FirstOrDefault()?.askPrice ?? 0
                        }
                    };
                }
                else
                {
                    Console.WriteLine($"Error: {response.StatusCode}");
                }
            }

            //If arrives here something went wrong
            throw new Exception();
        }

        public virtual async Task<GetInstrumentsResponse> GetGetInstruments(GetInstrumentsRequest request)
        {
            throw new NotImplementedException();
        }
    }
}
