namespace net.roomful.assets.Network.Request {
    internal class EnvironmentMetaDataCreate : AssetMetadataRequest
    {

        public const string RequestUrl = "/api/v0/asset/environment/create";

        public EnvironmentMetaDataCreate(EnvironmentAssetTemplate assetTemplate) : base(RequestUrl) {
            SetTemplate(assetTemplate);
        }
       
    }
}
