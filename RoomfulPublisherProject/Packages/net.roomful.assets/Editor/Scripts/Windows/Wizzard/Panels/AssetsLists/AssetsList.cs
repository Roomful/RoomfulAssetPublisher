using System;
using System.Collections.Generic;
using net.roomful.api;
using UnityEditor;
using UnityEngine;
using StansAssets.Foundation;
using StansAssets.Plugins.Editor;
using Json = StansAssets.Foundation.Json;

namespace net.roomful.assets.editor
{
    abstract class AssetsList<T> : Panel where T : AssetTemplate
    {
        protected T m_SelectedAsset;
        protected int m_NetworkSelectedIndex;
        protected SeartchRequestType m_SearchType = SeartchRequestType.ByName;
        protected string m_SearchPattern = string.Empty;
        protected virtual bool AllowDownloadSelectedAsset => true;

        Vector2 m_KeyScrollPos;
        int m_ItemsPreloaderAngle;
        const string k_SearchBarControlName = "seartchBat";
        public bool AssetsSearchInProgress;

        protected AssetsList(EditorWindow window) : base(window) { }
        protected abstract void DrawAssetInfo();
        protected abstract GetAssetsList CreateAssetsListRequests();
        protected abstract List<T> LocallySavedTemplates { get; }

        protected abstract void CreateNewAsset();
        protected virtual int SearchPreloaderOffset => 0;

        public override void OnGUI() {
            if (!LocallySavedTemplates.Contains(m_SelectedAsset)) {
                m_SelectedAsset = null;
            }

            if (m_SelectedAsset == null && LocallySavedTemplates.Count > 0) {
                m_SelectedAsset = LocallySavedTemplates[0];
            }

            if (AssetsSearchInProgress) {
                DrawPreloaderAt(new Rect(280 + SearchPreloaderOffset, 12, 20, 20));
                GUI.enabled = false;
            }

            GUILayout.BeginHorizontal(WizardWindow.Constants.settingsBoxTitle);
            {
                var s = new GUIStyle(EditorStyles.boldLabel);
                s.margin = new RectOffset(0, 0, 0, 0);
                s.padding = new RectOffset(2, 2, 2, 2);

                GUILayout.Label("Network:", s, GUILayout.Width(75));

                var netWorkOptions = new List<string>();
                netWorkOptions.Add("Any");
                netWorkOptions.AddRange(RoomfulPublisher.Session.Networks.Names);
                m_NetworkSelectedIndex = EditorGUILayout.Popup(m_NetworkSelectedIndex, netWorkOptions.ToArray(), GUILayout.Width(130));
                GUILayout.Space(5);
                
                m_SearchType = (SeartchRequestType) EditorGUILayout.EnumPopup(m_SearchType, GUILayout.Width(75));
                GUILayout.Space(5);

                GUI.SetNextControlName(k_SearchBarControlName);
                m_SearchPattern = EditorGUILayout.TextField(m_SearchPattern, WizardWindow.Constants.toolbarSearchTextFieldStyle, GUILayout.MinWidth(125));

                if (GUILayout.Button("", WizardWindow.Constants.toolbarSearchCancelButtonStyle)) {
                    m_SearchPattern = string.Empty;
                    GUI.FocusControl(null);
                }

                var refreshIcon = IconManager.GetIcon(Icon.refresh_black);
                var refresh = GUILayout.Button(refreshIcon, WizardWindow.Constants.settingsBoxTitle, GUILayout.Width(20), GUILayout.Height(20));
                if (refresh) {
                    LocallySavedTemplates.Clear();
                    SearchAssets();
                }

                var addnew = GUILayout.Button("+", WizardWindow.Constants.settingsBoxTitle, GUILayout.Width(20));
                if (addnew) {
                    CreateNewAsset();
                }

                GUILayout.Space(7);
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(5);

            const int assetsListWidth = 215;
            GUILayout.BeginHorizontal();
            GUILayout.Space(1);
            GUILayout.BeginVertical(GUILayout.Width(assetsListWidth));

            if (EditorWindow.focusedWindow != null && EditorWindow.focusedWindow.ToString().Contains("Asset Publisher"))
                m_KeyScrollPos = GUILayout.BeginScrollView(m_KeyScrollPos, GUIStyle.none, GUI.skin.verticalScrollbar, GUILayout.Width(assetsListWidth), GUILayout.MaxHeight(EditorWindow.GetWindow<WizardWindow>(false, "Roomful - Asset Publisher").position.height), GUILayout.ExpandHeight(true));
            else
                m_KeyScrollPos = GUILayout.BeginScrollView(m_KeyScrollPos, GUIStyle.none, GUI.skin.verticalScrollbar, GUILayout.Width(assetsListWidth), GUILayout.ExpandHeight(true));

            m_ItemsPreloaderAngle += 3;
            if (m_ItemsPreloaderAngle > 360) {
                m_ItemsPreloaderAngle = 0;
            }

            GUI.Box(new Rect(0, 0, assetsListWidth - 5, LocallySavedTemplates.Count * 16.25f + 26), "", WizardWindow.Constants.settingsBox);
            Texture assetImage = new Texture2D(0, 0);
            foreach (var asset in LocallySavedTemplates) {
                var assetDisplayContent = asset.DisplayContent;

                if (assetDisplayContent.image == null) {
                    var preloader = Texture2DUtility.Rotate(IconManager.GetIcon(Icon.loader), m_ItemsPreloaderAngle);
                    assetDisplayContent.image = preloader;
                }

                if (GUILayout.Toggle(m_SelectedAsset == asset, assetDisplayContent, WizardWindow.Constants.keysElement, GUILayout.Width(assetsListWidth - 5))) {
                    m_SelectedAsset = asset;
                }

                if (Event.current.type == EventType.Repaint &&
                    GUILayoutUtility.GetLastRect().Contains(Event.current.mousePosition)) {
                    assetImage = assetDisplayContent.image;
                }
            }

            if (LocallySavedTemplates.Count > 0) {
                EditorGUILayout.Space(5);

                if (GUILayout.Button("Load more", EditorStyles.miniButton, GUILayout.Width(75))) {
                    SearchAssets();
                }
            }

            GUILayout.EndScrollView();
            GUILayout.EndVertical();

            GUILayout.Space(5);
            GUILayout.BeginVertical(GUILayout.ExpandWidth(true));
            var roomfulLogo = Resources.Load("roomful_logo") as Texture2D;

            if (m_SelectedAsset != null) {
                using (new IMGUIBeginHorizontal(GUILayout.ExpandWidth(true))) {
                    GUILayout.Label("Actions", WizardWindow.Constants.settingsBoxTitle);
                    var download = Resources.Load("download") as Texture2D;

                    var downloadContent = new GUIContent
                    {
                        text = string.Empty,
                        tooltip = "Download",
                        image = download,
                    };

                    if(AllowDownloadSelectedAsset)
                    {
                        var editAsset = GUILayout.Button(downloadContent, WizardWindow.Constants.settingsBoxTitle, GUILayout.Width(20), GUILayout.Height(20));
                        if (editAsset) {
                            BundleService.Download(m_SelectedAsset);
                        }
                    }
                   

                    var trash = Resources.Load("trash") as Texture2D;
                    
                    var deleteContent = new GUIContent
                    {
                        text = string.Empty,
                        tooltip = "Delete",
                        image = trash,
                    };
                    
                    var removeAsset = GUILayout.Button(deleteContent, WizardWindow.Constants.settingsBoxTitle, GUILayout.Width(20), GUILayout.Height(20));
                    if (removeAsset) {
                        if (EditorUtility.DisplayDialog("Delete " + m_SelectedAsset.Title, "Are you sure you want to remove this asset?", "Remove", "Cancel")) {
                            RequestManager.RemoveAsset(m_SelectedAsset);
                        }
                    }

                }

                using (new IMGUIBeginHorizontal(GUILayout.ExpandWidth(true))) {
                    GUILayout.Label("Selected Asset", WizardWindow.Constants.settingsBoxTitle);
                    var openAsset = GUILayout.Button("Edit Meta", WizardWindow.Constants.settingsBoxTitle, GUILayout.Width(68), GUILayout.Height(20));
                    if (openAsset) {
                        BundleService.Create(m_SelectedAsset);
                    }
                }

                EditorGUILayout.Space();

                AssetInfoLabel("Id", m_SelectedAsset.Id);
                AssetInfoLabel("Title", m_SelectedAsset.Title);
                AssetInfoLabel("Status", m_SelectedAsset.Status);

                var network = RoomfulPublisher.Session.Networks.GetNetwork(m_SelectedAsset.NetworkId)?.Name;
                AssetInfoLabel("Network", network);

                DrawAssetInfo();

                var platforms = string.Empty;
                foreach (var p in m_SelectedAsset.Urls) {
                    platforms += p.Platform + "  ";
                }

                AssetInfoLabel("Platforms", platforms);

                //Tags
                var countBeforeBreake = 0;
                var line = 0;

                var tags = new List<string>();
                tags.Add(string.Empty);

                foreach (var tag in m_SelectedAsset.Tags) {
                    if (countBeforeBreake == 3) {
                        countBeforeBreake = 0;
                        line++;
                        tags.Add(string.Empty);
                    }

                    tags[line] += tag;

                    if (m_SelectedAsset.Tags.IndexOf(tag) == (m_SelectedAsset.Tags.Count - 1)) {
                        tags[line] += ";";
                    }
                    else {
                        tags[line] += ", ";
                    }

                    countBeforeBreake++;
                }

                for (var i = 0; i < tags.Count; i++) {
                    if (i == 0) {
                        AssetInfoLabel("Tags", tags[i]);
                    }
                    else {
                        AssetInfoLabel(string.Empty, tags[i]);
                    }
                }

                AssetInfoLabel("Created", m_SelectedAsset.Created.ToString("dd MMM yyyy HH:mm"));
                AssetInfoLabel("Updated", m_SelectedAsset.Updated.ToString("dd MMM yyyy HH:mm"));

                EditorGUILayout.Space();
                GUILayout.Box(roomfulLogo, GUIStyle.none);
            }

            GUILayout.EndVertical();

            GUILayout.Space(10f);
            GUILayout.EndHorizontal();

            if (assetImage.width > 100)
                GUI.Box(new Rect(Event.current.mousePosition.x + 10, Event.current.mousePosition.y + 10, assetImage.width, assetImage.height), assetImage, WizardWindow.Constants.settingsBox);
            GUI.enabled = true;
            Window.Repaint();
        }

        protected void AssetInfoLabel(string title, object msg) {
            GUILayout.BeginHorizontal();

            if (!string.IsNullOrEmpty(title)) {
                title += ": ";
            }

            EditorGUILayout.LabelField(title, EditorStyles.boldLabel, GUILayout.Height(16), GUILayout.Width(65));
            EditorGUILayout.SelectableLabel(msg.ToString(), EditorStyles.label, GUILayout.Height(16));
            GUILayout.EndHorizontal();
        }

        void SearchAssets() {
            AssetsSearchInProgress = true;

            var listRequest = CreateAssetsListRequests();

            listRequest.PackageCallbackText = allAssetsCallback => {
                var allAssetsList = Json.Deserialize(allAssetsCallback) as List<object>;

                foreach (var assetData in allAssetsList) {
                    var rawData = new JSONData(assetData).RawData;
                    // PropTemplate at = new PropTemplate(new JSONData(assetData).RawData);
                    var tpl = (T) Activator.CreateInstance(typeof(T), rawData);
                    LocallySavedTemplates.Add(tpl);
                }

                AssetBundlesSettings.Save();
                AssetsSearchInProgress = false;
            };

            if (m_NetworkSelectedIndex != 0)
            {
                var index = m_NetworkSelectedIndex - 1;
                if (RoomfulPublisher.Session.Networks.Models.Count > index)
                {
                    var network = RoomfulPublisher.Session.Networks.Models[index];
                    listRequest.SetNetwork(network.Id);
                }
            }

            listRequest.Send();
        }
    }
}