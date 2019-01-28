using System;
using System.Collections.Generic;
using RF.AssetWizzard.Network.Request;

namespace RF.AssetWizzard.Commands {

    public class UpdateStyleDraftCommand : BaseNetworkCommand<AssetRelatedCommandResult<StyleTemplate>> {
        private StyleTemplate m_template;

        public UpdateStyleDraftCommand(StyleTemplate template) {
            m_template = template;
        }

        protected override void ErrorHandler(long errorId) {
            FireComplete(new AssetRelatedCommandResult<StyleTemplate>());
        }

        protected override BaseWebPackage GetRequest() {
            return new UpdateStyleDraft(m_template);
        }

        protected override void SuccessHandler(string responce) {
            FireComplete(new AssetRelatedCommandResult<StyleTemplate>(responce));
        }
    }
}
