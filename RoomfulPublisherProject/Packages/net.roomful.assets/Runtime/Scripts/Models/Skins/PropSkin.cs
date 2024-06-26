using System;
using System.Collections.Generic;
using net.roomful.api;
using net.roomful.api.assets;
using UnityEngine;

namespace net.roomful.assets
{
    internal class PropSkin : IPropSkin
    {
        private Texture2D m_overridenThumbnail;
        private Resource m_iconResource = null;
        private readonly PropSkinDataModel m_dataModel;

        public string Id => m_dataModel.Id;

        public Texture2D Thumbnail {
            get {
                if (m_overridenThumbnail == null) {
                    return m_iconResource.Thumbnail;
                }

                return m_overridenThumbnail;
            }
            set => m_overridenThumbnail = value;
        }

        public string Name {
            get => m_dataModel.Name;
            set => m_dataModel.Name = value;
        }

        public DateTime Created => m_dataModel.Created;
        public DateTime Updated => m_dataModel.Updated;

        public int SortOrder {
            get => m_dataModel.SortOrder;
            set => m_dataModel.SortOrder = value;
        }

        public string VariantId {
            get => m_dataModel.VariantId;
            set => m_dataModel.VariantId = value;
        }

        public bool IsDefault {
            get => m_dataModel.IsDefault;
            set => m_dataModel.IsDefault = value;
        }

        public bool HeavySkin {
            get => m_dataModel.HeavySkin;
            set => m_dataModel.HeavySkin = value;
        }

        public bool ColorOnly {
            get => m_dataModel.ColorOnly;
            set => m_dataModel.ColorOnly = value;
        }

        public Color OverrideColor {
            get => m_dataModel.OverrideColor;
            set => m_dataModel.OverrideColor = value;
        }

        public void GetThumbnail(ThumbnailSize size, Action<Texture2D> callback) {
            throw new NotImplementedException();
        }

        private List<AssetUrl> Urls {
            get => m_dataModel.Urls;
            set => m_dataModel.Urls = value;
        }

        public List<string> AvailablePlatforms { get; } = new List<string>();

        public PropSkin(string name, string variantId) {
            m_dataModel = new PropSkinDataModel();

            Name = name;
            VariantId = variantId;
            m_iconResource = new Resource();
        }

        public PropSkin(JSONData metaData) {
            m_dataModel = new PropSkinDataModel(metaData);
            m_iconResource = m_dataModel.ThumbnailData != null
                ? new Resource(m_dataModel.ThumbnailData.RawData)
                : new Resource();

            foreach (var url in Urls) {
                AvailablePlatforms.Add(url.Platform);
            }
        }

        public Dictionary<string, object> ToDictionary() {
            var data = m_dataModel.ToDictionary();

            // we want to override the thumbnail key
            if (m_iconResource != null) {
                data["thumbnail"] = m_iconResource.ToDictionary();
            }

            return data;
        }

        public void SetIconResource(Resource iconResource) {
            m_iconResource = iconResource;
        }
    }
}
