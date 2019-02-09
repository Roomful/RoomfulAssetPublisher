using RF.AssetWizzard.Network.Request;
using RF.AssetWizzard.Results;

namespace RF.AssetWizzard.Commands {
    
    public class GetResourceThumbnailUrlCommand : BaseNetworkCommand<GetResourceThumbnailUrlResult> {
        
        private string m_resId;
        
        public GetResourceThumbnailUrlCommand(string resId) {
            m_resId = resId;
        }
        
        protected override BaseWebPackage GetRequest() {
            return new GetResourceThumbnailUrl(m_resId);
        }

        protected override void SuccessHandler(string response) {
            FireComplete(new GetResourceThumbnailUrlResult(response));
        }

        protected override void ErrorHandler(long obj) {
            FireComplete(new GetResourceThumbnailUrlResult());
        }
    }
}