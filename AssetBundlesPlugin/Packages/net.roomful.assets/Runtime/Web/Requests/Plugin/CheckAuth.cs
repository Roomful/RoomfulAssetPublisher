using System.Collections.Generic;

namespace net.roomful.assets.Network.Request {
    public class CheckAuth : BaseWebPackage  {

        public const string PackUrl = "/auth/check";
        
        public CheckAuth():base(PackUrl, RequestMethods.GET) {
            
        }
        
        public override Dictionary<string, object> GenerateData () {
            Dictionary<string, object> OriginalJSON =  new Dictionary<string, object>();
            return OriginalJSON;
        }
    }
}