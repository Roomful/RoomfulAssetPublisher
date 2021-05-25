using System;
using System.Collections.Generic;
using UnityEngine;

namespace net.roomful.api
{
    public class PropSkinDataModel : PropRelatedTemplate
    {
        public string Name { get; set; } = string.Empty;
        public string VariantId { get; set; } = string.Empty;
        public bool IsDefault { get; set; }
        public bool HeavySkin { get; set; }
        public string AssetId { get; set; } = string.Empty;

        public List<AssetUrl> Urls { get; set; } = new List<AssetUrl>();
        public ResourceDataModel ThumbnailData { get; set; }

        /// <summary>
        /// Variant default color.
        /// This is not part of the skin metadata, it is only here in runtime.
        /// Since we do not want to load whole variant meta.
        /// If we would need more variant properties `DefaultColor` and other variant meta properties
        /// might be moved to 1 filed that will represent borrowed <see cref="PropVariantDataModel"/> properties.
        /// </summary>
        public Color DefaultColor { get; set; } = Color.white;

        public Color OverrideColor { get; set; } = Color.white;
        public bool ColorOnly { get; set; }

        public PropSkinDataModel() { }

        public PropSkinDataModel(JSONData metaData) : base(metaData) {
            if (metaData.HasValue("assetId")) {
                AssetId = metaData.GetValue<string>("assetId");
            }

            VariantId = metaData.GetValue<string>("variantId");
            Name = metaData.GetValue<string>("name");
            IsDefault = metaData.GetValue<bool>("isDefault");
            HeavySkin = metaData.GetValue<bool>("simplifiedSkin");
            ColorOnly = metaData.GetValue<bool>("colorOnly");

            var overrideColor = new JSONData(metaData.GetValue<Dictionary<string, object>>("overrideColor"));
            OverrideColor = new Color {
                r = overrideColor.GetValue<float>("r"),
                g = overrideColor.GetValue<float>("g"),
                b = overrideColor.GetValue<float>("b"),
                a = overrideColor.GetValue<float>("b")
            };


            if (metaData.HasValue("defaultColor")) {
                var defaultColor = new JSONData(metaData.GetValue<Dictionary<string, object>>("defaultColor"));
                DefaultColor = new Color {
                    r = defaultColor.GetValue<float>("r"),
                    g = defaultColor.GetValue<float>("g"),
                    b = defaultColor.GetValue<float>("b"),
                    a = defaultColor.GetValue<float>("b")
                };
            }

            if (metaData.HasValue("thumbnail")) {
                var resInfo = new JSONData(metaData.GetValue<Dictionary<string, object>>("thumbnail"));
                ThumbnailData = new ResourceDataModel(resInfo);
            }

            if (metaData.HasValue("urls")) {
                var urlsList = metaData.GetValue<Dictionary<string, object>>("urls");
                if (urlsList != null) {
                    foreach (var pair in urlsList) {
                        var url = new AssetUrl(pair.Key, Convert.ToString(pair.Value));
                        Urls.Add(url);
                    }
                }
            }
        }

        public override Dictionary<string, object> ToDictionary() {
            var data = base.ToDictionary();

            AppendDictionary(data);
            return data;
        }

        public void AppendDictionary(Dictionary<string, object> data) {
            data.Add("name", Name);
            data.Add("variantId", VariantId);
            data.Add("isDefault", IsDefault);
            data.Add("simplifiedSkin", HeavySkin);
            data.Add("colorOnly", ColorOnly);
            data.Add("assetId", AssetId);

            var overrideColor = new Dictionary<string, object>();
            overrideColor.Add("r", OverrideColor.r);
            overrideColor.Add("g", OverrideColor.g);
            overrideColor.Add("b", OverrideColor.b);
            overrideColor.Add("a", OverrideColor.a);
            data.Add("overrideColor", overrideColor);

            if (ThumbnailData != null) {
                data["thumbnail"] = ThumbnailData.ToDictionary();
            }
        }
    }
}
