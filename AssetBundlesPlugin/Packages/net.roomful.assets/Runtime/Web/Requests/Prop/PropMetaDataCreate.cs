using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace net.roomful.assets.Network.Request {
	public class PropMetaDataCreate : AssetMetadataRequest
    {
        public const string RequestUrl = "/api/v0/asset/create";

        public PropMetaDataCreate(PropTemplate template) : base(RequestUrl) {
            SetTemplate(template);
        }
    }
}
