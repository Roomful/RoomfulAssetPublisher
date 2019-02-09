using RF.AssetWizzard.Network.Request;
using RF.AssetWizzard.Results;

namespace RF.AssetWizzard.Commands {

    public class CreateEnvironmentDraftFromReleaseCommand : BaseNetworkCommand<AssetRelatedCommandResult<EnvironmentTemplate>> {
        private string m_assetId;

        public CreateEnvironmentDraftFromReleaseCommand(string assetId) {
            m_assetId = assetId;
        }

        protected override void ErrorHandler(long obj) {
            FireComplete(new AssetRelatedCommandResult<EnvironmentTemplate>());
        }
        protected override BaseWebPackage GetRequest() {
            return new CreateDraftEnviromentFromRelease(m_assetId);
        }

        protected override void SuccessHandler(string responce) {
            FireComplete(new AssetRelatedCommandResult<EnvironmentTemplate>(responce));
        }
    }
}
