using System.Collections.Generic;
using UnityEditor;

namespace net.roomful.assets.editor
{
    class PropsList : AssetsList<PropAssetTemplate>
    {
        protected override int SearchPreloaderOffset => -17;
        public PropsList(EditorWindow window) : base(window) { }

        protected override void CreateNewAsset() {
            WindowManager.ShowCreateNewProp();
        }

        protected override List<PropAssetTemplate> LocallySavedTemplates => AssetBundlesSettings.Instance.m_localPropTemplates;

        protected override void DrawAssetInfo() {
            AssetInfoLabel("Size", m_SelectedAsset.Size);
            AssetInfoLabel("Placement", m_SelectedAsset.Placing);
            AssetInfoLabel("Invoke", m_SelectedAsset.InvokeType);
            AssetInfoLabel("Can Stack", m_SelectedAsset.CanStack);
            AssetInfoLabel("Scale", $"{m_SelectedAsset.MinSize} / {m_SelectedAsset.MaxSize}"  );

            //Types
            var types = string.Empty;
            foreach (var t in m_SelectedAsset.ContentTypes) {
                types += t.ToString();

                if (m_SelectedAsset.ContentTypes.IndexOf(t) == (m_SelectedAsset.ContentTypes.Count - 1)) {
                    types += ";";
                }
                else {
                    types += ", ";
                }
            }

            if (types.Equals(string.Empty)) {
                types = "None;";
            }

            AssetInfoLabel("Types", types);
            AssetInfoLabel("Meta:", $"LogoCount: {m_SelectedAsset.AssetBundleMeta.LogoCount}, " +
                                           $"ThumbnailCount: {m_SelectedAsset.AssetBundleMeta.ThumbnailCount} ");
        }

        protected override GetAssetsList CreateAssetsListRequests() {
            GetPropsList listRequest = null;

            switch (m_SearchType) {
                case SeartchRequestType.ByName:
                    listRequest = new GetPropsList(LocallySavedTemplates.Count, 5, m_SearchPattern);
                    break;
                case SeartchRequestType.ByTag:
                    var separatedTags = new List<string>(m_SearchPattern.Split(','));
                    listRequest = new GetPropsList(LocallySavedTemplates.Count, 5, separatedTags);
                    break;
                case SeartchRequestType.ById:
                    listRequest = new GetPropsList(LocallySavedTemplates.Count, 5, string.Empty);
                    listRequest.SetId(m_SearchPattern);
                    break;
            }

            return listRequest;
        }
    }
}
