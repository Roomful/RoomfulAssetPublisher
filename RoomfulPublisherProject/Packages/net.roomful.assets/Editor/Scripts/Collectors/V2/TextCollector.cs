﻿using UnityEngine;
using net.roomful.assets.serialization;

namespace net.roomful.assets.editor
{
    internal class TextCollector : BaseCollector
    {
        public override void Run(IAssetBundle asset) {
            foreach (var textInfo in asset.gameObject.GetComponentsInChildren<SerializedText>(true)) {
                if (textInfo.FontFileContent != null && textInfo.FontFileContent.Length > 0) {
                    AssetDatabase.SaveFontAsset(asset, textInfo);
                    textInfo.Font = AssetDatabase.LoadFontAsset(asset, textInfo);
                }

                var text = textInfo.gameObject.AddComponent<RoomfulText>();
                text.Restore(textInfo);
                Object.DestroyImmediate(textInfo);
            }
        }
    }
}