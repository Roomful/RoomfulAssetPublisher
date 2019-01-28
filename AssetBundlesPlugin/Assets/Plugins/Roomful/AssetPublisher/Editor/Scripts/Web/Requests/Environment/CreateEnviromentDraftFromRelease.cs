using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF.AssetWizzard.Network.Request {
	public class CreateDraftEnviromentFromRelease : CreateDraftFromReleaseRequest {
        private const string REQUEST_URL = "/api/v0/assetpublisher/environment/createDraftFromRelease";
        public CreateDraftEnviromentFromRelease(string assetId) : base(assetId) { }

        public override string Url {
            get {
                return REQUEST_URL;
            }
        }
    }
}
