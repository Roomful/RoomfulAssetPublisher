namespace net.roomful.assets {
    internal class EnvironmentMetaDataUpdate : AssetMetadataRequest
    {

        public const string RequestUrl = "/api/v0/asset/environment/update";

        public EnvironmentMetaDataUpdate(EnvironmentAssetTemplate assetTemplate) : base(RequestUrl) {
            SetTemplate(assetTemplate);
        }
    }
}
