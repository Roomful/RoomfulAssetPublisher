using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Rotorz.ReorderableList;

namespace RF.AssetWizzard.Editor
{


    public class PropWizzard : Panel
    {


        public PropWizzard(EditorWindow window) : base(window) { }

        public override void OnGUI() {

            if (AssetBundlesSettings.Instance.IsUploadInProgress) {
                DrawPreloaderAt(new Rect(570, 12, 20, 20));
                GUI.enabled = false;
            }

            GUILayout.Space(10f);

            if (CurrentProp != null) {
                PropWizard(GUI.enabled);
                return;
            }


            if (CurrentEnvironment == null) {
                EnvironmentWizard();
                return;
            }

            NoAssetWizard();



        }

        private PropAsset CurrentProp {
            get {
                return Object.FindObjectOfType<PropAsset>();
            }
        }

        private EnvironmentAsset CurrentEnvironment {
            get {
                return Object.FindObjectOfType<EnvironmentAsset>();
            }
        }



        private void NoAssetWizard() {
            GUILayout.Label("Create New Roomful Asset", EditorStyles.boldLabel);

            GUIContent createPropContent = new GUIContent();
            createPropContent.image = IconManager.GetIcon(Icon.model_icon);

            GUIContent environmentContent = new GUIContent();
            environmentContent.image = IconManager.GetIcon(Icon.environment_icon);

            GUIContent styleContent = new GUIContent();
            styleContent.image = IconManager.GetIcon(Icon.environment_icon);

            var options = new GUILayoutOption[2] { GUILayout.Width(150), GUILayout.Height(82) };

            GUILayout.BeginHorizontal();

            if (GUILayout.Button(environmentContent, options)) {
                WindowManager.ShowCreateNewEnvironment();
            }

            if (GUILayout.Button(styleContent, options)) {
                WindowManager.ShowCreateNewStyle();
            }

            if (GUILayout.Button(createPropContent, options)) {
                WindowManager.ShowCreateNewAsset();
            }

            GUILayout.EndHorizontal();

            return;
        }


        private void EnvironmentWizard() {

        }

        private void PropWizard(bool GUIState) {
            EditorGUI.BeginChangeCheck();
            {


                GUILayout.BeginHorizontal();

                GUILayout.BeginVertical(GUILayout.Width(370));
                {

                    GUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Title: ", GUILayout.Width(100));
                    GUI.enabled = false;
                    CurrentProp.Template.Title = EditorGUILayout.TextField(CurrentProp.Template.Title, GUILayout.Width(240));
                    GUI.enabled = GUIState;
                    GUILayout.EndHorizontal();

                    GUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Placing: ", GUILayout.Width(100));
                    CurrentProp.Template.Placing = (Placing)EditorGUILayout.EnumPopup(CurrentProp.Template.Placing, GUILayout.Width(240));
                    GUILayout.EndHorizontal();


                    GUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField("Invoke Type: ", GUILayout.Width(100));
                    CurrentProp.Template.InvokeType = (InvokeTypes)EditorGUILayout.EnumPopup(CurrentProp.Template.InvokeType, GUILayout.Width(240));
                    GUILayout.EndHorizontal();

                    if (CurrentProp.HasStandSurface) {
                        CurrentProp.Template.CanStack = false;
                        GUI.enabled = GUIState;
                    }

                    CurrentProp.Template.CanStack = YesNoFiled("CanStack", CurrentProp.Template.CanStack, 100, 240);
                    GUI.enabled = GUIState;

                }
                GUILayout.EndVertical();


                GUILayout.BeginVertical(GUILayout.Width(100));
                {
                    CurrentProp.Icon = (Texture2D)EditorGUILayout.ObjectField(CurrentProp.Icon, typeof(Texture2D), false, new GUILayoutOption[] { GUILayout.Width(70), GUILayout.Height(70) });


                    if (CurrentProp.Icon == null) {
                        DrawPreloaderAt(new Rect(525, 65, 32, 32));
                    }

                }
                GUILayout.EndVertical();
                GUILayout.EndHorizontal();



                GUIStyle alignment_center = new GUIStyle(EditorStyles.label);
                alignment_center.alignment = TextAnchor.MiddleCenter;

                GUIStyle alignment_right = new GUIStyle(EditorStyles.label);
                alignment_right.alignment = TextAnchor.MiddleRight;


                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Allowed Scale: ", GUILayout.Width(100));

                float minLimit = AssetBundlesSettings.MIN_ALLOWED_SIZE;
                float maxLimit = AssetBundlesSettings.MAX_AlLOWED_SIZE;

                EditorGUILayout.MinMaxSlider(ref CurrentProp.Template.MinSize, ref CurrentProp.Template.MaxSize, minLimit, maxLimit, GUILayout.Width(240));  //    EditorGUILayout.MinMaxSlider (CurrentProp.Template.MinScale, GUILayout.Width (240));

                if (CurrentProp.Template.MaxSize < CurrentProp.MaxAxisValue) {
                    CurrentProp.Template.MaxSize = CurrentProp.MaxAxisValue;
                }

                EditorGUILayout.LabelField(Mathf.CeilToInt(CurrentProp.Template.MinSize * 100f) + "mm / " + Mathf.CeilToInt(CurrentProp.Template.MaxSize * 100f) + "mm", alignment_right, GUILayout.Width(99));
                GUILayout.EndHorizontal();



                GUILayout.BeginHorizontal();

                float labelSize = 146;

                Vector3 def = CurrentProp.Size * 100f;
                Vector3 min = def * CurrentProp.MinScale;
                Vector3 max = def * CurrentProp.MaxScale;


                EditorGUILayout.LabelField("Min(" + Mathf.CeilToInt(CurrentProp.MinScale * 100f) + "%): " + (int)min.x + "x" + (int)min.y + "x" + (int)min.z, GUILayout.Width(labelSize));
                EditorGUILayout.LabelField("Default: " + (int)def.x + "x" + (int)def.y + "x" + (int)def.z, alignment_center, GUILayout.Width(labelSize));
                GUILayout.Space(1);
                EditorGUILayout.LabelField("Max(" + Mathf.CeilToInt(CurrentProp.MaxScale * 100f) + "%):" + (int)max.x + "x" + (int)max.y + "x" + (int)max.z, alignment_right, GUILayout.Width(labelSize));
                GUILayout.EndHorizontal();
                GUILayout.Space(10f);

                GUILayout.BeginHorizontal();
                GUILayout.BeginVertical(GUILayout.Width(225));
                {

                    ReorderableListGUI.Title("Asset Tags");
                    ReorderableListGUI.ListField(CurrentProp.Template.Tags, TagListItem, DrawEmptyTag);
                }
                GUILayout.EndVertical();

                GUILayout.BeginVertical(GUILayout.Width(225));
                {

                    ReorderableListGUI.Title("Supported Content Types");
                    List<string> ContentTypes = new List<string>();

                    foreach (ContentType t in CurrentProp.Template.ContentTypes) {
                        ContentTypes.Add(t.ToString());
                    }

                    ReorderableListGUI.ListField(ContentTypes, ContentTypeListItem, DrawEmptyContentType);

                    CurrentProp.Template.ContentTypes = new List<ContentType>();

                    foreach (string val in ContentTypes) {
                        ContentType parsed = SA.Common.Util.General.ParseEnum<ContentType>(val);
                        CurrentProp.Template.ContentTypes.Add(parsed);
                    }

                }
                GUILayout.EndVertical();

                GUILayout.EndHorizontal();

            }
            if (EditorGUI.EndChangeCheck()) {
                //AssetBundlesManager.SavePrefab (CurrentProp);
            }

            GUILayout.BeginHorizontal();

            GUILayout.FlexibleSpace();

            Rect buttonRect1 = new Rect(460, 360, 120, 18);
            Rect buttonRect2 = new Rect(310, 360, 120, 18);

            Rect buttonRect3 = new Rect(460, 390, 120, 18);

            if (string.IsNullOrEmpty(CurrentProp.Template.Id)) {
                bool upload = GUI.Button(buttonRect1, "Upload");
                if (upload) {
                    AssetBundlesSettings.Instance.IsInAutoloading = false;

                    PropBundleManager.UploadAssets(CurrentProp);
                }

            } else {
                bool upload = GUI.Button(buttonRect1, "Re Upload");
                if (upload) {
                    AssetBundlesSettings.Instance.IsInAutoloading = false;

                    PropBundleManager.ReuploadAsset(CurrentProp);
                }

                bool refresh = GUI.Button(buttonRect2, "Refresh");
                if (refresh) {
                    PropBundleManager.DownloadAssetBundle(CurrentProp.Template);
                }
            }

            bool create = GUI.Button(buttonRect3, "Create New");
            if (create) {
                WindowManager.ShowCreateNewAsset();
            }


            GUILayout.Space(40f);
            GUILayout.EndHorizontal();


            GUI.enabled = true;
            Window.Repaint();
        }



        private string ContentTypeListItem(Rect position, string itemValue) {

            position.y += 2;
            if (string.IsNullOrEmpty(itemValue)) {
                itemValue = ContentType.Image.ToString();
            }

            ContentType t = SA.Common.Util.General.ParseEnum<ContentType>(itemValue);
            t = (ContentType)EditorGUI.EnumPopup(position, t);
            return t.ToString();
        }

        private void DrawEmptyContentType() {
            GUILayout.Label("Asset willn't support any content.", EditorStyles.miniLabel);
        }
       

        private string TagListItem(Rect position, string itemValue) {
            if (itemValue == null)
                itemValue = "new_tag";
            return EditorGUI.TextField(position, itemValue);
        }

        private void DrawEmptyTag() {
            GUILayout.Label("No items in list.", EditorStyles.miniLabel);
        }


    }
}
