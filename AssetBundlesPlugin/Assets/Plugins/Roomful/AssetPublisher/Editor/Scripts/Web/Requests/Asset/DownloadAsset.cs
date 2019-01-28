namespace RF.AssetWizzard.Network.Request {
    public class DownloadAsset : BaseWebPackage {
        private string m_url;

        public DownloadAsset(string url) {
            m_url = url;
        }

        public override bool IsDataPack {
            get {
                return true;
            }
        }

        public override string Url {
            get {
                return m_url;
            }
        }

        public override RequestMethods MethodName {
            get {
                return RequestMethods.GET;
            }
        }
    }
}
