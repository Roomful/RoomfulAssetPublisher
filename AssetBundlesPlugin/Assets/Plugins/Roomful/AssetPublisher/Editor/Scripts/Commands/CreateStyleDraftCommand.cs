using System;
using System.Collections.Generic;
using RF.AssetWizzard.Network.Request;

namespace RF.AssetWizzard.Commands {
    public class CreateStyleDraftCommand : BaseNetworkCommand<AssetRelatedCommandResult<StyleTemplate>> {
        private StyleTemplate m_template;

        public CreateStyleDraftCommand(StyleTemplate template) {
            m_template = template;
        }

        protected override void ErrorHandler(long obj) {
            FireComplete(new AssetRelatedCommandResult<StyleTemplate>());
        }

        protected override BaseWebPackage GetRequest() {
            return new CreateDraftStyle(m_template);
        }

        protected override void SuccessHandler(string responce) {
            FireComplete(new AssetRelatedCommandResult<StyleTemplate>(responce));
        }
    }
}
