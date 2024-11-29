using System;
using System.Collections.Generic;
using net.roomful.api;
using UnityEngine;

namespace net.roomful.assets
{
    internal class PropVariant : IPropVariant
    {
        private Texture2D m_overridenThumbnail;
        private Resource m_iconResource = null;
        private readonly PropVariantDataModel m_dataModel;

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

        public int SortOrder {
            get => m_dataModel.SortOrder;
            set => m_dataModel.SortOrder = value;
        }

        public bool HasColorSupport {
            get => m_dataModel.HasColorSupport;
            set => m_dataModel.HasColorSupport = value;
        }
        
        public bool IsHidden {
            get => m_dataModel.IsHidden;
            set => m_dataModel.IsHidden = value;
        }

        public Color DefaultColor {
            get => m_dataModel.DefaultColor;
            set => m_dataModel.DefaultColor = value;
        }

        public DateTime Created => m_dataModel.Created;
        public DateTime Updated => m_dataModel.Updated;
        public void GetThumbnail(ThumbnailSize size, Action<Texture2D> callback)
        {
            throw new NotImplementedException();
        }


        public List<string> GameobjectsNames {
            get => m_dataModel.GameobjectsNames;
            private set => m_dataModel.GameobjectsNames = value;
        }

        public List<GameObject> GameObjects {
            get => m_dataModel.GameObjects;
            private set => m_dataModel.GameObjects = value;
        }

        private List<PropSkin> m_skins = new List<PropSkin>();

        public PropVariant(string name, IEnumerable<GameObject> gameObjects, Transform graphicsRoot) {
            m_dataModel = new PropVariantDataModel();

            Name = name;
            m_iconResource = new Resource();
            GameObjects = new List<GameObject>(gameObjects);
            GameobjectsNames = new List<string>();
            foreach (var gameObject in GameObjects) {
                GameobjectsNames.Add(SkinUtility.GetGameObjectPath(gameObject.transform, graphicsRoot));
            }
        }


        public PropVariant(JSONData metaData) {
            m_dataModel = new PropVariantDataModel(metaData);
            m_iconResource = m_dataModel.ThumbnailData != null
                ? new Resource(m_dataModel.ThumbnailData.RawData)
                : new Resource();
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

        public void FinRenderers(Transform graphicsRoot) {
            m_dataModel.GameObjects = new List<GameObject>();
            m_dataModel.FinAssignedRenderers(graphicsRoot, m_dataModel.GameObjects);
        }

        public void AddSkin(PropSkin skin) {
            m_skins.Add(skin);
        }

        public void RemoveSkin(PropSkin skin) {
            m_skins.Remove(skin);
        }

        public IEnumerable<PropSkin> Skins => m_skins;

        public void SetSkins(List<PropSkin> skins) {
            m_skins = skins;
        }
    }
}
