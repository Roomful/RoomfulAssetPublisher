using UnityEngine;
using UnityEditor;
using System.Collections;

using SA.Foundation.Editor;
using StansAssets.Plugins.Editor;

namespace SA.Analytics.Google
{

    public class GA_SettingsWindow : SA_PluginSettingsWindow<GA_SettingsWindow>
    {

        protected override void OnAwake() {
            SetHeaderTitle("Google Analytics");
            SetHeaderDescription("Implement all the power of Google Analytics in your game. " +
                                 "The plugin has a clean and easy to use implementation of Google Analytics Measurement Protocol.");
            SetHeaderVersion(GA_Settings.VERSION_NUMBER);
            SetDocumentationUrl("https://unionassets.com/google-analytics-sdk");


            AddMenuItem("GENERAL", CreateInstance<GA_SettingsGeneralTab>());
            AddMenuItem("PLATFORMS", CreateInstance<GA_SettingsPlatfromsTab>());
            AddMenuItem("ABOUT", CreateInstance<IMGUIAboutTab>());

        }

        protected override void BeforeGUI() {
            EditorGUI.BeginChangeCheck();
        }


        protected override void AfterGUI() {
            if (EditorGUI.EndChangeCheck()) {
                GA_Settings.Save();
            }
        }
    }
}