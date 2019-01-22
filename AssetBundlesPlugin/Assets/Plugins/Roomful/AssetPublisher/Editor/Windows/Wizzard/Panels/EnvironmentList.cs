using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace RF.AssetWizzard.Editor
{

    public class EnvironmentList : AssetsList<EnvironmentTemplate>
    {

        public EnvironmentList(EditorWindow window) : base(window) { }


        protected override void CreateNewAsset() {
            WindowManager.ShowCreateNewEnvironment();
        }

        protected override List<EnvironmentTemplate> LocalySavedTemplates {
            get {
                return AssetBundlesSettings.Instance.LocalEnvironmentsTemplates;
            }
        }


        protected override void DrawAssetInfo() {

        }


        protected override Network.Request.GetAssetsList CreateAssetsListRequests() {

            Network.Request.GetAllEnvironmentsList listRequest = new Network.Request.GetAllEnvironmentsList(LocalySavedTemplates.Count, 5);
            return listRequest;
        }

    }
}