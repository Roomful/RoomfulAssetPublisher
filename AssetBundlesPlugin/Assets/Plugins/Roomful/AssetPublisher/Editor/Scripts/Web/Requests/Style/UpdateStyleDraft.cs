using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF.AssetWizzard.Network.Request {
	public class UpdateStyleDraft : AssetMetadataRequest<StyleTemplate>
    {
        private const string REQUEST_URL = "/api/v0/assetpublisher/prop/updateDraft";

        public UpdateStyleDraft(StyleTemplate template): base(template) {
        }

        public override string Url {
            get {
                return REQUEST_URL;
            }
        }
    }
}
