using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF.AssetWizzard.Network.Request {
	public class StyleMetaDataCreate : AssetMetaData
    {

        public const string RequestUrl = "/api/v0/asset/style/create";

        public StyleMetaDataCreate(StyleTemplate template) : base(RequestUrl) {
            SetTemplate(template);
        }
       
    }
}
