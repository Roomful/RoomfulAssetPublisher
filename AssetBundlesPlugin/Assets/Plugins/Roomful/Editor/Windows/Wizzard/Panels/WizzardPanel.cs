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

            if (AssetBundlesSettings.Instance.IsUploadInProgress) {
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
        }

     
        
        private List<IAssetWizzard> Wizards {
            get {
                if(m_wizards == null || m_wizards.Count == 0) {
                    m_wizards = new List<IAssetWizzard>();
                    m_wizards.Add(new PropWizzard());
                }

                return m_wizards;
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




    }
}
