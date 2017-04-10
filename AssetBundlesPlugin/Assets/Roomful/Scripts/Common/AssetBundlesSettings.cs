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
		
		//public const string SERVICE_WEB_SERVER_SECRET_KEY = "560e00954b717b8f330bbffde48be57f4ac46e327e0d10958a40664cd74af89d"; // TODO:set right url

		//public const string SERVICE_WEB_SERVER_URL = "http://demo.roomful.co:4000"; 

		public static string WEB_SERVER_URL = "https://demo.roomful.co:4443"; //test
		//public static string SOCKET_SERVER_URL = "https://demo.roomful.co:2443/socket/"; //test

		//public const int SOCKET_TIMEOUT_TIME = 50;
		//public const int MAX_PANNELS_PER_ROOM = 20; 

		public const string SETTINGS_LOCATION = "Roomful/Settings/Editor/Resources/";
		public const string PROPS_ASSETS_LOCATION = "Assets/Roomful/PropAssets/";

		private const string SettingsPath = "Roomful/Resources";
		private const string SettingsAssetName = "AssetBundlesSettings";
		private const string SettingsAssetExtension = ".asset";
		public static string AssetBundlesPath = "Assets/Roomful/AssetBundles";
		public static string AssetBundlesWorshopScene = "Assets/Roomful/Scenes/AssetWorkshop.unity";
		public static string AssetBundlesWorshopSceneName = "AssetWorkshop";

		public string _SessionId = "";

		public List<AssetTemplate> LocalAssetTemplates = new List<AssetTemplate>();

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

