using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF.AssetWizzard
{
    [Serializable]
    public class StyleTemplate : Template
    {

        public StyleMetadata Metadata = null;


        public StyleTemplate():base() { }
        public StyleTemplate(string data) : base(data) { }


  

        public override Dictionary<string, object> ToDictionary() {
            Dictionary<string, object> OriginalJSON = base.ToDictionary();
            OriginalJSON.Add("params", Metadata.ToDictionary());
            return OriginalJSON;
        }



        public override void ParseData(JSONData assetData) {
            base.ParseData(assetData);

            if (assetData.HasValue("params")) {
                var meta = new JSONData(assetData.GetValue<Dictionary<string, object>>("params"));
                Metadata = new StyleMetadata(meta);
            }
        }

    }
}
