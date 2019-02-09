using System.Collections.Generic;

namespace RF.AssetWizzard.Network.Request {

    public class GetUserTemplate : BaseWebPackage {

        private const string REQUEST_URL = "/api/v0/rpc/user.info";
        
        public override RequestMethods MethodName => RequestMethods.POST;

        public override string Url => REQUEST_URL;

        public override Dictionary<string, object> GetRequestData() {
            return new Dictionary<string, object>();
        }
    }
}