using System.Collections.Generic;
using UnityEditor;

namespace net.roomful.assets.editor
{
    class EnvironmentList : AssetsList<EnvironmentAssetTemplate>
    {
        protected override int SearchPreloaderOffset => 45;
        
        public EnvironmentList(EditorWindow window) : base(window) { }

        protected override void CreateNewAsset() {
            WindowManager.ShowCreateNewEnvironment();
        }

        protected override List<EnvironmentAssetTemplate> LocallySavedTemplates => AssetBundlesSettings.Instance.m_localEnvironmentsTemplates;

        protected override void DrawAssetInfo() { }

        protected override GetAssetsList CreateAssetsListRequests() {
            GetEnvironmentsList listRequest = null;

            switch (m_SearchType) {
                case SeartchRequestType.ByName:
                    listRequest = new GetEnvironmentsList(LocallySavedTemplates.Count, 5, m_SearchPattern);
                    break;
                case SeartchRequestType.ByTag:
                    var separatedTags = new List<string>(m_SearchPattern.Split(','));
                    listRequest = new GetEnvironmentsList(LocallySavedTemplates.Count, 5, separatedTags);
                    break;
                case SeartchRequestType.ById:
                    listRequest = new GetEnvironmentsList(LocallySavedTemplates.Count, 5, string.Empty);
                    listRequest.SetId(m_SearchPattern);
                    break;
            }

            return listRequest;
        }
    }
}