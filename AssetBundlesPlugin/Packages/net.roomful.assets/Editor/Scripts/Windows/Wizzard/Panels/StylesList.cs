using System.Collections.Generic;
using UnityEditor;

namespace net.roomful.assets.Editor
{

    public class StylesList : AssetsList<StyleTemplate>
    {

        public StylesList(EditorWindow window) : base(window) { }


        protected override void CreateNewAsset() {
            WindowManager.ShowCreateNewStyle();
        }


        protected override List<StyleTemplate> LocalySavedTemplates => AssetBundlesSettings.Instance.m_localStyleTemplates;

        protected override void DrawAssetInfo() {

          
        }


        protected override Network.Request.GetAssetsList CreateAssetsListRequests() {

            Network.Request.GetStylesList listRequest = null;


            switch(SeartchType) {
                case SeartchRequestType.ByName:
                    listRequest = new Network.Request.GetStylesList(LocalySavedTemplates.Count, 5, SeartchPattern);
                    break;
                case SeartchRequestType.ByTag:
                    var separatedTags = new List<string>(SeartchPattern.Split(','));
                    listRequest = new Network.Request.GetStylesList(LocalySavedTemplates.Count, 5, separatedTags);
                    break;
                case SeartchRequestType.ById:
                    listRequest = new Network.Request.GetStylesList(LocalySavedTemplates.Count, 5, string.Empty);
                    listRequest.SetId(SeartchPattern);
                    break;

            }

            return listRequest;
        }

    }
}