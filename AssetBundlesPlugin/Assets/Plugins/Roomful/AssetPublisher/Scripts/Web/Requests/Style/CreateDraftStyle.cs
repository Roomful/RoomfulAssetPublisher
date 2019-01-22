using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF.AssetWizzard.Network.Request {
	public class CreateDraftStyle : AssetMetadataRequest<StyleTemplate>
    {

        public const string REQUEST_URL = "/api/v0/assetpublisher/style/createDraft";

        public CreateDraftStyle(StyleTemplate template) : base(template) {
        }

        public override string Url {
            get {
                return REQUEST_URL;
            }
        }
    }
}
