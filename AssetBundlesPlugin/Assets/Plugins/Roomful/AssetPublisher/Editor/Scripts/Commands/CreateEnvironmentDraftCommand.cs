using RF.AssetWizzard.Network.Request;
using RF.AssetWizzard.Results;

namespace RF.AssetWizzard.Commands {
    
    public class CreateEnvironmentDraftCommand : BaseNetworkCommand<AssetRelatedCommandResult<EnvironmentTemplate>> {
        
        private EnvironmentTemplate m_template;

        public CreateEnvironmentDraftCommand(EnvironmentTemplate template) {
            m_template = template;
        }

        protected override void ErrorHandler(long obj) {
            FireComplete(new AssetRelatedCommandResult<EnvironmentTemplate>());
        }

        protected override BaseWebPackage GetRequest() {
            return new CreateDraftEnviroment(m_template);
        }

        protected override void SuccessHandler(string responce) {
            FireComplete(new AssetRelatedCommandResult<EnvironmentTemplate>(responce));
        }
    }
}
