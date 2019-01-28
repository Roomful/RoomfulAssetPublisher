using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF.AssetWizzard.Network.Request
{
    public abstract class CreateDraftFromReleaseRequest : BaseWebPackage
    {

        private string m_releaseAssetId;
        
        public override Dictionary<string, object> GetRequestData() {
            return new Dictionary<string, object>() {
                {"asset", m_releaseAssetId } };
        }

        public CreateDraftFromReleaseRequest(string releaseAssetId) {
            m_releaseAssetId = releaseAssetId;
        }

    }
}
