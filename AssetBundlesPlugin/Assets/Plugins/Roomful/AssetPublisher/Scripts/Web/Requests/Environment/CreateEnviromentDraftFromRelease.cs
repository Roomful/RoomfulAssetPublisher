using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF.AssetWizzard.Network.Request {
	public class CreateEnviromentDraftFromRelease : CreateDraftFromReleaseRequest {
        private const string REQUEST_URL = "/api/v0/assetpublisher/environment/createDraftFromRelease";
        public CreateEnviromentDraftFromRelease(string assetId) : base(assetId) { }

        public override string Url {
            get {
                return REQUEST_URL;
            }
        }
    }
}
