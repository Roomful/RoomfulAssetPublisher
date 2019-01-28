using System.Collections.Generic;

namespace RF.AssetWizzard.Network.Request {
    public class CheckAuth : BaseWebPackage  {

        private const string PACK_URL = "/auth/check";
        
        public CheckAuth() {
            
        }

        public override RequestMethods MethodName {
            get {
                return RequestMethods.GET;
            }
        }

        public override string Url {
            get {
                return PACK_URL;
            }
        }

        public override Dictionary<string, object> GetRequestData () {
            return new Dictionary<string, object>();
        }
    }
}