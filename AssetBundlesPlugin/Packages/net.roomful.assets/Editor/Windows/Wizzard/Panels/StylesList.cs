using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace net.roomful.assets.Editor
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

            Network.Request.GetStylesList listRequest = null;


            switch(SeartchType) {
                case SeartchRequestType.ByName:
                    listRequest = new Network.Request.GetStylesList(LocalySavedTemplates.Count, 5, SeartchPattern);
                    break;
                case SeartchRequestType.ByTag:
                    List<string> separatedTags = new List<string>(SeartchPattern.Split(','));
                    listRequest = new net.roomful.assets.Network.Request.GetStylesList(LocalySavedTemplates.Count, 5, separatedTags);
                    break;
                case SeartchRequestType.ById:
                    listRequest = new net.roomful.assets.Network.Request.GetStylesList(LocalySavedTemplates.Count, 5, string.Empty);
                    listRequest.SetId(SeartchPattern);
                    break;

            }

            return listRequest;
        }

    }
}