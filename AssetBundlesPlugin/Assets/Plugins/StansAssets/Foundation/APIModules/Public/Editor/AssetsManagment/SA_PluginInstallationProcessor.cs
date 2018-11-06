using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

using SA.Foundation.Patterns;

namespace SA.Foundation.Editor
{
    public abstract class SA_PluginInstallationProcessor
    {


        private SA_ScriptableSettings m_pluginSettings;
        private SA_PluginFirstInstallInfo m_pluginFirstInstallInfo;


        public void Init(SA_ScriptableSettings pluginSettings) {
            m_pluginSettings = pluginSettings;
            string version = m_pluginSettings.GetFormattedVersion();

            string key = pluginSettings.name + "_instalation_info";

            m_pluginFirstInstallInfo = null;
            if(EditorPrefs.HasKey(key)) {
                string json = EditorPrefs.GetString(key);
                m_pluginFirstInstallInfo = JsonUtility.FromJson<SA_PluginFirstInstallInfo>(json);
            }

            if (m_pluginFirstInstallInfo == null) {
                m_pluginFirstInstallInfo = new SA_PluginFirstInstallInfo(version);
                m_pluginFirstInstallInfo.SetCurrentVersion(version);
                UpdayeVersionInfo(key, pluginSettings, m_pluginFirstInstallInfo);

            } else {
                if (!m_pluginFirstInstallInfo.CurrentVersion.Equals(version)) {
                    m_pluginFirstInstallInfo.SetCurrentVersion(version);
                    UpdayeVersionInfo(key, pluginSettings, m_pluginFirstInstallInfo);
                } else {

                }
            }

        }

        private void UpdayeVersionInfo(string key, SA_ScriptableSettings settings, SA_PluginFirstInstallInfo info) {
            string json = JsonUtility.ToJson(info);
            EditorPrefs.SetString(key, json);

            OnInstall();

            Debug.Log(settings.name + " " + info.CurrentVersion + " installed.");
        }


        public SA_PluginFirstInstallInfo PluginFirstInstallInfo {
            get {
                return m_pluginFirstInstallInfo;
            }
        }

        protected abstract void OnInstall();


        public static bool IsDevelopmentMode {
            get {
#if SA_DEVELOPMENT_PROJECT
                return true;
#else
                return false;
#endif
            }
        }


    }
}