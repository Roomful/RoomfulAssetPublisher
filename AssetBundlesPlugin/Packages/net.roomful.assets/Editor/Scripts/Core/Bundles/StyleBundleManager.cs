using UnityEngine;
using UnityEditor;
using net.roomful.assets.Network.Request;

namespace net.roomful.assets.Editor
{
    internal class StyleBundleManager : BundleManager<StyleAssetTemplate, StyleAsset>
    {
        protected override void CreateAsset(StyleAssetTemplate tpl) {
            var asset = new GameObject(tpl.Title).AddComponent<StyleAsset>();
            asset.SetTemplate(tpl);

            Selection.activeGameObject = asset.gameObject;
        }

        protected override IAsset CreateDownloadedAsset(StyleAssetTemplate tpl, GameObject gameObject) {
            var asset = gameObject.AddComponent<StyleAsset>();
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