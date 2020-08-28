using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using RF.AssetWizzard.Network.Request;

namespace RF.AssetWizzard.Editor
{
    public class StyleBundleManager : BundleManager<StyleTemplate, StyleAsset>
    {

        public override void CreateAsset(StyleTemplate tpl) {
            StyleAsset asset = new GameObject(tpl.Title).AddComponent<StyleAsset>();
            asset.SetTemplate(tpl);

            Selection.activeGameObject = asset.gameObject;
        }

        public override IAsset CreateDownloadedAsset(StyleTemplate tpl, GameObject gameObject) {
            StyleAsset asset = gameObject.AddComponent<StyleAsset>();
            asset.SetTemplate(tpl);

            Selection.activeGameObject = asset.gameObject;

            return asset;
        }


        protected override bool IsAssetValid(StyleAsset asset) {
            return Validation.Run(asset);
        }

        protected override AssetMetadataRequest GenerateMeta_Create_Request(StyleAsset asset) {
            return new StyleMetaDataCreate(asset.Template);
        }

        protected override AssetMetadataRequest GenerateMeta_Update_Request(StyleAsset asset) {
            return new StyleMetaDataUpdate(asset.Template);
        }
        
    }
}