using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace RF.AssetWizzard.Editor
{

    public class StylesList : AssetsList<StyleTemplate>
    {

        public StylesList(EditorWindow window) : base(window) { }


        protected override void CreateNewAsset() {
            WindowManager.ShowCreateNewStyle();
        }


        protected override List<StyleTemplate> LocalySavedTemplates {
            get {
                return AssetBundlesSettings.Instance.LocalStyleTemplates;
            }
        }


        protected override void DrawAssetInfo() {

          
        }


        protected override Network.Request.GetAssetsList CreateAssetsListRequests() {

            Network.Request.GetAllStylesList listRequest = new RF.AssetWizzard.Network.Request.GetAllStylesList(LocalySavedTemplates.Count, 5);

            return listRequest;
        }

    }
}