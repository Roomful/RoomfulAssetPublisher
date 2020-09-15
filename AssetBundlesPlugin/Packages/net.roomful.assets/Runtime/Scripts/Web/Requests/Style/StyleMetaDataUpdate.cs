namespace net.roomful.assets.Network.Request {
    internal class StyleMetaDataUpdate : AssetMetadataRequest
    {

        public const string RequestUrl = "/api/v0/asset/style/update";

        public StyleMetaDataUpdate(StyleAssetTemplate assetTemplate) : base(RequestUrl) {
            SetTemplate(assetTemplate);
        }
    }
}
