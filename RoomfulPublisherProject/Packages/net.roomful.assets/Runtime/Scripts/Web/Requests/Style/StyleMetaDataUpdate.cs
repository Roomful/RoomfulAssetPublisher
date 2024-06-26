namespace net.roomful.assets {
    internal class StyleMetaDataUpdate : AssetMetadataRequest
    {
        private const string REQUEST_URL = "/api/v0/asset/style/update";

        public StyleMetaDataUpdate(StyleAssetTemplate assetTemplate) : base(REQUEST_URL) {
            SetTemplate(assetTemplate);
        }
    }
}
