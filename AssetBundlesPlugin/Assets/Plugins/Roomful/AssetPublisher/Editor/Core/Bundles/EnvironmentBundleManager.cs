using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RF.AssetWizzard.Network.Request;

namespace RF.AssetWizzard.Editor
{
    public class EnvironmentBundleManager : BundleManager<EnvironmentTemplate, EnvironmentAsset>
    {

        public override void CreateAsset(EnvironmentTemplate tpl) {
            EnvironmentAsset asset = new GameObject(tpl.Title).AddComponent<EnvironmentAsset>();
            asset.SetTemplate(tpl);
        }

        public override IAsset CreateDownloadedAsset(EnvironmentTemplate tpl, GameObject gameObject) {
            EnvironmentAsset asset = gameObject.AddComponent<EnvironmentAsset>();
            asset.SetTemplate(tpl);

            return asset;
        }


        protected override bool IsAssetValid(EnvironmentAsset asset) {
            return Validation.Run(asset);
        }

        protected override BaseWebPackage GenerateMeta_Create_Request(EnvironmentAsset asset) {
            return new CreateDraftEnviroment(asset.Template);
        }

        protected override BaseWebPackage GenerateMeta_Update_Request(EnvironmentAsset asset) {
            return new UpdateEnvironmentDraft(asset.Template);
        }
        
    }
}