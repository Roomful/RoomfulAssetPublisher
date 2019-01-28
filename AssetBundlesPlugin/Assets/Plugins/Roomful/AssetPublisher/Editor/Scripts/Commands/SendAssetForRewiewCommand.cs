using System;
using System.Collections.Generic;
using RF.AssetWizzard.Network.Request;

namespace RF.AssetWizzard.Commands {

    public class SendAssetForReviewCommand : BaseNetworkCommand<SendForReviewResult> {
        private string m_assetId;

        public SendAssetForReviewCommand(string assetId) {
            m_assetId = assetId;
        }

        protected override void ErrorHandler(long obj) {
            FireComplete(new SendForReviewResult());
        }
        
        protected override BaseWebPackage GetRequest() {
            return new SendAssetForReviewRequest(m_assetId);
        }

        protected override void SuccessHandler(string response) {
            Dictionary<string, object> originalJson = SA.Common.Data.Json.Deserialize(response) as Dictionary<string, object>;
            var status = (ReleaseStatus) Enum.Parse(typeof(ReleaseStatus), originalJson["releaseStatus"].ToString());
            FireComplete(new SendForReviewResult(status, m_assetId));
        }
    }
}
