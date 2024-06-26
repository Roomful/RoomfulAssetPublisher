namespace net.roomful.assets {
    internal class PropMetaDataUpdate : AssetMetadataRequest
    {

        public const string RequestUrl = "/api/v0/asset/update";

        public PropMetaDataUpdate(PropAssetTemplate assetTemplate) : base(RequestUrl) {
            SetTemplate(assetTemplate);
        }
    }
}
