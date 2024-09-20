using System;
using System.Collections.Generic;
using net.roomful.api;
using UnityEngine;
using UnityEditor;
using Rotorz.ReorderableList;
using StansAssets.Foundation;
using StansAssets.Plugins.Editor;

namespace net.roomful.assets.editor
{
    class PropWizard : AssetWizard<PropAsset>
    {
        protected override void Create() {
            CreateProp();
        }

        protected override void Download() {
            DownloadProp(Asset.Template);
        }

        protected override void UpdateMeta() {
            UpdateMeta(Asset);
        }

        protected override void Upload() {
            UploadProp(Asset);
        }

        public static void CreateProp() {
            WindowManager.ShowCreateNewProp();
        }

        public static void DownloadProp(PropAssetTemplate tpl) {
            BundleService.Download(tpl);
        }

        public static void UploadProp(PropAsset asset) {
            BundleService.Upload(asset);
        }

        public static void UpdateMeta(PropAsset asset) {
            BundleService.UpdateMeta(asset);
        }

        protected override void DrawGUI(bool guiState) {
            
            GUILayout.BeginHorizontal();
            GUILayout.BeginVertical(GUILayout.Width(370));
            {
                DrawTitleFiled();

                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Placement: ", GUILayout.Width(100));
                Asset.Template.Placing = (PlacingType) EditorGUILayout.EnumPopup(Asset.Template.Placing, GUILayout.Width(240));
                GUILayout.EndHorizontal();

                GUILayout.BeginHorizontal();
                EditorGUILayout.LabelField("Invoke Type: ", GUILayout.Width(100));
                Asset.Template.InvokeType = (PropInvokeType) EditorGUILayout.EnumPopup(Asset.Template.InvokeType, GUILayout.Width(240));
                GUILayout.EndHorizontal();

                if (Asset.HasStandSurface) {
                    Asset.Template.CanStack = false;
                }

                Asset.Template.CanStack = YesNoFiled("Can Stack", Asset.Template.CanStack, 100, 240);
                Asset.Template.DisplaySkins = YesNoFiled("Skins as Props", Asset.Template.DisplaySkins, 100, 240);
                
                DrawStatus();
                DrawNetwork();
            }

            GUILayout.EndVertical();
            GUILayout.BeginVertical(GUILayout.Width(100));
            {
                Asset.Icon = (Texture2D) EditorGUILayout.ObjectField(Asset.Icon, typeof(Texture2D), false, GUILayout.Width(70), GUILayout.Height(70));

                if (Asset.Icon == null) {
                    DrawPreloaderAt(new Rect(525, 65, 32, 32));
                }
            }
            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            var alignment_center = new GUIStyle(EditorStyles.label);
            alignment_center.alignment = TextAnchor.MiddleCenter;

            var alignment_right = new GUIStyle(EditorStyles.label);
            alignment_right.alignment = TextAnchor.MiddleRight;

            GUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("Allowed Scale: ", GUILayout.Width(100));

            var minSize = Asset.Template.MinSize;
            var maxSize = Asset.Template.MaxSize;
            EditorGUILayout.MinMaxSlider(ref minSize, ref maxSize, PropAssetTemplate.MinAllowedAxisSize, PropAssetTemplate.MaxAllowedAxisSize, GUILayout.Width(240)); //    EditorGUILayout.MinMaxSlider (CurrentProp.Template.MinScale, GUILayout.Width (240));
            
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

            var def = Asset.Size * 100f;
            var min = def * Asset.MinScale;
            var max = def * Asset.MaxScale;

            EditorGUILayout.LabelField("Min(" + Mathf.CeilToInt(Asset.MinScale * 100f) + "%): " + (int) min.x + "x" + (int) min.y + "x" + (int) min.z, GUILayout.Width(labelSize));
            EditorGUILayout.LabelField("Default: " + (int) def.x + "x" + (int) def.y + "x" + (int) def.z, alignment_center, GUILayout.Width(labelSize));
            GUILayout.Space(1);
            EditorGUILayout.LabelField("Max(" + Mathf.CeilToInt(Asset.MaxScale * 100f) + "%):" + (int) max.x + "x" + (int) max.y + "x" + (int) max.z, alignment_right, GUILayout.Width(labelSize));
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
                var contentTypes = new List<string>();
                foreach (var t in Asset.Template.ContentTypes) {
                    contentTypes.Add(t.ToString());
                }

                try {
                    ReorderableListGUI.ListField(contentTypes, ContentTypeListItem, DrawEmptyContentType);
                }
                catch (Exception e) {
                    Console.WriteLine(e);
                }

                Asset.Template.ContentTypes = new List<ContentType>();

                foreach (var val in contentTypes) {
                    var parsed = EnumUtility.ParseOrDefault<ContentType>(val);
                    Asset.Template.ContentTypes.Add(parsed);
                }
            }

            GUILayout.EndVertical();
            GUILayout.EndHorizontal();

            DrawControlButtons();
        }

        string ContentTypeListItem(Rect position, string itemValue) {
            position.y += 2;
            if (string.IsNullOrEmpty(itemValue)) {
                itemValue = ContentType.Image.ToString();
            }

            var t = EnumUtility.ParseOrDefault<ContentType>(itemValue);
            t = (ContentType) EditorGUI.EnumPopup(position, t);
            return t.ToString();
        }

        void DrawEmptyContentType() {
            GUILayout.Label("Asset won't support content.", EditorStyles.miniLabel);
        }
    }
}