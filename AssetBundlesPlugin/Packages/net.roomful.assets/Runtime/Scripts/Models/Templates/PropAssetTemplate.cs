using System;
using System.Collections.Generic;
using System.Text;
using JetBrains.Annotations;
using net.roomful.api;
using UnityEngine;

namespace net.roomful.assets
{
    internal class PropAssetTemplate : AssetTemplate
    {
        public const float MIN_ALLOWED_AXIS_SIZE = 0.5f;
        public const float MAX_ALLOWED_AXIS_SIZE = 50f;

        private PropAssetDataModel m_dataModel;

        public PlacingType Placing {
            get => m_dataModel.Placing;
            set => m_dataModel.Placing = value;
        }

        public PropInvokeType InvokeType {
            get => m_dataModel.InvokeType;
            set => m_dataModel.InvokeType = value;
        }

        public bool CanStack {
            get => m_dataModel.CanStack;
            set => m_dataModel.CanStack = value;
        }

        public Vector3 Size {
            get => m_dataModel.Size;
            set => m_dataModel.Size = value;
        }

        public List<ContentType> ContentTypes {
            get => m_dataModel.ContentTypes;
            set => m_dataModel.ContentTypes = value;
        }

        //TODO add into model
        private readonly List<PropVariant> m_variants = new List<PropVariant>();
        private readonly Dictionary<Renderer, PropVariant> m_variantByRenderer = new Dictionary<Renderer, PropVariant>();
        public IEnumerable<PropVariant> Variants => m_variants;

        public PropAssetTemplate() : base() {
            m_dataModel = new PropAssetDataModel();
        }

        [UsedImplicitly]
        public PropAssetTemplate(string data) : base(data) { }

        public override Dictionary<string, object> ToDictionary() {
            var data = base.ToDictionary();
            m_dataModel.AppendDictionary(data);
            return data;
        }

        public override void ParseData(JSONData assetData) {
            base.ParseData(assetData);
            m_dataModel = new PropAssetDataModel(assetData);
        }

        public bool ValidateVariantCreate(IEnumerable<GameObject> gameObjects) {
            var usedRenderers = new List<Renderer>();
            var renderers = new List<Renderer>();
            foreach (var go in gameObjects) {
                var renderer = go.GetComponent<Renderer>();
                if (renderer != null) {
                    if (HasVariantForRenderer(renderer)) {
                        usedRenderers.Add(renderer);
                    }
                    else {
                        renderers.Add(renderer);
                    }
                }
            }

            if (usedRenderers.Count > 0) {
                var builder = new StringBuilder();
                builder.AppendLine("Can't create Variant for Selected Renderers collection!");
                builder.AppendLine("Renderers:");
                foreach (var r in usedRenderers) {
                    builder.AppendLine(r.name);
                }

                builder.AppendLine("already in use!");
                Debug.LogWarning(builder.ToString());
                return false;
            }

            if (renderers.Count == 0) {
                return false;
            }

            return true;
        }

        public bool TryCreateVariant(IEnumerable<GameObject> gameObjects, out PropVariant variant, string name) {
            variant = null;

            var usedRenderers = new List<Renderer>();
            var renderers = new List<Renderer>();
            foreach (var go in gameObjects) {
                var renderer = go.GetComponent<Renderer>();
                if (renderer != null) {
                    if (HasVariantForRenderer(renderer)) {
                        usedRenderers.Add(renderer);
                    }
                    else {
                        renderers.Add(renderer);
                    }
                }
            }

            if (usedRenderers.Count > 0) {
                var builder = new StringBuilder();
                builder.AppendLine("Can't create Variant for Selected Renderers collection!");
                builder.AppendLine("Renderers:");
                foreach (var r in usedRenderers) {
                    builder.AppendLine(r.name);
                }

                builder.AppendLine("already in use!");
                Debug.LogWarning(builder.ToString());
                return false;
            }

            if (renderers.Count == 0) {
                return false;
            }

            variant = new PropVariant(name, renderers);
            foreach (var renderer in renderers) {
                m_variantByRenderer[renderer] = variant;
            }

            return true;
        }

        public bool HasVariantForRenderer(Renderer renderer) {
            return m_variantByRenderer.ContainsKey(renderer);
        }

        public void AddVariant(PropVariant variant) {
            m_variants.Add(variant);
        }

        public void RemoveVariant(PropVariant variant) {
            foreach (var renderer in variant.Renderers) {
                m_variantByRenderer.Remove(renderer);
            }

            m_variants.Remove(variant);
        }

        public float MinSize {
            get => m_dataModel.MinSize;
            set {
                value = Math.Max(MIN_ALLOWED_AXIS_SIZE, value);
                m_dataModel.MinSize = value;
            }
        }

        public float MaxSize {
            get => m_dataModel.MaxSize;
            set {
                value = Math.Min(MAX_ALLOWED_AXIS_SIZE, value);
                m_dataModel.MaxSize = value;
            }
        }
    }
}