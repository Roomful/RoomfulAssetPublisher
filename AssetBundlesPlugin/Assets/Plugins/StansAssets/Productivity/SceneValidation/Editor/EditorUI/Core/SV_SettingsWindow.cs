using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using SA.Foundation.Editor;
using SA.Foundation.Config;


namespace SA.Productivity.SceneValidator
{
    public class SV_SettingsWindow : SA_PluginSettingsWindow<SV_SettingsWindow>
    {

        protected override void OnAwake() {
            SetHeaderTitle(SV_Settings.PLUGIN_NAME);
            SetHeaderDescription("As good developer you are alwasy try keep your code without compilation issues or warnings. " +
                "Unity environment and a lot of diffrent IDE's designed to help you with this. " +
                "This toll was designed to help you keep your scene clean & orginized, and not to allow " +
                "critical issues to go inside the production build.");
            SetHeaderVersion(SV_Settings.GetFormattedVersion());
            SetDocumentationUrl("https://unionassets.com/ios-deploy");


            
            AddMenuItem("REPORT", CreateInstance<SV_ReportTab>());
            AddMenuItem("SETTINGS", CreateInstance<SV_SettingsTab>());
            AddMenuItem("GUIDES", CreateInstance<SV_RulesTab>());
            AddMenuItem("ABOUT", CreateInstance<SA_PluginAboutLayout>());

        }

        protected override void BeforeGUI() {
            EditorGUI.BeginChangeCheck();
        }


        protected override void AfterGUI() {
            if (EditorGUI.EndChangeCheck()) {

                foreach(var script in SV_Settings.Instance.CustomRules) {
                    if (script != null) {
                        try {
                            var rule = Activator.CreateInstance(script.GetClass());
                            if (!(rule is SV_iValidationRule)) {
                                SV_Settings.Instance.CustomRules.Remove(script);
                                ShowNotification(new GUIContent("The " + script.name + " has to implement SV_iValidationRule interface."));
                                break;
                            }
                        } catch(Exception ex) {
                            SV_Settings.Instance.CustomRules.Remove(script);
                            ShowNotification(new GUIContent("Failed to read " + script.name + " " + ex.Message));
                        }
                      
                    }
                }


                foreach (var script in SV_Settings.Instance.ConventionResolvers) {
                    if (script != null) {

                        try {
                            var rule = Activator.CreateInstance(script.GetClass());
                            if (!(rule is SV_NamingConventionRule.SV_iNamingConventionResolver)) {
                                SV_Settings.Instance.ConventionResolvers.Remove(script);
                                ShowNotification(new GUIContent("The " + script.name + " has to implement SV_iNamingConventionResolver interface."));
                                break;
                            }
                        } catch(Exception ex) {
                            SV_Settings.Instance.ConventionResolvers.Remove(script);
                            ShowNotification(new GUIContent("Failed to read " + script.name + " " + ex.Message));
                        }
                    }
                }

                SV_Settings.Save();
            }
        }
    }
}