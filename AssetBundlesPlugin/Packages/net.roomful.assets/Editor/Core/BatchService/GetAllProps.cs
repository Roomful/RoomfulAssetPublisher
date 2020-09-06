﻿using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using net.roomful.assets.Network.Request;
using StansAssets.Foundation;

namespace net.roomful.assets.Editor {
    public class GetAllProps {
        private Action<List<PropTemplate>> m_callback;
        private List<PropTemplate> m_templates = new List<PropTemplate>();
        private const int m_loadSize = 10;
        
        public void DownloadAll(Action<List<PropTemplate>> callback) {
            m_callback = callback;
            DownloadNextPage();    
        }

        private void DownloadNextPage() {
            var allAssetsRequest = new GetPropsList(m_templates.Count, m_loadSize, string.Empty);
            allAssetsRequest.PackageCallbackText = (allAssetsCallback) => {
                List<object> allAssetsList = Json.Deserialize(allAssetsCallback) as List<object>;
                foreach (object assetData in allAssetsList) {
                    PropTemplate at = new PropTemplate(new JSONData(assetData).RawData);
                    m_templates.Add(at);
                }

                if (allAssetsList.Count < m_loadSize) {
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