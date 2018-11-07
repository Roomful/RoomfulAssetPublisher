using UnityEngine;
using UnityEditor;
using SA.Foundation.Config;



namespace SA.Productivity.SceneValidator
{
    public class SV_EditorMenu : MonoBehaviour
    {

        private const int MENU_INDEX = SA_Config.PRODUCTIVITY_MENU_INDEX + 30;

        public class ISD_EditorMenu : EditorWindow
        {

            [MenuItem(SA_Config.EDITOR_PRODUCTIVITY_MENU_ROOT + "Scene Validation/Settings", false, MENU_INDEX)]
            public static void Edit() {
                OpenReportTab();
            }

        }



        public static void OpenReportTab() {
            SV_SettingsWindow.ShowTowardsInspector("Validation", SV_Skin.SettingsWindowIcon);
        }
    }
}