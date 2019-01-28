using RF.AssetWizzard.Network.Request;

namespace RF.AssetWizzard.Commands {


    public class CreatePropDraftFromReleaseCommand : BaseNetworkCommand<AssetRelatedCommandResult<PropTemplate>> {
        private string m_assetId;

        public CreatePropDraftFromReleaseCommand(string assetId) {
            m_assetId = assetId;
        }

        protected override void ErrorHandler(long obj) {
            FireComplete(new AssetRelatedCommandResult<PropTemplate>());
        }
        protected override BaseWebPackage GetRequest() {
            return new CreateDraftPropFromRelease(m_assetId);
        }

        protected override void SuccessHandler(string responce) {
            FireComplete(new AssetRelatedCommandResult<PropTemplate>(responce));
        }

    }
}
