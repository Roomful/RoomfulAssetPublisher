using System;
using System.Collections.Generic;
using net.roomful.api;

namespace net.roomful.assets
{
    [Serializable]
    public class StyleTemplate : Template
    {

        public StyleMetadata Metadata = null;


        public StyleTemplate():base() { }
        public StyleTemplate(string data) : base(data) { }


  

        public override Dictionary<string, object> ToDictionary() {
            var OriginalJSON = base.ToDictionary();
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
