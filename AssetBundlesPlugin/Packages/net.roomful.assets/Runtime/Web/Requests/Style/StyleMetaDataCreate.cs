using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace net.roomful.assets.Network.Request {
	public class StyleMetaDataCreate : AssetMetadataRequest
    {

        public const string RequestUrl = "/api/v0/asset/style/create";

        public StyleMetaDataCreate(StyleTemplate template) : base(RequestUrl) {
            SetTemplate(template);
        }
       
    }
}
