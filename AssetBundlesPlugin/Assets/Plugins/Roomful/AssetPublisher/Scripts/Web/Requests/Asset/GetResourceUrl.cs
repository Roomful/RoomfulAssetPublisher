namespace RF.AssetWizzard.Network.Request {
    public class GetResourceUrl : BaseWebPackage {

        private const string PackUrl = "/api/v0/resource/url/{0}";

        private const RequestMethods PackMethodName = RequestMethods.GET;
        private string m_id;
        public GetResourceUrl(string id) {
            m_id = id;
        }

        public override string Url {
            get {
                return string.Format(PackUrl, m_id);
            }
        }

        public override RequestMethods MethodName {
            get {
                return RequestMethods.GET;
            }
        }

    }
}