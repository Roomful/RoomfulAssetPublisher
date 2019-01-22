using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using Rotorz.ReorderableList;


namespace RF.AssetWizzard.Editor
{
    public class PropWizzard : AssetWizzard<PropAsset>
    {

        public override void Create() {
            CreateProp();
        }

        public override void Download() {
            DownloadProp(Asset.Template);
        }

        public override void Upload() {
            UploadProp(Asset);
        }


        public static void CreateProp() {
            WindowManager.ShowCreateNewProp();
        }
        public static void DownloadProp(PropTemplate tpl) {
            BundleService.Download<PropTemplate>(tpl);
        }

        public static void UploadProp(PropAsset asset) {
            BundleService.Upload<PropAsset>(asset);
        }

        public static void UpdateMeta(PropAsset asset) {
            BundleService.UpdateMeta<PropAsset>(asset);
        }




        public override void OnGUI(bool GUIState) {

            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical(GUILayout.Width(370));
            {

                DrawTitleFiled(GUIState);

                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Placement: ", GUILayout.Width(100));
                Asset.Template.Placing = (Placing)EditorGUILayout.EnumPopup(Asset.Template.Placing, GUILayout.Width(240));
                GUILayout.EndHorizontal();


                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Invoke Type: ", GUILayout.Width(100));
                Asset.Template.InvokeType = (InvokeTypes)EditorGUILayout.EnumPopup(Asset.Template.InvokeType, GUILayout.Width(240));
                GUILayout.EndHorizontal();

                if (Asset.HasStandSurface) {
                    Asset.Template.CanStack = false;
                    GUI.enabled = GUIState;
                }

                Asset.Template.CanStack = YesNoFiled("Can Stack", Asset.Template.CanStack, 100, 240);
                Asset.Template.PedestalInZoomView = YesNoFiled("Pedestal In Zoom", Asset.Template.PedestalInZoomView, 100, 240);
                GUI.enabled = GUIState;

            }

            GUILayout.EndVertical();
            GUILayout.BeginVertical(GUILayout.Width(100));
            {
                Asset.Icon = (Texture2D)EditorGUILayout.ObjectField(Asset.Icon, typeof(Texture2D), false, new GUILayoutOption[] { GUILayout.Width(70), GUILayout.Height(70) });

                if (Asset.Icon == null) {
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

            float minSize = Asset.Template.MinSize;
            float maxSize = Asset.Template.MaxSize;
            EditorGUILayout.MinMaxSlider(ref minSize, ref maxSize, PropTemplate.MIN_ALLOWED_AXIS_SIZE, PropTemplate.MAX_ALLOWED_AXIS_SIZE, GUILayout.Width(240));  //    EditorGUILayout.MinMaxSlider (CurrentProp.Template.MinScale, GUILayout.Width (240));


            Asset.Template.MinSize = minSize;
            Asset.Template.MaxSize = maxSize;

            if (Asset.MinScale >= 1f) {
                Asset.Template.MinSize = Asset.MaxAxisValue;
            }

            if (Asset.Template.MaxSize < Asset.MaxAxisValue) {
                Asset.Template.MaxSize = Asset.MaxAxisValue;
            }



            EditorGUILayout.LabelField(Mathf.CeilToInt(Asset.Template.MinSize * 100f) + "mm / " + Mathf.CeilToInt(Asset.Template.MaxSize * 100f) + "mm", alignment_right, GUILayout.Width(99));
            GUILayout.EndHorizontal();



            GUILayout.BeginHorizontal();

            float labelSize = 146;

            Vector3 def = Asset.Size * 100f;
            Vector3 min = def * Asset.MinScale;
            Vector3 max = def * Asset.MaxScale;


            EditorGUILayout.LabelField("Min(" + Mathf.CeilToInt(Asset.MinScale * 100f) + "%): " + (int)min.x + "x" + (int)min.y + "x" + (int)min.z, GUILayout.Width(labelSize));
            EditorGUILayout.LabelField("Default: " + (int)def.x + "x" + (int)def.y + "x" + (int)def.z, alignment_center, GUILayout.Width(labelSize));
            GUILayout.Space(1);
            EditorGUILayout.LabelField("Max(" + Mathf.CeilToInt(Asset.MaxScale * 100f) + "%):" + (int)max.x + "x" + (int)max.y + "x" + (int)max.z, alignment_right, GUILayout.Width(labelSize));
            GUILayout.EndHorizontal();
            GUILayout.Space(10f);

            GUILayout.BeginHorizontal();
            try {
                DrawTags();
            }
            catch (Exception e) {
                Console.WriteLine(e); 
            }

            GUILayout.BeginVertical(GUILayout.Width(225));
            {
                ReorderableListGUI.Title("Supported Content Types");
                List<string> ContentTypes = new List<string>();

                foreach (ContentType t in Asset.Template.ContentTypes) {
                    ContentTypes.Add(t.ToString());
                }

                try {
                    ReorderableListGUI.ListField(ContentTypes, ContentTypeListItem, DrawEmptyContentType);
                }
                catch (Exception e) {
                    Console.WriteLine(e);
                }
                
                Asset.Template.ContentTypes = new List<ContentType>();

                foreach (string val in ContentTypes) {
                    ContentType parsed = SA.Common.Util.General.ParseEnum<ContentType>(val);
                    Asset.Template.ContentTypes.Add(parsed);
                }
            }

            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            DrawControlButtons();


            GUI.enabled = true;
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
            GUILayout.Label("Asset won't support content.", EditorStyles.miniLabel);
        }



    }
}