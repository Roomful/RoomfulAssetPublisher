using System;
using UnityEditor;
using UnityEngine;
using SA.Foundation.Config;

namespace SA.Productivity.Console
{
    public static class UCL_EditorMenu
    {

        private const int MENU_INDEX = SA_Config.ProductivityMenuIndex + 10;


        [MenuItem(SA_Config.EditorProductivityMenuRoot + "Console/Show", false, MENU_INDEX)]
        public static void ShowConsole() {
            Type inspectorType = Type.GetType("UnityEditor.ConsoleWindow, UnityEditor.dll");
            var window = EditorWindow.GetWindow<UCL_ConsoleWindow>(new Type[] { inspectorType });
            window.Show();
        }

        public static void CloseConsole() {
            var window = EditorWindow.GetWindow<UCL_ConsoleWindow>();
            window.Close();
        }


        [MenuItem(SA_Config.EditorProductivityMenuRoot + "Console/Settings", false, MENU_INDEX)]
        public static void ShowSettings() {
            UCL_ConsolePreferencesWindow.ShowAsModal();
        }


        [MenuItem(SA_Config.EditorProductivityMenuRoot + "Console/Documentation", false, MENU_INDEX)]
        public static void Documentation() {
            Application.OpenURL("https://unionassets.com/ultimate-logger/manual");
        }
    }
}