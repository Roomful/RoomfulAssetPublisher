using net.roomful.api;
using UnityEngine;
using UnityEditor;

namespace net.roomful.assets.editor
{
    class StyleBundleManager : BundleManager<StyleAssetTemplate, BaseStyleAsset>
    {
        protected override void CreateAsset(StyleAssetTemplate tpl) 
        {
            if (tpl.StyleType == StyleType.NonExtendable)
            {
                var root = new GameObject("Root");
                var graphics = new GameObject("Graphics");
                graphics.transform.parent = root.transform;
                
                var sceneStyleAsset =  root.AddComponent<SceneStyleAsset>();
                sceneStyleAsset.Icon = tpl.Icon.Thumbnail;
                sceneStyleAsset.SetTemplate(tpl);
                
                var ceiling  = new GameObject(StylePanel.CeilingParentName).transform;
                ceiling.parent = graphics.transform;
                
                var wall  = new GameObject(StylePanel.WallParentName).transform;
                wall.parent = graphics.transform;
                
                var floor  = new GameObject(StylePanel.FloorParentName).transform;
                floor.parent = graphics.transform;
                return;
            }    
            
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

        protected override bool IsAssetValid(BaseStyleAsset asset) {
            return Validation.Run(asset);
        }

        protected override AssetMetadataRequest GenerateMeta_Create_Request(BaseStyleAsset asset) {
            return new StyleMetaDataCreate(asset.Template);
        }

        protected override AssetMetadataRequest GenerateMeta_Update_Request(BaseStyleAsset asset) {
            return new StyleMetaDataUpdate(asset.Template);
        }
    }
}