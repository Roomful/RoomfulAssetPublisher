namespace RF.AssetWizzard.Network.Request {
    public class UpdateEnviromentDraft : AssetMetadataRequest<EnvironmentTemplate> {

        private const string REQUEST_URL = "/api/v0/assetpublisher/enviroment/updateDraft";

        public UpdateEnviromentDraft(EnvironmentTemplate template) : base(template) {
        }

        public override string Url {
            get {
                return REQUEST_URL;
            }
        }
    }
}
