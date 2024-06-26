using System.Collections.Generic;
using JetBrains.Annotations;
using net.roomful.api;
using UnityEngine;

namespace net.roomful.assets
{
    internal class StyleAssetTemplate : AssetTemplate
    {
        public StyleMetadata Metadata = null;
        private StyleAssetDataModel m_dataModel;
        
        public StyleDoorsType DoorsType {
            get => m_dataModel.DoorsType;
            set => m_dataModel.DoorsType = value;
        }
        
        public int Score {
            get => m_dataModel.Score;
            set => m_dataModel.Score = value;
        }
        
        public decimal Price {
            get => m_dataModel.Price;
            set => m_dataModel.Price = value;
        }
        
        public StyleType StyleType {
            get => m_dataModel.StyleType;
            set => m_dataModel.StyleType = value;
        }
        
        public Vector3 HomePosition {
            get => m_dataModel.HomePosition;
            set => m_dataModel.HomePosition = value;
        }

        public StyleAssetTemplate() {
            m_dataModel = new StyleAssetDataModel();
        }
        
        [UsedImplicitly]
        public StyleAssetTemplate(string data) : base(data) { }
        
        public override Dictionary<string, object> ToDictionary()
        {
            var data = base.ToDictionary();
            m_dataModel.AppendDictionary(data);
            data.Add("params", Metadata.ToDictionary());
            return data;
        }

        public override void ParseData(JSONData assetData)
        {
            base.ParseData(assetData);
            m_dataModel = new StyleAssetDataModel(assetData);
            if (assetData.HasValue("params"))
            {
                var meta = new JSONData(assetData.GetValue<Dictionary<string, object>>("params"));
                Metadata = new StyleMetadata(meta);
            }
        }
    }
}
