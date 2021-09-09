////////////////////////////////////////////////////////////////////////////////
//
// @module V2D
// @author Osipov Stanislav lacost.st@gmail.com
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;
using UnityEditor;
using System.Collections;
using SA.Foundation.Config;

namespace SA.Productivity.GoogleSheets {

    public static class GD_EditorMenu {
        private const int MENU_INDEX = SA_Config.ProductivityMenuIndex + 20;

    	//--------------------------------------
    	//  PUBLIC METHODS
    	//--------------------------------------

        [MenuItem(SA_Config.EditorProductivityMenuRoot + "Google Sheets/Sheets", false, MENU_INDEX)]
        public static void Edit() {
            var window = GD_SettingsWindow.ShowTowardsInspector("G Sheets", GD_Skin.SettingsWindowIcon);
            window.SetSelectedTabIndex(0);
        }

        [MenuItem(SA_Config.EditorProductivityMenuRoot + "Google Sheets/Localization", false, MENU_INDEX)]
        public static void Localization() {
            var window = GD_SettingsWindow.ShowTowardsInspector("G Sheets", GD_Skin.SettingsWindowIcon);
            window.SetSelectedTabIndex(1);
        }


        [MenuItem(SA_Config.EditorProductivityMenuRoot + "Google Sheets/Documentation", false, MENU_INDEX)]
        public static void Documentation() {
            Application.OpenURL(GD_Settings.DOCUMENTATION_URL);
        }


    }

}
