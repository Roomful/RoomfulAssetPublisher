using UnityEngine;
using System.IO;
using System.Collections.Generic;

namespace net.roomful.assets
{
    internal class AssetBundlesSettings : ScriptableObject
    {
        //--------------------------------------
        // Constants
        //--------------------------------------
        
        private const string SETTINGS_ASSET_EXTENSION = ".asset";

        private const string PACKAGE_NAME = "net.roomful.assets";
        private const string PACKAGE_LOCATION = "Packages/" + PACKAGE_NAME + "/";
        private const string PLUGINS_EDITOR_ROOMFUL = "Plugins/Roomful/Editor/";

        private const string SETTINGS_LOCATION = PLUGINS_EDITOR_ROOMFUL + "Settings/Resources/";
        public const string ASSETS_TEMP_LOCATION = PLUGINS_EDITOR_ROOMFUL + "Temp/";
        public const string ASSETS_RESOURCES_LOCATION = PLUGINS_EDITOR_ROOMFUL + "Bundles/";

        public const string WEB_SERVER_URL = "https://api.roomful.net";

        public const string FULL_ASSETS_TEMP_LOCATION = "Assets/" + ASSETS_TEMP_LOCATION;

        public const string FULL_ASSETS_RESOURCES_LOCATION = "Assets/" + ASSETS_RESOURCES_LOCATION;

        public const string PLUGIN_PREFABS_LOCATION = PACKAGE_LOCATION + "Editor/Prefabs/";

        public const string THUMBNAIL_POINTER = "rf_prop_thumbnail_pointer";
        public const string THUMBNAIL_RESOURCE_INDEX_BOUND = "ResourceIndexBound";

        //--------------------------------------
        // Session Data
        //--------------------------------------

        [SerializeField] private string m_sessionId = string.Empty;

        [SerializeField] public List<PropAssetTemplate> m_localPropTemplates = new List<PropAssetTemplate>();
        [SerializeField] public List<StyleAssetTemplate> m_localStyleTemplates = new List<StyleAssetTemplate>();
        [SerializeField] public List<EnvironmentAssetTemplate> m_localEnvironmentsTemplates = new List<EnvironmentAssetTemplate>();
       
        [field: SerializeField]
        public int WizardWindowSelectedTabIndex { get; set; } = 0;

        [field: SerializeField]
        public int UploadPlatformIndex { get; set; } = 0;

        //--------------------------------------
        // Config
        //--------------------------------------

        public bool m_showWebInLogs = false;
        public bool m_showWebOutLogs = true;
        public bool m_automaticCacheClean = true;
        public bool m_downloadAssetAfterUploading = true;

#if UNITY_EDITOR
        [SerializeField] private List<UnityEditor.BuildTarget> m_targetPlatforms = new List<UnityEditor.BuildTarget>();
        public List<UnityEditor.BuildTarget> TargetPlatforms => m_targetPlatforms;
#endif

        private static AssetBundlesSettings s_instance = null;

        public static AssetBundlesSettings Instance {
            get {
                if (s_instance == null) {
                    s_instance = Resources.Load(nameof(AssetBundlesSettings)) as AssetBundlesSettings;  

                    if (s_instance == null) {
                        s_instance = CreateInstance<AssetBundlesSettings>();

#if UNITY_EDITOR
                        FolderUtils.CreateFolder(SETTINGS_LOCATION);
                        var fullPath = Path.Combine(Path.Combine("Assets", SETTINGS_LOCATION),
                            nameof(AssetBundlesSettings) + SETTINGS_ASSET_EXTENSION
                        );

                        UnityEditor.AssetDatabase.CreateAsset(s_instance, fullPath);
#endif
                    }
                }

                return s_instance;
            }
        }

        //--------------------------------------
        // Get / Set
        //--------------------------------------

        public string PublisherCurrentVersion {
            get {
#if UNITY_2018_3_OR_NEWER
                return "4.0";
#elif UNITY_2017_3_OR_NEWER
                return "3.0";
#else
                return "2.0";
#endif
            }
        }

        public string SessionId => m_sessionId;

        public bool IsLoggedIn => string.IsNullOrEmpty(SessionId);

        //--------------------------------------
        // Public Methods
        //--------------------------------------

        public void SetSessionId(string id) {
            m_sessionId = id;
            Save();
        }

        public static void Save() {
#if UNITY_EDITOR
            UnityEditor.EditorUtility.SetDirty(Instance);
            UnityEditor.AssetDatabase.SaveAssets();
#endif
        }

        public void RemoveSavedTemplate(AssetTemplate tpl) {
            RemoveTemplateFromList(tpl, m_localPropTemplates);
            RemoveTemplateFromList(tpl, m_localStyleTemplates);
            RemoveTemplateFromList(tpl, m_localEnvironmentsTemplates);

            Save();
        }

        public void ReplaceSavedTemplate(AssetTemplate tpl) {
            ReplaceTemplateInList(tpl, m_localPropTemplates);
            ReplaceTemplateInList(tpl, m_localStyleTemplates);
            ReplaceTemplateInList(tpl, m_localEnvironmentsTemplates);

            Save();
        }

        //--------------------------------------
        // Private Methods
        //--------------------------------------

        private void ReplaceTemplateInList<T>(AssetTemplate tpl, List<T> templates) where T : AssetTemplate {
            for (var i = 0; i < templates.Count; i++) {
                if (templates[i].Id.Equals(tpl.Id)) {
                    templates[i] = (T) tpl;
                    return;
                }
            }
        }

        private void RemoveTemplateFromList<T>(AssetTemplate tpl, List<T> templates) where T : AssetTemplate {
            for (var i = 0; i < templates.Count; i++) {
                if (templates[i].Id.Equals(tpl.Id)) {
                    templates.Remove(templates[i]);
                    return;
                }
            }
        }
    }
}