using Types.FinancialLive;

namespace Interfaces
{
    public interface IFinancialLiveService
    {
        public Task<SubscribeResponse> Subscribe(SubscribeRequest request);
        public Task<UnSubscribeResponse> UnSubscribe(UnSubscribeRequest request);
    }
}
