using UnityEngine;
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

        public const string WEB_SERVER_URL = "https://dev.roomful.net";
	
		public const string ASSETS_TEMP_LOCATION = "Roomful/Temp/";
		public const string FULL_ASSETS_TEMP_LOCATION = "Assets/" + ASSETS_TEMP_LOCATION;
        public const string FULL_AUTOMATIC_REUPLOADER_TEMP_LOCATION = "Assets/Roomful/AutoReUploaderTemp/AutomaticLoaderInProgress/";

        public const string ASSETS_RESOURCES_LOCATION = "Roomful/Bundles";
        public const string FULL_ASSETS_RESOURCES_LOCATION = "Assets/" + ASSETS_RESOURCES_LOCATION;
        

        public const string PLUGIN_LOCATION = "Assets/Plugins/Roomful/AssetPublisher/";
		public const string PLUGIN_PREFABS_LOCATION = PLUGIN_LOCATION + "Prefabs/";

		public const string SETTINGS_LOCATION = "Plugins/Roomful/AssetPublisher/Editor/Resources/Settings/";

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


        public bool ShowWebInLogs = false;
		public bool ShowWebOutLogs = true;
        public bool AutomaticCacheClean = true;
        public bool DownloadAssetAfterUploading = true;
   

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

		public bool IsLoggedIn => !string.IsNullOrEmpty(SessionId);


		//--------------------------------------
        // Public Methods
        //--------------------------------------


        public void SetSessionId(string id) {
            Debug.Log("SetSessionId " + id);
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

