using System.Collections.Generic;

namespace RF.AssetWizzard.Network.Request {
    public abstract class AssetMetadataRequest<T> : BaseWebPackage where T : Template {
        private T m_template;

        protected AssetMetadataRequest(T template) {
            m_template = template;
        }

        public override Dictionary<string, object> GetRequestData() {
            Dictionary<string, object> OriginalJSON = new Dictionary<string, object>();
            OriginalJSON.Add("data", m_template.ToDictionary());
            return OriginalJSON;
        }

    }
}
