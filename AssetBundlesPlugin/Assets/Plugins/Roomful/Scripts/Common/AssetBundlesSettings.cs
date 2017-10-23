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

        
        //public int PublisherCurrentVersionIndex = 1;
        //public string[] PublisherExistingVersions = new string[] {"1.0", "2.0"};

        public static string WEB_SERVER_URL = "https://demo.roomful.co:3443";
	
		public const string ASSETS_PREFABS_LOCATION = "Roomful/Prefabs/";
		public const string FULL_ASSETS_PREFABS_LOCATION = "Assets/" + ASSETS_PREFABS_LOCATION;

        public const string ASSETS_RESOURCES_LOCATION = "Roomful/Bundles";
        public const string FULL_ASSETS_RESOURCES_LOCATION = "Assets/" + ASSETS_RESOURCES_LOCATION;

        public const string PLUGIN_LOCATION = "Assets/Plugins/Roomful/";
		public const string PLUGIN_PREFABS_LOCATION = PLUGIN_LOCATION + "Prefabs/";

		public const string SETTINGS_LOCATION = "Plugins/Roomful/Editor/Resources/Settings/";

	


        private const string SettingsAssetName = "AssetBundlesSettings";
        private const string SettingsAssetExtension = ".asset";

        [SerializeField]
		private string m_sessionId = string.Empty;
        
		public string SeartchPattern = string.Empty;
		public SeartchRequestType SeartchType = SeartchRequestType.ByTag;

		public List<AssetTemplate> LocalAssetTemplates = new List<AssetTemplate>();

		public List<AssetTemplate> TemporaryAssetTemplates = new List<AssetTemplate>(); //using for automatic reuploader

		public const float MAX_AlLOWED_SIZE = 4f;
		public const float MIN_ALLOWED_SIZE = 0.3f;

		public const string THUMBNAIL_POINTER = "rf_prop_thumbnail_pointer";
        public const string THUMBNAIL_RESOURCE_INDEX_BOUND = "ResourceIndexBound";

        public bool ShowWebInLogs = true;
		public bool ShowWebOutLogs = false;
        public bool AutomaticCacheClean = true;

        public string LastBundlePath = string.Empty;

        public AssetTemplate UploadTemplate = null;
        public int UploadPlatfromIndex = 0;

		public bool IsInAutoloading = false;


        public int WizardWindowSelectedTabIndex = 0;

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

        public bool IsUploadInProgress {
            get {
                if (AssetBundlesSettings.Instance.UploadTemplate == null || AssetBundlesSettings.Instance.UploadTemplate.Id.Equals(string.Empty)) {
                    return false;
                }

                return true;
            }
        }

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

		public void ReplaceTemplate(AssetTemplate tpl) {
			for(int i = 0; i < LocalAssetTemplates.Count; i++) {
				if(LocalAssetTemplates[i].Id.Equals(tpl.Id)) {
					LocalAssetTemplates [i] = tpl;
					Save ();
					return;
				}
			}
		}


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

		public void RemoverFromLocalAssetTemplates(AssetTemplate tpl) {
			LocalAssetTemplates.Remove (tpl);

		}

		public bool IsAssetInLocal(string id) {
			foreach (AssetTemplate at in LocalAssetTemplates) {
				if (at.Id.Equals (id)) {
					return true;
				}
			}

			return false;
		}
	}
}

