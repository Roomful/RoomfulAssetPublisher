using System;
using System.Collections.Generic;
using UnityEngine;

namespace net.roomful.api
{
    public class PropAssetDataModel
    {
        public PlacingType Placing { get; set; } = PlacingType.Wall;
        public PropInvokeType InvokeType { get; set; } = PropInvokeType.Default;
        public float MinSize { get; set; } = 0.5f;
        public float MaxSize { get; set; } = 2f;
        public Vector3 Size { get; set; } = Vector3.one;
        public bool CanStack { get; set; } = false;
        public bool HasVariants { get; set; } = false;
        public int LogoCount { get; set; }
        public int ThumbnailCount { get; set; }
        public List<ContentType> ContentTypes { get; set; } = new List<ContentType>();

        public PropAssetDataModel() { }

        public PropAssetDataModel(JSONData assetData) {
            Placing = EnumUtility.ParseEnum<PlacingType>(assetData.GetValue<string>("placing"));
            var invType = assetData.GetValue<string>("invokeType");
            InvokeType = EnumUtility.ParseEnum<PropInvokeType>(invType);

            MinSize = assetData.GetValue<float>("minScale");
            MaxSize = assetData.GetValue<float>("maxScale");
            CanStack = assetData.GetValue<bool>("canStack");

            var sizeData = new JSONData(assetData.GetValue<Dictionary<string, object>>("size"));
            Size = new Vector3 {
                x = sizeData.GetValue<float>("x"),
                y = sizeData.GetValue<float>("y"),
                z = sizeData.GetValue<float>("z")
            };

            if (assetData.HasValue("contentType")) {
                var types = assetData.GetValue<List<object>>("contentType");
                foreach (var type in types) {
                    var typeName = Convert.ToString(type);
                    if (EnumUtility.TryParseEnum(typeName, out ContentType ct)) {
                        if (!ContentTypes.Contains(ct)) {
                            ContentTypes.Add(ct);
                        }
                    }
                }
            }

            if (assetData.HasValue("hasVariants")) {
                HasVariants = assetData.GetValue<bool>("hasVariants");
            }

            if (assetData.HasValue("assetBundleMeta")) {
                var bundleMeta = new JSONData(assetData.GetValue<Dictionary<string, object>>("assetBundleMeta"));
                if (bundleMeta.HasValue("logoCount")) {
                    LogoCount = bundleMeta.GetValue<int>("logoCount");
                } 
                if (bundleMeta.HasValue("thumbnailCount")) {
                    ThumbnailCount = bundleMeta.GetValue<int>("thumbnailCount");
                }
            }
        }

        public Dictionary<string, object> ToDictionary() {
            var data = new Dictionary<string, object>();

            AppendDictionary(data);
            return data;
        }

        public void AppendDictionary(Dictionary<string, object> data) {
            data.Add("placing", Placing.ToString());
            data.Add("invokeType", InvokeType.ToString());
            data.Add("minScale", MinSize);
            data.Add("maxScale", MaxSize);
            data.Add("canStack", CanStack);

            var sizeData = new Dictionary<string, object>();
            sizeData.Add("x", Size.x);
            sizeData.Add("y", Size.y);
            sizeData.Add("z", Size.z);
            data.Add("size", sizeData);
            var contTypes = new List<object>();
            foreach (var t in ContentTypes) {
                contTypes.Add(t.ToString());
            }

            data.Add("contentType", contTypes);
        }
    }
}