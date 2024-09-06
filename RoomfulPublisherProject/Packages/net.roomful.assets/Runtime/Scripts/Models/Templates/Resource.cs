using System;
using UnityEngine;
using System.Collections.Generic;
using net.roomful.api;

namespace net.roomful.assets
{
    class Resource
    {
        ResourceDataModel m_DataModel;
        Texture2D m_Thumbnail;
        string m_ThumbnailData = string.Empty;

        public string Id => m_DataModel.Id;

        //--------------------------------------
        //  Initialization
        //--------------------------------------

        public Resource() {
            m_DataModel = new ResourceDataModel();
        }

        public Resource(string resourceData) {
            var resourceInfo = new JSONData(resourceData);
            ParseTemplate(resourceInfo);
        }

        public Resource(JSONData resourceInfo) {
            ParseTemplate(resourceInfo);
        }

        bool m_thumbnailLoadStarted = false;

        void LoadThumbnail() {
            if (m_thumbnailLoadStarted) {
                return;
            }

            m_thumbnailLoadStarted = true;

            if (!string.IsNullOrEmpty(m_ThumbnailData)) {
                var byteData = Convert.FromBase64String(m_ThumbnailData);
                var texture = new Texture2D(2, 2);
                texture.LoadImage(byteData);
                OnThumbnailLoaded(texture);
                m_thumbnailLoadStarted = false;

                return;
            }

            var getAssetUrl = new GetResourceUrl(m_DataModel.Id);
            getAssetUrl.PackageCallbackText = assetUrl => {
                var loadThumbnail = new DownloadIcon(assetUrl);
                loadThumbnail.PackageCallbackData = data => {
                    var texture = new Texture2D(2, 2);
                    texture.LoadImage(data);
            
                    var byteData = texture.EncodeToPNG();
                    m_ThumbnailData = Convert.ToBase64String(byteData);
            
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

        void FallBackToDefaultTexture() {
            var texture = new Texture2D(32, 32);
            OnThumbnailLoaded(texture);
        }

        public Dictionary<string, object> ToDictionary() => m_DataModel.ToDictionary();

        public Texture2D Thumbnail {
            get {
                if (m_Thumbnail == null) {
                    if (string.IsNullOrEmpty(m_DataModel.Id)) {
                        m_Thumbnail = new Texture2D(32, 32);
                    }
                    else {
                        LoadThumbnail();
                    }
                }

                return m_Thumbnail;
            }

            private set => m_Thumbnail = value;
        }

        void ParseTemplate(JSONData resourceInfo) {
            m_DataModel = new ResourceDataModel(resourceInfo);
        }

        void OnThumbnailLoaded(Texture2D tex) {
            Thumbnail = tex;
        }

        public void SetThumbnail(Texture2D tex)
        {
            Thumbnail = tex;
        }
    }
}