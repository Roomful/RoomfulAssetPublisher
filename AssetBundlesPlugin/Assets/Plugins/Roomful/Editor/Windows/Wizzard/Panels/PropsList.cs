using System.Collections.Generic;
using UnityEditor;
using UnityEngine;


namespace RF.AssetWizzard.Editor
{

    public class PropsList : Panel
    {


        private Vector2 m_KeyScrollPos;
        private int m_itemsPreloaderAgnle = 0;
        private PropTemplate SelectedAsset = null;
        private const string SEARTCH_BAR_CONTROL_NAME = "seartchBat";

        public PropsList(EditorWindow window):base(window) {}


        public override void OnGUI() {

            if (!AssetBundlesSettings.Instance.LocalAssetTemplates.Contains(SelectedAsset)) {
                SelectedAsset = null;
            }

            if (SelectedAsset == null) {
                if(AssetBundlesSettings.Instance.LocalAssetTemplates.Count > 0) {
                    SelectedAsset = AssetBundlesSettings.Instance.LocalAssetTemplates [0];
                }
            }

            if(RequestManager.ASSETS_SEARTCH_IN_PROGRESS) {
                DrawPreloaderAt(new Rect(570, 12, 20, 20));
                GUI.enabled = false;
            }
            
            GUILayout.BeginHorizontal(WizardWindow.Constants.settingsBoxTitle); {
                
                GUIStyle s = new GUIStyle (EditorStyles.boldLabel);
                s.margin = new RectOffset (0, 0, 0, 0);
                s.padding = new RectOffset (2, 2, 2, 2);

                GUILayout.Label("Your Assets List", s, new GUILayoutOption[] {GUILayout.Width(130)});
                AssetBundlesSettings.Instance.SeartchType = (SeartchRequestType) EditorGUILayout.EnumPopup(AssetBundlesSettings.Instance.SeartchType, GUILayout.Width (55));


                GUI.SetNextControlName(SEARTCH_BAR_CONTROL_NAME);
                AssetBundlesSettings.Instance.SeartchPattern = EditorGUILayout.TextField(AssetBundlesSettings.Instance.SeartchPattern, WizardWindow.Constants.toolbarSeachTextFieldStyle, GUILayout.MinWidth(150));

                if (GUILayout.Button("", WizardWindow.Constants.toolbarSeachCancelButtonStyle)) {
                    AssetBundlesSettings.Instance.SeartchPattern = string.Empty;
                    GUI.FocusControl(null);
                }

                Texture2D refreshIcon = IconManager.GetIcon(Icon.refresh_black);
                bool refresh = GUILayout.Button (refreshIcon, WizardWindow.Constants.settingsBoxTitle, new GUILayoutOption[] {GUILayout.Width(20), GUILayout.Height(20)});
                if (refresh) {
                    AssetBundlesSettings.Instance.LocalAssetTemplates.Clear ();
                    RequestManager.SeartchAssets ();
                }

                bool addnew = GUILayout.Button ("+", WizardWindow.Constants.settingsBoxTitle, GUILayout.Width (20));
                if(addnew) {
                    WindowManager.ShowCreateNewAsset ();
                }
                    


                GUILayout.Space (7);
            } GUILayout.EndHorizontal();

            GUILayout.Space (1);



            int ASSETS_LIST_WIDTH = 200;
            int ASSETS_INFO_WIDTH = 268;

            int SCROLL_BAR_HEIGHT = 350;

            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical( GUILayout.Width(ASSETS_LIST_WIDTH));
        
            GUI.Box (new Rect (130, 58, ASSETS_LIST_WIDTH, SCROLL_BAR_HEIGHT), "", WizardWindow.Constants.settingsBox);

            m_KeyScrollPos = GUILayout.BeginScrollView(m_KeyScrollPos, GUIStyle.none,  GUI.skin.verticalScrollbar, new GUILayoutOption[] {GUILayout.Width(ASSETS_LIST_WIDTH), GUILayout.Height(SCROLL_BAR_HEIGHT)});


            m_itemsPreloaderAgnle+= 3;
            if (m_itemsPreloaderAgnle > 360) {
                m_itemsPreloaderAgnle = 0;
            }

            foreach (var asset in AssetBundlesSettings.Instance.LocalAssetTemplates) {

                GUIContent assetDisaplyContent = asset.DisaplyContent;

                if (assetDisaplyContent.image == null) {
                    Texture2D preloader = IconManager.Rotate(IconManager.GetIcon(Icon.loader), m_itemsPreloaderAgnle);
                    assetDisaplyContent.image = preloader;
                }

                if (GUILayout.Toggle(SelectedAsset == asset, assetDisaplyContent, WizardWindow.Constants.keysElement, new GUILayoutOption[] {GUILayout.Width(ASSETS_LIST_WIDTH)})) {
                    SelectedAsset = asset;
                }

            }

            if (AssetBundlesSettings.Instance.LocalAssetTemplates.Count > 0) {
                EditorGUILayout.Space ();

                if(GUILayout.Button ("Load more", EditorStyles.miniButton, GUILayout.Width(65))) {
                    RequestManager.SeartchAssets ();
                }
            }

            GUILayout.EndScrollView();
            GUILayout.EndVertical();


        



            GUILayout.BeginVertical(GUILayout.Width(ASSETS_INFO_WIDTH));



            if(SelectedAsset != null) {

                GUILayout.BeginHorizontal ();


                GUILayout.Label("Selected Asset", WizardWindow.Constants.settingsBoxTitle, new GUILayoutOption[] {GUILayout.Width(ASSETS_INFO_WIDTH - 20*2)});


                Texture2D edit = Resources.Load ("edit") as Texture2D;
                bool editAsset = GUILayout.Button (edit, WizardWindow.Constants.settingsBoxTitle, new GUILayoutOption[] {GUILayout.Width(20), GUILayout.Height(20)});
                if(editAsset) {
                    PropBundleManager.DownloadAssetBundle (SelectedAsset);
                }



                Texture2D trash = Resources.Load ("trash") as Texture2D;
                bool removeAsset = GUILayout.Button (trash, WizardWindow.Constants.settingsBoxTitle, new GUILayoutOption[] {GUILayout.Width(20), GUILayout.Height(20)});
                if(removeAsset) {
                    if (EditorUtility.DisplayDialog ("Delete " + SelectedAsset.Title, "Are you sure you want to remove this asset?", "Remove", "Cancel")) {;
                        RequestManager.RemoveAsset (SelectedAsset);
                    }
                }


                GUILayout.EndHorizontal ();

                EditorGUILayout.Space ();

                AssetInfoLable ("Id", SelectedAsset.Id);
                AssetInfoLable ("Title", SelectedAsset.Title);
                AssetInfoLable ("Size", SelectedAsset.Size);
                AssetInfoLable ("Placing", SelectedAsset.Placing);
                AssetInfoLable ("Invoke", SelectedAsset.InvokeType);
                AssetInfoLable ("Can Stack", SelectedAsset.CanStack);
                AssetInfoLable ("Max Scale", SelectedAsset.MaxSize);
                AssetInfoLable ("Min Scale", SelectedAsset.MinSize);


                string Plaforms = string.Empty;
                foreach(AssetUrl p in SelectedAsset.Urls) {
                    Plaforms += p.Platform + "  ";
                }
                AssetInfoLable ("Plaforms", Plaforms);


                //Types
                string types = string.Empty;
                foreach(ContentType t in SelectedAsset.ContentTypes) {
                    types += t.ToString ();

                    if(SelectedAsset.ContentTypes.IndexOf(t) == (SelectedAsset.ContentTypes.Count -1)) {
                        types+= ";";
                    } else {
                        types+= ", ";
                    }
                }

                if(types.Equals(string.Empty)) {
                    types = "None;";
                }

                AssetInfoLable ("Types", types);


                //Tags
                int countBeforeBreake = 0;
                int line = 0;
            
                List<string> tags = new List<string>();
                tags.Add (string.Empty);

                foreach(string tag in SelectedAsset.Tags) {

                    if(countBeforeBreake == 3) {
                        countBeforeBreake = 0;
                        line++;
                        tags.Add (string.Empty);
                    }

                    tags[line] += tag;

                    if(SelectedAsset.Tags.IndexOf(tag) == (SelectedAsset.Tags.Count -1)) {
                        tags[line]+= ";";
                    } else {
                        tags[line]+= ", ";
                    }

                    countBeforeBreake++;
                        
                }

                for(int i = 0; i < tags.Count; i++) {
                    if(i == 0) {
                        AssetInfoLable ("Tags", tags[i]);
                    } else {
                        AssetInfoLable (string.Empty, tags[i]);
                    }
                }



                AssetInfoLable ("Created", SelectedAsset.Created.ToString());
                AssetInfoLable ("Updated", SelectedAsset.Updated.ToString());
            

                EditorGUILayout.Space ();



            }


            GUILayout.EndVertical();

            GUILayout.Space(10f);
            GUILayout.EndHorizontal();


            Texture2D roomful_logo = Resources.Load ("roomful_logo") as Texture2D;
            GUI.DrawTexture (new Rect (380, 358, roomful_logo.width, roomful_logo.height), roomful_logo);



            GUI.enabled = true;


            Window.Repaint();

        }
            

        private void AssetInfoLable(string title, object msg) {
            GUILayout.BeginHorizontal();

            if(!string.IsNullOrEmpty(title)) {
                title += ": ";
            }

            EditorGUILayout.LabelField (title,  EditorStyles.boldLabel, new GUILayoutOption[] {GUILayout.Height(16), GUILayout.Width(65)});
            EditorGUILayout.SelectableLabel (msg.ToString(), EditorStyles.label, new GUILayoutOption[] {GUILayout.Height(16)});
            GUILayout.EndHorizontal ();
        }
            
    }
}