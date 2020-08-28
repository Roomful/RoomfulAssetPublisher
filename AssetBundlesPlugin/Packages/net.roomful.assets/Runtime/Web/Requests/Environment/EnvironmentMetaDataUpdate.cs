using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF.AssetWizzard.Network.Request {
	public class EnvironmentMetaDataUpdate : AssetMetadataRequest
    {

        public const string RequestUrl = "/api/v0/asset/environment/update";

        public EnvironmentMetaDataUpdate(EnvironmentTemplate template) : base(RequestUrl) {
            SetTemplate(template);
        }
    }
}
