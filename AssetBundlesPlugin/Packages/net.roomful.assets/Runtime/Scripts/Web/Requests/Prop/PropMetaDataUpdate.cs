namespace net.roomful.assets.Network.Request {
	public class PropMetaDataUpdate : AssetMetadataRequest
    {

        public const string RequestUrl = "/api/v0/asset/update";

        public PropMetaDataUpdate(PropTemplate template) : base(RequestUrl) {
            SetTemplate(template);
        }
    }
}
