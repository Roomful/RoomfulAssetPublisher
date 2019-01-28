using System;
using System.Collections.Generic;
using RF.AssetWizzard.Network.Request;

namespace RF.AssetWizzard.Commands {

    public class UpdatePropDraftCommand : BaseNetworkCommand<AssetRelatedCommandResult<PropTemplate>> {
        private PropTemplate m_template;

        public UpdatePropDraftCommand(PropTemplate template) {
            m_template = template;
        }

        protected override void ErrorHandler(long errorId) {
            FireComplete(new AssetRelatedCommandResult<PropTemplate>());
        }

        protected override BaseWebPackage GetRequest() {
            return new UpdatePropDraft(m_template);
        }

        protected override void SuccessHandler(string responce) {
            FireComplete(new AssetRelatedCommandResult<PropTemplate>(responce));
        }
    }
}
