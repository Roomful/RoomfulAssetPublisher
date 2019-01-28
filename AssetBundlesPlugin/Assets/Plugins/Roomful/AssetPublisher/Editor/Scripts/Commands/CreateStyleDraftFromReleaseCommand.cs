using RF.AssetWizzard.Network.Request;

namespace RF.AssetWizzard.Commands {


    public class CreateStyleDraftFromReleaseCommand : BaseNetworkCommand<AssetRelatedCommandResult<StyleTemplate>> {
        private string m_assetId;

        public CreateStyleDraftFromReleaseCommand(string assetId) {
            m_assetId = assetId;
        }

        protected override void ErrorHandler(long obj) {
            FireComplete(new AssetRelatedCommandResult<StyleTemplate>());
        }
        protected override BaseWebPackage GetRequest() {
            return new CreateDraftStyleFromRelease(m_assetId);
        }

        protected override void SuccessHandler(string responce) {
            FireComplete(new AssetRelatedCommandResult<StyleTemplate>(responce));
        }

    }
}
