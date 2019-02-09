using RF.AssetWizzard.Network.Request;
using RF.AssetWizzard.Results;

namespace RF.AssetWizzard.Commands {
    
    public class CreatePropDraftCommand : BaseNetworkCommand<AssetRelatedCommandResult<PropTemplate>> {
        private PropTemplate m_template;

        public CreatePropDraftCommand(PropTemplate template) {
            m_template = template;
        }

        protected override void ErrorHandler(long obj) {
            FireComplete(new AssetRelatedCommandResult<PropTemplate>());
        }

        protected override BaseWebPackage GetRequest() {
            return new CreateDraftProp(m_template);
        }

        protected override void SuccessHandler(string responce) {
            FireComplete(new AssetRelatedCommandResult<PropTemplate>(responce));
        }
    }
}
