using UnityEngine;
using UnityEditor;

namespace net.roomful.assets.editor
{
    internal abstract class AssetInspector<T, TAsset> : UnityEditor.Editor where T : AssetTemplate where TAsset : class, IAsset
    {
        public void DrawEnvironmentSwitch() {
            var env = FindObjectOfType<Environment>();
            if (env != null) {
                EditorGUI.BeginChangeCheck();
                env.RenderEnvironment = EditorGUILayout.Toggle("Render Environment", env.RenderEnvironment);
                if (EditorGUI.EndChangeCheck()) {
                    env.Update();
                }
            }
        }

        protected void DrawGizmosSwitch() {
            Asset.DrawGizmos = EditorGUILayout.Toggle("Draw Gizmos", Asset.DrawGizmos);
        }

        protected void DrawActionButtons(bool drawRefreshButton = true) {
            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Actions: ", EditorStyles.boldLabel);
            GUILayout.BeginHorizontal();
            {
                GUILayout.FlexibleSpace();
                var wizzard = GUILayout.Button("Wizzard", EditorStyles.miniButton, GUILayout.Width(120));
                if (wizzard) {
                    WindowManager.ShowWizard();
                    WindowManager.Wizzard.SwitchTab(WizardTabs.Wizzard);
                }

                if (string.IsNullOrEmpty(Template.Id)) {
                    var upload = GUILayout.Button("Upload", EditorStyles.miniButton, GUILayout.Width(120));
                    if (upload) {
                        BundleService.Upload(Asset);
                    }
                }
                else {
                    var reUpload = GUILayout.Button("Re Upload", EditorStyles.miniButton, GUILayout.Width(120));
                    if (reUpload) {
                        BundleService.ReUpload(Asset);
                    }
                    
                    var updateMeta = GUILayout.Button("Update Meta", EditorStyles.miniButton, GUILayout.Width(120));
                    if (updateMeta) {
                        BundleService.UpdateMeta(Asset);
                    }

                    if (drawRefreshButton)
                    {
                        var refresh = GUILayout.Button("Refresh", EditorStyles.miniButton, GUILayout.Width(120));
                        if (refresh) {
                            BundleService.Download(Template);
                        }
                    }
                }
            }
            GUILayout.EndHorizontal();


            EditorGUILayout.Space();
        }

        protected abstract TAsset Asset { get; }

        private T Template => (T) Asset.GetTemplate();
    }
}
