using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF.AssetWizzard.Network.Request {
	public class CreateDraftStyleFromRelease : CreateDraftFromReleaseRequest {
        private const string REQUEST_URL = "/api/v0/assetpublisher/style/createDraftFromRelease";
        public CreateDraftStyleFromRelease(string assetId) : base(assetId) { }

        public override string Url {
            get {
                return REQUEST_URL;
            }
        }
    }
}
