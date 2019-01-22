namespace RF.AssetWizzard.Network.Request {
    public class CreateDraftProp : AssetMetadataRequest<PropTemplate> {
        private const string REQUEST_URL = "/api/v0/assetpublisher/prop/createDraft";

        public CreateDraftProp(PropTemplate template) : base(template) {
        }

        public override string Url {
            get {
                return REQUEST_URL;
            }
        }
    }
}
