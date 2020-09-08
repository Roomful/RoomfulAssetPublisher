namespace net.roomful.assets.Network.Request {
	public class StyleMetaDataUpdate : AssetMetadataRequest
    {

        public const string RequestUrl = "/api/v0/asset/style/update";

        public StyleMetaDataUpdate(StyleTemplate template) : base(RequestUrl) {
            SetTemplate(template);
        }
    }
}
