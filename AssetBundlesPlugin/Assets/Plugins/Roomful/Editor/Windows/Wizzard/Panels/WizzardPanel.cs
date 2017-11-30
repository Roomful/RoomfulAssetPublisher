using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using Rotorz.ReorderableList;

namespace RF.AssetWizzard.Editor
{

    public class WizzardPanel : Panel
    {

        private List<IAssetWizzard> m_wizards = null;


        public WizzardPanel(EditorWindow window) : base(window) { }

        public override void OnGUI() {

            if (BundleService.IsUploadInProgress) {
                DrawPreloaderAt(new Rect(570, 12, 20, 20));
                GUI.enabled = false;
            }

            GUILayout.Space(10f);
            foreach(IAssetWizzard wizard in Wizards) {
                if(wizard.HasAsset) {
                    wizard.OnGUI(GUI.enabled);
                    Window.Repaint();
                    return;
                }
            }

            NoAssetWizard();

            Window.Repaint();
        }

     
        
        private List<IAssetWizzard> Wizards {
            get {
                if(m_wizards == null || m_wizards.Count == 0) {
                    m_wizards = new List<IAssetWizzard>();
                    m_wizards.Add(new PropWizzard());
                    m_wizards.Add(new EnvironmentWizzard());
                }

                return m_wizards;
            }
        }



        private void NoAssetWizard() {

            GUILayout.Label("Create New Roomful Asset", EditorStyles.boldLabel);
            GUILayout.Space(10);


            Texture2D prop_icon = IconManager.GetIcon(Icon.prop_icon_light);
            if (EditorGUIUtility.isProSkin) {
                prop_icon = IconManager.GetIcon(Icon.prop_icon);
            }

            Texture2D environment_icon = IconManager.GetIcon(Icon.environment_icon_light);
            if (EditorGUIUtility.isProSkin) {
                environment_icon = IconManager.GetIcon(Icon.environment_icon);
            }

            Texture2D style_icon = IconManager.GetIcon(Icon.style_icon_light);
            if (EditorGUIUtility.isProSkin) {
                style_icon = IconManager.GetIcon(Icon.style_icon);
            }


            string propMsg = "Props are objects that can be placed in rooms" +
                "\n" +
                "Upload any 3D model, and share it with the world" ;
            DrawCreateAssetItem("Prop", propMsg, prop_icon, () => {
                WindowManager.ShowCreateNewProp();
            });


            string styleMsg = "Styles are the building blocks for rooms" +
                "\n" +
                "Create your unique and awesome style!";
            DrawCreateAssetItem("Style", styleMsg, style_icon, () => {
                WindowManager.ShowCreateNewStyle();
            });


            string envMsg = "Environments are effects that make rooms come alive" +
               "\n" +
               "Like a skybox, sounds, particles and visual effects";
            DrawCreateAssetItem("Environment", envMsg, environment_icon, () => {
                WindowManager.ShowCreateNewEnvironment();
            });

        }


        private void DrawCreateAssetItem(string title, string msg, Texture2D image, System.Action callback) {

            var options = new GUILayoutOption[2] { GUILayout.Width(75), GUILayout.Height(75) };
            var leftPannelOptions = new GUILayoutOption[2] { GUILayout.Width(75), GUILayout.Height(100) };
            var rightPannelOptions = new GUILayoutOption[2] { GUILayout.Width(350), GUILayout.Height(100) };

            GUIContent createAssetContent = new GUIContent();
            createAssetContent.image = image;

            bool imageButton;
            bool createButton;

            GUILayout.BeginHorizontal();
            {

                GUILayout.BeginVertical(leftPannelOptions);
                {
                    imageButton = GUILayout.Button(createAssetContent, options);
                }
                GUILayout.EndVertical();

                GUILayout.BeginVertical(rightPannelOptions);
                {
                    GUILayout.Label(title, EditorStyles.boldLabel);
                    GUILayout.Label(msg, GUILayout.Width(320));
                    GUILayout.Space(5);
                    GUILayout.BeginHorizontal();
                    GUILayout.FlexibleSpace();
                    createButton = GUILayout.Button("Create", GUILayout.Width(90));
                    GUILayout.EndHorizontal();
                }
                GUILayout.EndVertical();
            }
            GUILayout.EndHorizontal();

            if(createButton || imageButton) {
                callback();
            }
        }



    }
}
