using System;
using net.roomful.api;
using StansAssets.Plugins.Editor;
using UnityEngine;
using UnityEditor;

namespace net.roomful.assets.editor
{
    class StyleWizard : AssetWizard<BaseStyleAsset>
    {
        protected override void Create() {
            WindowManager.ShowCreateNewStyle();
        }

        protected override void Download() {
            BundleService.Download(Asset.Template);
        }

        protected override void UpdateMeta() {
            BundleService.UpdateMeta(Asset);
        }

        protected override void Upload() {
            BundleService.Upload(Asset);
        }

        protected override void DrawGUI(bool guiState) {
            GUILayout.BeginHorizontal();

            GUILayout.BeginVertical(GUILayout.Width(370));
            {
                DrawTitleFiled();

                using (new IMGUIBeginHorizontal()) {
                    EditorGUILayout.LabelField("Home X: ", GUILayout.Width(100));
                    var homePosition = Asset.Template.HomePosition;
                    homePosition.x = EditorGUILayout.FloatField(homePosition.x, GUILayout.Width(100));
                    Asset.Template.HomePosition = homePosition;
                }

                using (new IMGUIBeginHorizontal()) {
                    EditorGUILayout.LabelField("Home Y: ", GUILayout.Width(100));
                    var homePosition = Asset.Template.HomePosition;
                    homePosition.y = EditorGUILayout.FloatField(homePosition.y, GUILayout.Width(100));
                    Asset.Template.HomePosition = homePosition;
                }

                using (new IMGUIBeginHorizontal()) {
                    EditorGUILayout.LabelField("Home Z: ", GUILayout.Width(100));
                    var homePosition = Asset.Template.HomePosition;
                    homePosition.z = EditorGUILayout.FloatField(homePosition.z, GUILayout.Width(100));
                    Asset.Template.HomePosition = homePosition;
                }

                using (new IMGUIBeginHorizontal()) {
                    EditorGUILayout.LabelField("Price (in USD): ", GUILayout.Width(100));
                    var price = EditorGUILayout.FloatField(Convert.ToSingle(Asset.Template.Price), GUILayout.Width(100));
                    Asset.Template.Price = Convert.ToDecimal(price);
                }
                
                using (new IMGUIBeginHorizontal()) {
                    EditorGUILayout.LabelField("Sorting Score: ", GUILayout.Width(100));
                    Asset.Template.Score = EditorGUILayout.IntField(Asset.Template.Score, GUILayout.Width(100));
                }

                using (new IMGUIBeginHorizontal()) {
                    EditorGUILayout.LabelField("Type: ", GUILayout.Width(100));
                    Asset.Template.StyleType = (StyleType) EditorGUILayout.EnumPopup(Asset.Template.StyleType, GUILayout.Width(240));
                }

                using (new IMGUIBeginHorizontal()) {
                    EditorGUILayout.LabelField("Doors: ", GUILayout.Width(100));
                    Asset.Template.DoorsType = (StyleDoorsType) EditorGUILayout.EnumPopup(Asset.Template.DoorsType, GUILayout.Width(240));
                }
                
                DrawStatus();
                DrawNetwork();
                
                using (new IMGUIBeginHorizontal()) {
                    EditorGUILayout.LabelField("Only available for Roomful admins: ", GUILayout.Width(200));
                    var testStyle = Asset.Template.Tags.Contains("test");
                    testStyle = EditorGUILayout.Toggle(testStyle);
                    if (testStyle && !Asset.Template.Tags.Contains("test")) {
                        Asset.Template.Tags.Add("test");
                    }

                    if (!testStyle && Asset.Template.Tags.Contains("test")) {
                        Asset.Template.Tags.Remove("test");
                    }
                }

                using (new IMGUIBeginHorizontal()) {
                    EditorGUILayout.LabelField("Can be extended with other styles: ", GUILayout.Width(200));
                    var extendable = Asset.Template.Tags.Contains("extendable");
                    extendable = EditorGUILayout.Toggle(extendable);
                    if (extendable && !Asset.Template.Tags.Contains("extendable")) {
                        Asset.Template.Tags.Add("extendable");
                    }

                    if (!extendable && Asset.Template.Tags.Contains("extendable")) {
                        Asset.Template.Tags.Remove("extendable");
                    }
                }
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

            DrawTags();
            DrawControlButtons();

            GUI.enabled = true;
        }
    }
}