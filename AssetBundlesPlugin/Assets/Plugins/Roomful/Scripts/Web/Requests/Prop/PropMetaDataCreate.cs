using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF.AssetWizzard.Network.Request {
	public class PropMetaDataCreate : AssetMetaData
    {
        public const string RequestUrl = "/api/v0/asset/create";

        public PropMetaDataCreate(PropTemplate template) : base(RequestUrl) {
            SetTemplate(template);
        }
    }
}
