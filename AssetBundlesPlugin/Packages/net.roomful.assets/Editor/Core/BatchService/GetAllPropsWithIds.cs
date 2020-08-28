using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using RF.AssetWizzard.Network.Request;
using StansAssets.Foundation;

namespace RF.AssetWizzard.Editor {
    public class GetAllPropsWithIds {
        private Action<List<PropTemplate>> m_callback;
        private List<PropTemplate> m_templates = new List<PropTemplate>();
        private const int m_loadSize = 10;
        
        public void DownloadAll(List<string> ids, Action<List<PropTemplate>> callback) {
            m_callback = callback;
            m_ids = ids;
            DownloadNextPage();    
        }

        private List<string> m_ids = new List<string>();

        private void DownloadNextPage() {
            var allAssetsRequest = new GetPropsList(0, 1, string.Empty);
            allAssetsRequest.SetId(m_ids[0]);
            m_ids.RemoveAt(0);
            allAssetsRequest.PackageCallbackText = (allAssetsCallback) => {
                List<object> allAssetsList = Json.Deserialize(allAssetsCallback) as List<object>;
                foreach (object assetData in allAssetsList) {
                    PropTemplate at = new PropTemplate(new JSONData(assetData).RawData);
                    m_templates.Add(at);
                }

                if (m_ids.Count == 0) {
                    FireCallback();
                } else {
                    DownloadNextPage();
                }
            };

            allAssetsRequest.Send();
        }

        private void FireCallback() {
            m_callback.Invoke(m_templates);
        }
    }
}