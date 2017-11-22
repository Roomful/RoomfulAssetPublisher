using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RF.AssetWizzard.Network.Request;

namespace RF.AssetWizzard.Editor
{
    public class PropBundleManager : BundleManager<PropTemplate, PropAsset>
    {

        public override void CreateAsset(PropTemplate tpl) {
            PropAsset createdProp = new GameObject(tpl.Title).AddComponent<PropAsset>();
            createdProp.SetTemplate(tpl);
        }

        public override IAsset CreateDownloadedAsset(PropTemplate tpl, GameObject gameObject) {
            PropAsset asset = gameObject.AddComponent<PropAsset>();
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