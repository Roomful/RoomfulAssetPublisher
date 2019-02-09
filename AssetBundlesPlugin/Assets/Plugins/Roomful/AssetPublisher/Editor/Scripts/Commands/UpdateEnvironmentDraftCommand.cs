using RF.AssetWizzard.Network.Request;
using RF.AssetWizzard.Results;

namespace RF.AssetWizzard.Commands {

    public class UpdateEnvironmentDraftCommand : BaseNetworkCommand<AssetRelatedCommandResult<EnvironmentTemplate>> {
        private EnvironmentTemplate m_template;

        public UpdateEnvironmentDraftCommand(EnvironmentTemplate template) {
            m_template = template;
        }

        protected override void ErrorHandler(long errorId) {
            FireComplete(new AssetRelatedCommandResult<EnvironmentTemplate>());
        }

        protected override BaseWebPackage GetRequest() {
            return new UpdateEnvironmentDraft(m_template);
        }

        protected override void SuccessHandler(string responce) {
            FireComplete(new AssetRelatedCommandResult<EnvironmentTemplate>(responce));
        }
    }
}
