using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF.AssetWizzard.Network.Request
{
    public abstract class AssetMetadataRequest : BaseWebPackage
    {

        private Template m_template;


        public AssetMetadataRequest(string url) : base(url) {}

        public override Dictionary<string, object> GenerateData() {
            Dictionary<string, object> OriginalJSON = new Dictionary<string, object>();
            OriginalJSON.Add("data", m_template.ToDictionary());

            return OriginalJSON;
        }


        protected void SetTemplate(Template template) {
            m_template = template;
        }


    }
}
