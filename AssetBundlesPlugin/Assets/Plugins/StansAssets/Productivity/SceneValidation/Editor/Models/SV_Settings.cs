using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;


using SA.Foundation.Config;
using SA.Foundation.Patterns;

namespace SA.Productivity.SceneValidator
{
    public class SV_Settings : SA_ScriptableSingletonEditor<SV_Settings>
    {

        public const string PLUGIN_NAME = "Scene Validator";
        public const string PLUGIN_FOLDER = SA_Config.STANS_ASSETS_PRODUCTIVITY_PLUGINS_PATH + "SceneValidation/";


    


        //UI Settings
       
        //Custom Rules Settings
        public List<MonoScript> CustomRules = new List<MonoScript>();

        //Default Rules Settings
        public bool NamingConventionRule = true;
        [SerializeField] List<SV_NamingConventionRule.ConventionRule> m_conventionRules = new List<SV_NamingConventionRule.ConventionRule>();
        public List<MonoScript> ConventionResolvers = new List<MonoScript>();
        public bool MissingReferenceDetectionRule = true;


        public List<SV_NamingConventionRule.ConventionRule> ConventionRules {
            get {
                if(m_conventionRules.Count == 0) {
                    m_conventionRules.Add(new SV_NamingConventionRule.ConventionRule("GameObject", SV_NamingConventionRule.StringValidationMethod.Equals));
                    m_conventionRules.Add(new SV_NamingConventionRule.ConventionRule("Button", SV_NamingConventionRule.StringValidationMethod.Equals));
                    m_conventionRules.Add(new SV_NamingConventionRule.ConventionRule("Text", SV_NamingConventionRule.StringValidationMethod.Equals));
                    m_conventionRules.Add(new SV_NamingConventionRule.ConventionRule("Panel", SV_NamingConventionRule.StringValidationMethod.Equals));
                    m_conventionRules.Add(new SV_NamingConventionRule.ConventionRule("(", SV_NamingConventionRule.StringValidationMethod.Contains));
                }

                return m_conventionRules;
            }
        }


        //--------------------------------------
        // Static
        //--------------------------------------

        private static SV_UserSettings s_userSettings = null;
        public static SV_UserSettings UserSettings {
            get {

                if(s_userSettings == null) {
                    if (EditorPrefs.HasKey(SV_UserSettings.PLAYER_PREFS_KEY)) {
                        string json = EditorPrefs.GetString(SV_UserSettings.PLAYER_PREFS_KEY);
                        s_userSettings  = JsonUtility.FromJson<SV_UserSettings>(json);
                    } else {
                        s_userSettings  = new SV_UserSettings();
                    }
                }

                return s_userSettings;
            }
        }

        public static void SaveUserSettings(SV_UserSettings settings) {
            string json = JsonUtility.ToJson(settings);
            EditorPrefs.SetString(SV_UserSettings.PLAYER_PREFS_KEY, json);

            s_userSettings = settings;
        }


        public static bool IsValidationActive {
            get {
                return !EditorApplication.isPlaying && UserSettings.ValidationEnabled;
            }
        }

        private static PluginVersionHandler s_pluginVersion;

        public static PluginVersionHandler PluginVersion {
            get {
                if (s_pluginVersion == null) {
                    s_pluginVersion = new PluginVersionHandler(PLUGIN_FOLDER);
                }
                return s_pluginVersion;
            }
        }

        public static string GetFormattedVersion() {
            return string.Format("2018.{0}.{1}", SA_Config.FoundationVersion.GetVersion(), PluginVersion.GetVersion());
        }
    }
}