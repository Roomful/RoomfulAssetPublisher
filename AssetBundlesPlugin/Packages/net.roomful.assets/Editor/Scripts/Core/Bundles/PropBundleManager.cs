using UnityEngine;
using net.roomful.assets.Network.Request;

namespace net.roomful.assets.Editor
{
    internal class PropBundleManager : BundleManager<PropTemplate, PropAsset>
    {
        protected override void CreateAsset(PropTemplate tpl) {
            var createdProp = new GameObject(tpl.Title).AddComponent<PropAsset>();
            createdProp.SetTemplate(tpl);
        }

        protected override IAsset CreateDownloadedAsset(PropTemplate tpl, GameObject gameObject) {
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
            return new PropMetaDataUpdate(asset.Template);
        }
    }
}