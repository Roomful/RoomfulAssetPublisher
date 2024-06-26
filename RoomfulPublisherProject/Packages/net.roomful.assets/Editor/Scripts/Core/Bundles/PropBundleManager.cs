using UnityEngine;

namespace net.roomful.assets.editor
{
    internal class PropBundleManager : BundleManager<PropAssetTemplate, PropAsset>
    {
        protected override void CreateAsset(PropAssetTemplate tpl) {
            var createdProp = new GameObject(tpl.Title).AddComponent<PropAsset>();
            createdProp.SetTemplate(tpl);
        }

        protected override IAsset CreateDownloadedAsset(PropAssetTemplate tpl, GameObject gameObject) {
            var asset = gameObject.GetComponent<PropAsset>();
            if (asset == null) {
                asset = gameObject.AddComponent<PropAsset>();
            }

            asset.SetTemplate(tpl);

            return asset;
        }

        protected override bool IsAssetValid(PropAsset asset) {
            return Validation.Run(asset);
        }

        protected override AssetMetadataRequest GenerateMeta_Create_Request(PropAsset asset) {
            return new PropMetaDataCreate(asset.Template);
        }

        protected override AssetMetadataRequest GenerateMeta_Update_Request(PropAsset asset) {
            asset.UpdateAssetBundleMeta();
            return new PropMetaDataUpdate(asset.Template);
        }
    }
}
