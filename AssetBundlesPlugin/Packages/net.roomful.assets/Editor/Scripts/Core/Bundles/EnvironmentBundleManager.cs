using UnityEngine;
using net.roomful.assets.Network.Request;

namespace net.roomful.assets.Editor
{
    internal class EnvironmentBundleManager : BundleManager<EnvironmentTemplate, EnvironmentAsset>
    {
        protected override void CreateAsset(EnvironmentTemplate tpl) {
            var asset = new GameObject(tpl.Title).AddComponent<EnvironmentAsset>();
            asset.SetTemplate(tpl);
        }

        protected override IAsset CreateDownloadedAsset(EnvironmentTemplate tpl, GameObject gameObject) {
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