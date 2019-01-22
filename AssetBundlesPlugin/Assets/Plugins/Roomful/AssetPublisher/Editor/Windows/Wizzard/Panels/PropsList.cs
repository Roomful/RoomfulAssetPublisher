using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace RF.AssetWizzard.Editor
{

    public class PropsList : AssetsList<PropTemplate>
    {

        public PropsList(EditorWindow window) : base(window) { }


        protected override void CreateNewAsset() {
            WindowManager.ShowCreateNewProp();
        }


        protected override List<PropTemplate> LocalySavedTemplates {
            get {
                return AssetBundlesSettings.Instance.LocalPropTemplates;
            }
        }

        protected override void DrawAssetInfo() {

            AssetInfoLable("Size", SelectedAsset.Size);
            AssetInfoLable("Placement", SelectedAsset.Placing);
            AssetInfoLable("Invoke", SelectedAsset.InvokeType);
            AssetInfoLable("Can Stack", SelectedAsset.CanStack);
            AssetInfoLable("Max Scale", SelectedAsset.MaxSize);
            AssetInfoLable("Min Scale", SelectedAsset.MinSize);

            //Types
            string types = string.Empty;
            foreach (ContentType t in SelectedAsset.ContentTypes) {
                types += t.ToString();

                if (SelectedAsset.ContentTypes.IndexOf(t) == (SelectedAsset.ContentTypes.Count - 1)) {
                    types += ";";
                } else {
                    types += ", ";
                }
            }

            if (types.Equals(string.Empty)) {
                types = "None;";
            }

            AssetInfoLable("Types", types);
        }


        protected override Network.Request.GetAssetsList CreateAssetsListRequests() {

            Network.Request.GetAllPropsList listRequest = new Network.Request.GetAllPropsList(LocalySavedTemplates.Count, 5);
            return listRequest;
        }

    }
}