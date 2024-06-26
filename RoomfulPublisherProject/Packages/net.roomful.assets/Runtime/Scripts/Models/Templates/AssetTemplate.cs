using System;
using System.Collections.Generic;
using UnityEngine;
using net.roomful.api;
using net.roomful.api.assets;
using StansAssets.Foundation;

namespace net.roomful.assets
{
    enum AssetStatus
    {
        workInProgress,
        published
    }
    class AssetTemplate : ISerializableTemplate
    {
        public Resource Icon;
        AssetDataModel m_DataModel;
        
        // So far we will keep it here, since we don't need it 
        // in the roomful Runtime app, but if we will will me moved to AssetDataModel

        string m_Ownership;

        public AssetStatus Status { get; set; }

        /// <summary>
        /// empty - public asset; network:{networkId} - belongs to network
        /// </summary>
        public string NetworkId
        {
            get => m_Ownership;
            set => m_Ownership = value;
        }

        public virtual string Id {
            get => m_DataModel.Id;
            set => m_DataModel.Id = value;
        }

        public string Title {
            get => m_DataModel.Title;
            set => m_DataModel.Title = value;
        }

        public List<string> Tags => m_DataModel.Tags;
        public List<AssetUrl> Urls => m_DataModel.Urls;
        public DateTime Created => m_DataModel.Created;

        public DateTime Updated => m_DataModel.Updated;

        public bool IsNew => string.IsNullOrEmpty(m_DataModel.Id);

        //--------------------------------------
        // Initialization
        //--------------------------------------

        protected AssetTemplate() {
            m_DataModel = new AssetDataModel();
            Icon = new Resource();
        }

        public AssetTemplate(string data) {
            LoadData(data);
        }

        void LoadData(string data) {
            ParseData(new JSONData(data));
        }

        public virtual Dictionary<string, object> ToDictionary() {
            var data = m_DataModel.ToDictionary();
            data.Add("ownership", m_Ownership);
            data.Add("status", Status.ToString());
            // we want to override the thumbnail key
            if (Icon != null) {
                data["thumbnail"] = Icon.ToDictionary();
            }

            return data;
        }

        public virtual void ParseData(JSONData assetData) {
            m_DataModel = new AssetDataModel(assetData);
            
            m_Ownership = assetData.GetValue<string>("ownership");
            if (!string.IsNullOrEmpty(m_Ownership))
            {
                m_Ownership = m_Ownership.Replace("network:", "");
            }
            
            Status = EnumUtility.ParseOrDefault<AssetStatus>(assetData.GetValue<string>("status"));

            Icon = m_DataModel.IconData != null
                ? new Resource(m_DataModel.IconData.RawData)
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

                content.text = m_DataModel.Title;
                return content;
            }
        }
    }
}
