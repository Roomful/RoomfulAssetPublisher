using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF.AssetWizzard.Network.Request {
	public class CreateEnviromentDraft : AssetMetadataRequest<EnvironmentTemplate>
    {
        private const string REQUEST_URL = "/api/v0/assetpublisher/environment/createDraft";
        public CreateEnviromentDraft(EnvironmentTemplate template) : base(template) {}

        public override string Url {
            get {
                return REQUEST_URL;
            }
        }
    }
}
