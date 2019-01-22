namespace RF.AssetWizzard.Network.Request {
    public class UpdatePropDraft : AssetMetadataRequest<PropTemplate> {

        private const string REQUEST_URL = "/api/v0/assetpublisher/prop/updateDraft";

        public UpdatePropDraft(PropTemplate template) : base(template) { }

        public override string Url {
            get {
                return REQUEST_URL;
            }
        }
    }
}
