using System.Collections.Generic;
using UnityEditor;

namespace net.roomful.assets.Editor
{

    public class PropsList : AssetsList<PropTemplate>
    {

        public PropsList(EditorWindow window) : base(window) { }


        protected override void CreateNewAsset() {
            WindowManager.ShowCreateNewProp();
        }


        protected override List<PropTemplate> LocalySavedTemplates => AssetBundlesSettings.Instance.m_localPropTemplates;

        protected override void DrawAssetInfo() {

            AssetInfoLable("Size", SelectedAsset.Size);
            AssetInfoLable("Placement", SelectedAsset.Placing);
            AssetInfoLable("Invoke", SelectedAsset.InvokeType);
            AssetInfoLable("Can Stack", SelectedAsset.CanStack);
            AssetInfoLable("Alternative Zoom", SelectedAsset.AlternativeZoom);
            AssetInfoLable("Max Scale", SelectedAsset.MaxSize);
            AssetInfoLable("Min Scale", SelectedAsset.MinSize);

            //Types
            var types = string.Empty;
            foreach (var t in SelectedAsset.ContentTypes) {
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

            Network.Request.GetPropsList listRequest = null;


            switch(SeartchType) {
                case SeartchRequestType.ByName:
                    listRequest = new Network.Request.GetPropsList(LocalySavedTemplates.Count, 5, SeartchPattern);
                    break;
                case SeartchRequestType.ByTag:
                    var separatedTags = new List<string>(SeartchPattern.Split(','));
                    listRequest = new Network.Request.GetPropsList(LocalySavedTemplates.Count, 5, separatedTags);
                    break;
                case SeartchRequestType.ById:
                    listRequest = new Network.Request.GetPropsList(LocalySavedTemplates.Count, 5, string.Empty);
                    listRequest.SetId(SeartchPattern);
                    break;

            }

            return listRequest;
        }

    }
}