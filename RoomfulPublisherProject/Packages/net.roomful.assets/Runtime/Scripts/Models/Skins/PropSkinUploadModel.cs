using System.Collections.Generic;
using JetBrains.Annotations;
using net.roomful.api;

namespace net.roomful.assets
{
    internal class PropSkinUploadModel : PropAssetTemplate
    {
        public string AssetId { get; private set; }
        public string AssetTitle { get; private set; }

        public string SkinId => Id;

        public PropVariant Variant { get; }

        public PropSkinUploadModel (AssetTemplate tpl, PropSkin skin, PropVariant variant) {
            
            base.ParseData(new JSONData(tpl.ToDictionary()));
            
            Id = skin.Id;
            Title = skin.Name;
            AssetId = tpl.Id;
            AssetTitle = tpl.Title;

            Variant = variant;
        }
        
        [UsedImplicitly]
        public PropSkinUploadModel(string data) : base(data) { }
        
        public override Dictionary<string, object> ToDictionary() {
            var data = base.ToDictionary();
            data.Add("assetId", AssetId);
            data.Add("assetTitle", AssetTitle);
            return data;
        }

        public override void ParseData(JSONData assetData) {
            base.ParseData(assetData);
            AssetId = assetData.GetValue<string>("assetId");
            AssetTitle = assetData.GetValue<string>("assetTitle");

        }
    }
}