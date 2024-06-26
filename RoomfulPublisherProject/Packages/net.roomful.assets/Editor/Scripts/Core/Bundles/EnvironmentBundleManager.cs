using UnityEngine;

namespace net.roomful.assets.editor
{
    internal class EnvironmentBundleManager : BundleManager<EnvironmentAssetTemplate, EnvironmentAsset>
    {
        protected override void CreateAsset(EnvironmentAssetTemplate tpl) {
            var asset = new GameObject(tpl.Title).AddComponent<EnvironmentAsset>();
            asset.SetTemplate(tpl);
        }

        protected override IAsset CreateDownloadedAsset(EnvironmentAssetTemplate tpl, GameObject gameObject) {
            var asset = gameObject.AddComponent<EnvironmentAsset>();
            asset.SetTemplate(tpl);

            return asset;
        }


        protected override bool IsAssetValid(EnvironmentAsset asset) {
            return Validation.Run(asset);
        }

        protected override AssetMetadataRequest GenerateMeta_Create_Request(EnvironmentAsset asset) {
            return new EnvironmentMetaDataCreate(asset.Template);
        }

        protected override AssetMetadataRequest GenerateMeta_Update_Request(EnvironmentAsset asset) {
            return new EnvironmentMetaDataUpdate(asset.Template);
        }
        
    }
}