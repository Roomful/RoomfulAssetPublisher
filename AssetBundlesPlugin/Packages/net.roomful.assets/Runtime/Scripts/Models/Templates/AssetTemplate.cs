using System;
using System.Collections.Generic;
using UnityEngine;
using net.roomful.api;

namespace net.roomful.assets
{
    internal class AssetTemplate
    {
        public Resource Icon = null;
        private AssetDataModel m_dataModel;

        public string Id {
            get => m_dataModel.Id;
            set => m_dataModel.Id = value;
        }

        public string Title {
            get => m_dataModel.Title;
            set => m_dataModel.Title = value;
        }

        public List<string> Tags => m_dataModel.Tags;
        public List<AssetUrl> Urls => m_dataModel.Urls;
        public DateTime Created => m_dataModel.Created;

        public DateTime Updated => m_dataModel.Updated;

        public bool IsNew => string.IsNullOrEmpty(m_dataModel.Id);

        //--------------------------------------
        // Initialization
        //--------------------------------------

        protected AssetTemplate() {
            m_dataModel = new AssetDataModel();
            Icon = new Resource();
        }

        public AssetTemplate(string data) {
            LoadData(data);
        }

        private void LoadData(string data) {
            ParseData(new JSONData(data));
        }

        public virtual Dictionary<string, object> ToDictionary() {
            var data = m_dataModel.ToDictionary();

            // we want to override the thumbnail key
            if (Icon != null) {
                data["thumbnail"] = Icon.ToDictionary();
            }

            return data;
        }

        public virtual void ParseData(JSONData assetData) {
            m_dataModel = new AssetDataModel(assetData);
            Icon = m_dataModel.IconData != null
                ? new Resource(m_dataModel.IconData.RawData)
                : new Resource();
        }

        //--------------------------------------
        // Get / Set
        //--------------------------------------

        public GUIContent DisplayContent {
            get {
                var content = new GUIContent();
                if (Icon != null && Icon.Thumbnail != null) {
                    content.image = Icon.Thumbnail;
                }

                content.text = m_dataModel.Title;
                return content;
            }
        }
    }
}