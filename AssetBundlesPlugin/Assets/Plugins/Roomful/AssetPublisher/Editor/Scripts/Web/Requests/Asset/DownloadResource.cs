namespace RF.AssetWizzard.Network.Request {
    
    public class DownloadResource : BaseWebPackage {
        
        public override bool IsDataPack => true;
        public override string Url => m_url;
        public override RequestMethods MethodName => RequestMethods.GET;
        
        private string m_url;

        public DownloadResource(string url) {
            m_url = url;
        }
    }
}
