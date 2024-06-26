using System;
using System.Collections.Generic;
using JetBrains.Annotations;
using net.roomful.api;
using UnityEngine;

namespace net.roomful.assets
{
    class PropAssetTemplate : AssetTemplate
    {
        public const float MinAllowedAxisSize = 0.5f;
        public const float MaxAllowedAxisSize = 50f;

        PropAssetDataModel m_DataModel;

        public PropAssetBundleMeta AssetBundleMeta { get; private set; } = new PropAssetBundleMeta();

        public PlacingType Placing {
            get => m_DataModel.Placing;
            set => m_DataModel.Placing = value;
        }

        public PropInvokeType InvokeType {
            get => m_DataModel.InvokeType;
            set => m_DataModel.InvokeType = value;
        }

        public bool CanStack {
            get => m_DataModel.CanStack;
            set => m_DataModel.CanStack = value;
        }

        public Vector3 Size {
            get => m_DataModel.Size;
            set => m_DataModel.Size = value;
        }

        public List<ContentType> ContentTypes {
            get => m_DataModel.ContentTypes;
            set => m_DataModel.ContentTypes = value;
        }

        public PropAssetTemplate() {
            m_DataModel = new PropAssetDataModel();
        }

        [UsedImplicitly]
        public PropAssetTemplate(string data) : base(data) { }

        public override Dictionary<string, object> ToDictionary() {
            var data = base.ToDictionary();
            m_DataModel.AppendDictionary(data);
            AssetBundleMeta.AppendDictionary(data);
            return data;
        }

        public override void ParseData(JSONData assetData) {
            base.ParseData(assetData);
            m_DataModel = new PropAssetDataModel(assetData);
            AssetBundleMeta = new PropAssetBundleMeta(assetData);
        }

        public float MinSize {
            get => m_DataModel.MinSize;
            set {
                value = Math.Max(MinAllowedAxisSize, value);
                m_DataModel.MinSize = value;
            }
        }

        public float MaxSize {
            get => m_DataModel.MaxSize;
            set {
                value = Math.Min(MaxAllowedAxisSize, value);
                m_DataModel.MaxSize = value;
            }
        }
    }
}
