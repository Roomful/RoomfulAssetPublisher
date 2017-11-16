using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF.AssetWizzard.Network.Request {
	public class StyleMetaDataUpdate : AssetMetaData
    {

        public const string RequestUrl = "/api/v0/asset/style/update";

        public StyleMetaDataUpdate(StyleTemplate template) : base(RequestUrl) {
            SetTemplate(template);
        }
    }
}
