namespace net.roomful.assets {
    internal class PropMetaDataCreate : AssetMetadataRequest
    {
        public const string RequestUrl = "/api/v0/asset/create";

        public PropMetaDataCreate(PropAssetTemplate assetTemplate) : base(RequestUrl) {
            SetTemplate(assetTemplate);
        }
    }
}
