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


		public static string WEB_SERVER_URL = "https://demo.roomful.co:4443"; 
	


		public const string ASSETS_LOCATION = "Roomful/Assets/";
		public const string FULL_ASSETS_LOCATION = "Assets/" + ASSETS_LOCATION;

		public const string SETTINGS_LOCATION = "Plugins/Roomful/Settings/Editor/Resources/";


		private const string SettingsAssetName = "AssetBundlesSettings";
		private const string SettingsAssetExtension = ".asset";

		public static string AssetBundlesPath = "Plugins/Roomful/Bundles";
		public static string AssetBundlesPathFull = "Assets/" + AssetBundlesPath;

		public string _SessionId = "";

		public List<AssetTemplate> LocalAssetTemplates = new List<AssetTemplate>();


		public const float MAX_AlLOWED_SIZE = 4f;
		public const float MIN_ALLOWED_SIZE = 0.3f;


		#if UNITY_EDITOR
		public List<BuildTarget> TargetPlatforms = new List<BuildTarget>();
		#endif

		private static AssetBundlesSettings _Instance = null;
		public static AssetBundlesSettings Instance {
			get {
				if (_Instance == null) {
					_Instance = Resources.Load(SettingsAssetName) as AssetBundlesSettings;

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

		public string SessionId {
			get {
				return _SessionId;
			}
		}

		public void SetSessionId(string id) {
			_SessionId = id;
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

