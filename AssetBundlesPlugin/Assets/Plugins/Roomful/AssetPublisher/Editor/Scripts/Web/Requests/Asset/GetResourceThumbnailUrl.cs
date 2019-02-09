using System.Collections.Generic;

namespace RF.AssetWizzard.Network.Request {

    public class GetResourceThumbnailUrl : BaseWebPackage {
        
        public override RequestMethods MethodName => RequestMethods.GET;
        public override string Url => REQUEST_URL;
        
        private string REQUEST_URL = string.Empty;
        
        public GetResourceThumbnailUrl(string resId) {
            REQUEST_URL = $"/api/v0/resource/thumbnail/url/{resId}";
        }
        
        public override Dictionary<string, object> GetRequestData() {
            return new Dictionary<string, object>();
        }
    }
}