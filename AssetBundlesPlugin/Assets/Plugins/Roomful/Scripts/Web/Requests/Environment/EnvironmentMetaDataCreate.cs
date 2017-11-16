using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF.AssetWizzard.Network.Request {
	public class EnvironmentMetaDataCreate : AssetMetaData
    {

        public const string RequestUrl = "/api/v0/asset/environment/create";

        public EnvironmentMetaDataCreate(EnvironmentTemplate template) : base(RequestUrl) {
            SetTemplate(template);
        }
       
    }
}
