using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace RF.AssetWizzard.Editor
{

    public abstract class AssetInspector<T, A> : UnityEditor.Editor where T : Template where A : IAsset
    {

        public void DrawEnvironmentSiwtch() {
            Environment env = GameObject.FindObjectOfType<Environment>();
            if (env != null) {
                EditorGUI.BeginChangeCheck();
                env.RenderEnvironment = EditorGUILayout.Toggle("Render Environment", env.RenderEnvironment);
                if (EditorGUI.EndChangeCheck()) {
                    env.Update();
                }
            }
        }

        public void DrawGizmosSiwtch() {
            Asset.DrawGizmos = EditorGUILayout.Toggle("Draw Gizmos", Asset.DrawGizmos);
        }

        public void DrawActionButtons() {


            EditorGUILayout.Space();
            EditorGUILayout.LabelField("Actions: ", EditorStyles.boldLabel);
            GUILayout.BeginHorizontal();
            {
                GUILayout.FlexibleSpace();
                bool wizzard = GUILayout.Button("Wizzard", EditorStyles.miniButton, new GUILayoutOption[] { GUILayout.Width(120) });
                if (wizzard) {
                    WindowManager.ShowWizard();
                    WindowManager.Wizzard.SiwtchTab(WizardTabs.Wizzard);
                }


                if (string.IsNullOrEmpty(Template.Id)) {

                    bool upload = GUILayout.Button("Upload", EditorStyles.miniButton, new GUILayoutOption[] { GUILayout.Width(120) });
                    if (upload) {
                        BundleService.Upload<A>(Asset);
                    }

                } else {
                    bool re_upload = GUILayout.Button("Re Upload", EditorStyles.miniButton, new GUILayoutOption[] { GUILayout.Width(120) });
                    if (re_upload) {
                        BundleService.Upload<A>(Asset);
                    }

                    bool refresh = GUILayout.Button("Refresh", EditorStyles.miniButton, new GUILayoutOption[] { GUILayout.Width(120) });
                    if (refresh) {
                        BundleService.Download<T>(Template);
                    }
                }

                
            }
            GUILayout.EndHorizontal();
            EditorGUILayout.Space();
        }



        public abstract A Asset { get; }

        public T Template {
            get {
                return (T) Asset.GetTemplate();
            }
        }


    }
}