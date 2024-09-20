using System.Collections.Generic;
using System.Globalization;
using net.roomful.api;
using UnityEditor;
using UnityEngine;

namespace net.roomful.assets.editor
{
    class StylesList : AssetsList<StyleAssetTemplate>
    {
        protected override int SearchPreloaderOffset => -17;

        protected override bool AllowDownloadSelectedAsset => m_SelectedAsset.StyleType != StyleType.NonExtendable;

        public StylesList(EditorWindow window) : base(window) { }

        protected override void CreateNewAsset() {
            WindowManager.ShowCreateNewStyle();
        }

        protected override List<StyleAssetTemplate> LocallySavedTemplates => AssetBundlesSettings.Instance.m_localStyleTemplates;

        protected override void DrawAssetInfo() {
            AssetInfoLabel("Home",$"x: {GetPositionString(m_SelectedAsset.HomePosition.x)}, y: {GetPositionString(m_SelectedAsset.HomePosition.y)}, z: {GetPositionString(m_SelectedAsset.HomePosition.z)}" );
            AssetInfoLabel("Type", m_SelectedAsset.StyleType);
            AssetInfoLabel("Doors", m_SelectedAsset.DoorsType);
            AssetInfoLabel("Price",  $"${m_SelectedAsset.Price:0.00}");
            AssetInfoLabel("Score",  m_SelectedAsset.Score);
        }

        private string GetPositionString(float pos) {
            if (Mathf.Approximately(pos, 0f)) {
                return "-";
            }

            return pos.ToString(CultureInfo.InvariantCulture);
        }

        protected override GetAssetsList CreateAssetsListRequests() {
            GetStylesList listRequest = null;

            switch (m_SearchType) {
                case SeartchRequestType.ByName:
                    listRequest = new GetStylesList(LocallySavedTemplates.Count, 5, m_SearchPattern);
                    break;
                case SeartchRequestType.ByTag:
                    var separatedTags = new List<string>(m_SearchPattern.Split(','));
                    listRequest = new GetStylesList(LocallySavedTemplates.Count, 5, separatedTags);
                    break;
                case SeartchRequestType.ById:
                    listRequest = new GetStylesList(LocallySavedTemplates.Count, 5, string.Empty);
                    listRequest.SetId(m_SearchPattern);
                    break;
            }

            return listRequest;
        }
    }
}
