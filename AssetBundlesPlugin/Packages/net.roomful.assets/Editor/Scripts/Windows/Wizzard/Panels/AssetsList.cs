using System;
using System.Collections.Generic;
using net.roomful.api;
using UnityEditor;
using UnityEngine;
using StansAssets.Foundation;
using Json = StansAssets.Foundation.Json;

namespace net.roomful.assets.Editor
{
    internal abstract class AssetsList<T> : Panel where T : AssetTemplate
    {
        protected T SelectedAsset = null;
        protected SeartchRequestType SeartchType = SeartchRequestType.ByName;
        protected string SeartchPattern = string.Empty;

        private Vector2 m_KeyScrollPos;
        private int m_itemsPreloaderAgnle = 0;
        private const string SEARTCH_BAR_CONTROL_NAME = "seartchBat";
        public bool m_assetsSeartchInProgress = false;

        public AssetsList(EditorWindow window) : base(window) { }
        protected abstract void DrawAssetInfo();
        protected abstract Network.Request.GetAssetsList CreateAssetsListRequests();
        protected abstract List<T> LocalySavedTemplates { get; }

        protected abstract void CreateNewAsset();

        public override void OnGUI() {
            if (!LocalySavedTemplates.Contains(SelectedAsset)) {
                SelectedAsset = null;
            }

            if (SelectedAsset == null && LocalySavedTemplates.Count > 0) {
                SelectedAsset = LocalySavedTemplates[0];
            }

            if (m_assetsSeartchInProgress) {
                DrawPreloaderAt(new Rect(570, 12, 20, 20));
                GUI.enabled = false;
            }

            GUILayout.BeginHorizontal(WizardWindow.Constants.settingsBoxTitle);
            {
                var s = new GUIStyle(EditorStyles.boldLabel);
                s.margin = new RectOffset(0, 0, 0, 0);
                s.padding = new RectOffset(2, 2, 2, 2);

                GUILayout.Label("Your Asset List", s, GUILayout.Width(130));
                SeartchType = (SeartchRequestType) EditorGUILayout.EnumPopup(SeartchType, GUILayout.Width(67));

                GUI.SetNextControlName(SEARTCH_BAR_CONTROL_NAME);
                SeartchPattern = EditorGUILayout.TextField(SeartchPattern, WizardWindow.Constants.toolbarSeachTextFieldStyle, GUILayout.MinWidth(150));

                if (GUILayout.Button("", WizardWindow.Constants.toolbarSeachCancelButtonStyle)) {
                    SeartchPattern = string.Empty;
                    GUI.FocusControl(null);
                }

                var refreshIcon = IconManager.GetIcon(Icon.refresh_black);
                var refresh = GUILayout.Button(refreshIcon, WizardWindow.Constants.settingsBoxTitle, GUILayout.Width(20), GUILayout.Height(20));
                if (refresh) {
                    LocalySavedTemplates.Clear();
                    SeartchAssets();
                }

                var addnew = GUILayout.Button("+", WizardWindow.Constants.settingsBoxTitle, GUILayout.Width(20));
                if (addnew) {
                    CreateNewAsset();
                }

                GUILayout.Space(7);
            }
            GUILayout.EndHorizontal();

            GUILayout.Space(1);

            var ASSETS_LIST_WIDTH = 200;
            var ASSETS_INFO_WIDTH = 268;

            var SCROLL_BAR_HEIGHT = 350;

            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical(GUILayout.Width(ASSETS_LIST_WIDTH));

            GUI.Box(new Rect(130, 58, ASSETS_LIST_WIDTH, SCROLL_BAR_HEIGHT), "", WizardWindow.Constants.settingsBox);

            m_KeyScrollPos = GUILayout.BeginScrollView(m_KeyScrollPos, GUIStyle.none, GUI.skin.verticalScrollbar, GUILayout.Width(ASSETS_LIST_WIDTH), GUILayout.Height(SCROLL_BAR_HEIGHT));

            m_itemsPreloaderAgnle += 3;
            if (m_itemsPreloaderAgnle > 360) {
                m_itemsPreloaderAgnle = 0;
            }

            foreach (var asset in LocalySavedTemplates) {
                var assetDisaplyContent = asset.DisplayContent;

                if (assetDisaplyContent.image == null) {
                    var preloader = Texture2DUtility.Rotate(IconManager.GetIcon(Icon.loader), m_itemsPreloaderAgnle);
                    assetDisaplyContent.image = preloader;
                }

                if (GUILayout.Toggle(SelectedAsset == asset, assetDisaplyContent, WizardWindow.Constants.keysElement, GUILayout.Width(ASSETS_LIST_WIDTH))) {
                    SelectedAsset = asset;
                }
            }

            if (LocalySavedTemplates.Count > 0) {
                EditorGUILayout.Space();

                if (GUILayout.Button("Load more", EditorStyles.miniButton, GUILayout.Width(65))) {
                    SeartchAssets();
                }
            }

            GUILayout.EndScrollView();
            GUILayout.EndVertical();

            GUILayout.BeginVertical(GUILayout.Width(ASSETS_INFO_WIDTH));

            if (SelectedAsset != null) {
                GUILayout.BeginHorizontal();

                GUILayout.Label("Selected Asset", WizardWindow.Constants.settingsBoxTitle, GUILayout.Width(ASSETS_INFO_WIDTH - 20 * 2));

                var edit = Resources.Load("edit") as Texture2D;
                var editAsset = GUILayout.Button(edit, WizardWindow.Constants.settingsBoxTitle, GUILayout.Width(20), GUILayout.Height(20));
                if (editAsset) {
                    BundleService.Download(SelectedAsset);
                }

                var trash = Resources.Load("trash") as Texture2D;
                var removeAsset = GUILayout.Button(trash, WizardWindow.Constants.settingsBoxTitle, GUILayout.Width(20), GUILayout.Height(20));
                if (removeAsset) {
                    if (EditorUtility.DisplayDialog("Delete " + SelectedAsset.Title, "Are you sure you want to remove this asset?", "Remove", "Cancel")) {
                        RequestManager.RemoveAsset(SelectedAsset);
                    }
                }

                GUILayout.EndHorizontal();

                EditorGUILayout.Space();

                AssetInfoLable("Id", SelectedAsset.Id);
                AssetInfoLable("Title", SelectedAsset.Title);

                DrawAssetInfo();

                var Plaforms = string.Empty;
                foreach (var p in SelectedAsset.Urls) {
                    Plaforms += p.Platform + "  ";
                }

                AssetInfoLable("Plaforms", Plaforms);

                //Tags
                var countBeforeBreake = 0;
                var line = 0;

                var tags = new List<string>();
                tags.Add(string.Empty);

                foreach (var tag in SelectedAsset.Tags) {
                    if (countBeforeBreake == 3) {
                        countBeforeBreake = 0;
                        line++;
                        tags.Add(string.Empty);
                    }

                    tags[line] += tag;

                    if (SelectedAsset.Tags.IndexOf(tag) == (SelectedAsset.Tags.Count - 1)) {
                        tags[line] += ";";
                    }
                    else {
                        tags[line] += ", ";
                    }

                    countBeforeBreake++;
                }

                for (var i = 0; i < tags.Count; i++) {
                    if (i == 0) {
                        AssetInfoLable("Tags", tags[i]);
                    }
                    else {
                        AssetInfoLable(string.Empty, tags[i]);
                    }
                }

                AssetInfoLable("Created", SelectedAsset.Created.ToString("dd MMM yyyy HH:mm"));
                AssetInfoLable("Updated", SelectedAsset.Updated.ToString("dd MMM yyyy HH:mm"));

                EditorGUILayout.Space();
            }

            GUILayout.EndVertical();

            GUILayout.Space(10f);
            GUILayout.EndHorizontal();

            var roomful_logo = Resources.Load("roomful_logo") as Texture2D;
            GUI.DrawTexture(new Rect(380, 358, roomful_logo.width, roomful_logo.height), roomful_logo);

            GUI.enabled = true;
            Window.Repaint();
        }

        protected void AssetInfoLable(string title, object msg) {
            GUILayout.BeginHorizontal();

            if (!string.IsNullOrEmpty(title)) {
                title += ": ";
            }

            EditorGUILayout.LabelField(title, EditorStyles.boldLabel, GUILayout.Height(16), GUILayout.Width(65));
            EditorGUILayout.SelectableLabel(msg.ToString(), EditorStyles.label, GUILayout.Height(16));
            GUILayout.EndHorizontal();
        }

        private void SeartchAssets() {
            m_assetsSeartchInProgress = true;

            var listRequest = CreateAssetsListRequests();

            listRequest.PackageCallbackText = allAssetsCallback => {
                var allAssetsList = Json.Deserialize(allAssetsCallback) as List<object>;

                foreach (var assetData in allAssetsList) {
                    var rawData = new JSONData(assetData).RawData;
                    // PropTemplate at = new PropTemplate(new JSONData(assetData).RawData);
                    var tpl = (T) Activator.CreateInstance(typeof(T), rawData);
                    LocalySavedTemplates.Add(tpl);
                }

                AssetBundlesSettings.Save();
                m_assetsSeartchInProgress = false;
            };

            listRequest.Send();
        }
    }
}