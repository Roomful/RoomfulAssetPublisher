using System;
using System.Collections.Generic;
using RF.AssetWizzard.Network.Request;

namespace RF.AssetWizzard.Commands {

    public class RemoveAssetCommand : BaseNetworkCommand<RemoveAssetResult> {
        private string m_assetId;

        public RemoveAssetCommand(string assetId) {
            m_assetId = assetId;
        }

        protected override void ErrorHandler(long obj) {
            FireComplete(new RemoveAssetResult());
        }
        
        protected override BaseWebPackage GetRequest() {
            return new RemoveAsset(m_assetId);
        }

        protected override void SuccessHandler(string obj) {
            FireComplete(new RemoveAssetResult(m_assetId));
        }
    }
}
