﻿using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Linq;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RF.AssetWizzard {

	#if UNITY_EDITOR
	[InitializeOnLoad]
	#endif
	public class AssetBundlesSettings : ScriptableObject {


        //--------------------------------------
        // Constants
        //--------------------------------------

        private const string SettingsAssetName = "AssetBundlesSettings";
        private const string SettingsAssetExtension = ".asset";

        public static string WEB_SERVER_URL = "https://demo.roomful.co:5443";
	
		public const string ASSETS_TEMP_LOCATION = "Roomful/Temp/";
		public const string FULL_ASSETS_TEMP_LOCATION = "Assets/" + ASSETS_TEMP_LOCATION;

        public const string ASSETS_RESOURCES_LOCATION = "Roomful/Bundles";
        public const string FULL_ASSETS_RESOURCES_LOCATION = "Assets/" + ASSETS_RESOURCES_LOCATION;

        public const string PLUGIN_LOCATION = "Assets/Plugins/Roomful/";
		public const string PLUGIN_PREFABS_LOCATION = PLUGIN_LOCATION + "Prefabs/";

		public const string SETTINGS_LOCATION = "Plugins/Roomful/Editor/Resources/Settings/";

        public const float MAX_AlLOWED_SIZE = 4f;
        public const float MIN_ALLOWED_SIZE = 0.3f;

        public const string THUMBNAIL_POINTER = "rf_prop_thumbnail_pointer";
        public const string THUMBNAIL_RESOURCE_INDEX_BOUND = "ResourceIndexBound";



        //--------------------------------------
        // Session Data
        //--------------------------------------

        [SerializeField]
		private string m_sessionId = string.Empty;

        [SerializeField]
        public List<PropTemplate> LocalPropTemplates = new List<PropTemplate>();
        [SerializeField]
        public List<StyleTemplate> LocalStyleTemplates = new List<StyleTemplate>();
        [SerializeField]
        public List<EnvironmentTemplate> LocalEnvironmentsTemplates = new List<EnvironmentTemplate>();

        public int UploadPlatfromIndex = 0;
        public int WizardWindowSelectedTabIndex = 0;


        //--------------------------------------
        // Config
        //--------------------------------------


        public bool ShowWebInLogs = true;
		public bool ShowWebOutLogs = false;
        public bool AutomaticCacheClean = true;

   

        #if UNITY_EDITOR
        public List<BuildTarget> TargetPlatforms = new List<BuildTarget>();
		#endif

		private static AssetBundlesSettings _Instance = null;
		public static AssetBundlesSettings Instance {
			get {
				if (_Instance == null) {
					_Instance = Resources.Load("Settings/" + SettingsAssetName) as AssetBundlesSettings;

					if (_Instance == null) {

						_Instance = CreateInstance<AssetBundlesSettings>();

						#if UNITY_EDITOR
						FolderUtils.CreateFolder(SETTINGS_LOCATION);
						string fullPath = Path.Combine(Path.Combine("Assets", SETTINGS_LOCATION),
							SettingsAssetName + SettingsAssetExtension
						);

						AssetDatabase.CreateAsset(_Instance, fullPath);
						#endif
					}
				}

				return _Instance;
			}
		}


        //--------------------------------------
        // Get / Set
        //--------------------------------------

        public string PublisherCurrentVersion {
			get {
                return "2.0";
			}
        }

		public string SessionId {
			get {
				return m_sessionId;
			}
		}

        public bool IsLoggedIn {
            get {
                return string.IsNullOrEmpty(SessionId);
            }
        }



        //--------------------------------------
        // Public Methods
        //--------------------------------------


        public void SetSessionId(string id) {
			m_sessionId = id;
			Save ();
		}

		public static void Save() {
			#if UNITY_EDITOR
			EditorUtility.SetDirty(Instance);
			AssetDatabase.SaveAssets();
			#endif
		}

		public void RemoveSavedTemplate(Template tpl) {

            RemoveTemplateFromList<PropTemplate>(tpl, LocalPropTemplates);
            RemoveTemplateFromList<StyleTemplate>(tpl, LocalStyleTemplates);
            RemoveTemplateFromList<EnvironmentTemplate>(tpl, LocalEnvironmentsTemplates);


            Save();
        }

        public void ReplaceSavedTemplate(Template tpl) {
            ReplaceTemplateInList<PropTemplate>(tpl, LocalPropTemplates);
            ReplaceTemplateInList<StyleTemplate>(tpl, LocalStyleTemplates);
            ReplaceTemplateInList<EnvironmentTemplate>(tpl, LocalEnvironmentsTemplates);

            Save();
        }


        //--------------------------------------
        // Private Methods
        //--------------------------------------

        private void ReplaceTemplateInList<T>(Template tpl, List<T> templates)  where T : Template {
            for (int i = 0; i < templates.Count; i++) {
                if (templates[i].Id.Equals(tpl.Id)) {
                    templates[i] = (T) tpl;
                    return;
                }
            }
        }

        private void RemoveTemplateFromList<T>(Template tpl, List<T> templates) where T : Template {
            for (int i = 0; i < templates.Count; i++) {
                if (templates[i].Id.Equals(tpl.Id)) {
                    templates.Remove(templates[i]);
                    return;
                }
            }
        }

    }
}

