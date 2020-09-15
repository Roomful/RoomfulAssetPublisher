using System;
using UnityEngine;
using System.Collections.Generic;
using net.roomful.api;

namespace net.roomful.assets
{
    internal class Resource
    {
        private ResourceDataModel m_dataModel;
        private Texture2D m_thumbnail = null;
        private string m_thumbnailData = string.Empty;

        //--------------------------------------
        //  Initialization
        //--------------------------------------

        public Resource() {
            m_dataModel = new ResourceDataModel();
        }

        public Resource(string resourceData) {
            var resourceInfo = new JSONData(resourceData);
            ParseTemplate(resourceInfo);
        }

        public Resource(JSONData resourceInfo) {
            ParseTemplate(resourceInfo);
        }

        private bool m_thumbnailLoadStarted = false;

        private void LoadThumbnail() {
            if (m_thumbnailLoadStarted) {
                return;
            }

            m_thumbnailLoadStarted = true;

            if (!string.IsNullOrEmpty(m_thumbnailData)) {
                var byteData = Convert.FromBase64String(m_thumbnailData);
                var texture = new Texture2D(2, 2);
                texture.LoadImage(byteData);
                OnThumbnailLoaded(texture);
                m_thumbnailLoadStarted = false;

                return;
            }

            var getAssetUrl = new Network.Request.GetResourceUrl(m_dataModel.Id);
            getAssetUrl.PackageCallbackText = assetUrl => {
                var loadThumbnail = new Network.Request.DownloadIcon(assetUrl);
                loadThumbnail.PackageCallbackData = data => {
                    var texture = new Texture2D(2, 2);
                    texture.LoadImage(data);

                    var byteData = texture.EncodeToPNG();
                    m_thumbnailData = Convert.ToBase64String(byteData);

                    OnThumbnailLoaded(texture);
                    m_thumbnailLoadStarted = false;
                };

                loadThumbnail.PackageCallbackError = code => {
                    FallBackToDefaultTexture();
                    m_thumbnailLoadStarted = false;
                };

                loadThumbnail.Send();
            };

            getAssetUrl.PackageCallbackError = errorCode => {
                FallBackToDefaultTexture();
                m_thumbnailLoadStarted = false;
            };

            getAssetUrl.Send();
        }

        private void FallBackToDefaultTexture() {
            var texture = new Texture2D(32, 32);
            OnThumbnailLoaded(texture);
        }

        public Dictionary<string, object> ToDictionary() => m_dataModel.ToDictionary();

        public Texture2D Thumbnail {
            get {
                if (m_thumbnail == null) {
                    if (string.IsNullOrEmpty(m_dataModel.Id)) {
                        m_thumbnail = new Texture2D(32, 32);
                    }
                    else {
                        LoadThumbnail();
                    }
                }

                return m_thumbnail;
            }

            private set => m_thumbnail = value;
        }

        private void ParseTemplate(JSONData resourceInfo) {
            m_dataModel = new ResourceDataModel(resourceInfo);
        }

        private void OnThumbnailLoaded(Texture2D tex) {
            Thumbnail = tex;
        }
    }
}