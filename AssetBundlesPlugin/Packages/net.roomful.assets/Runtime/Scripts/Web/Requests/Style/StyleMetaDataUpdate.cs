namespace net.roomful.assets.Network.Request {
    internal class StyleMetaDataUpdate : AssetMetadataRequest
    {

        public const string RequestUrl = "/api/v0/asset/style/update";

        public StyleMetaDataUpdate(StyleTemplate template) : base(RequestUrl) {
            SetTemplate(template);
        }
    }
}
