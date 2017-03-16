using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;

namespace RF.AssetWizzard {

	[InitializeOnLoad]
	public class AssetBundlesSettings : ScriptableObject {
		
		public const string SERVICE_WEB_SERVER_SECRET_KEY = "560e00954b717b8f330bbffde48be57f4ac46e327e0d10958a40664cd74af89d"; // TODO:set right url

		public const string SERVICE_WEB_SERVER_URL = "http://demo.roomful.co:4000"; 

		public static string WEB_SERVER_URL = "https://demo.roomful.co:2443"; //test
		public static string SOCKET_SERVER_URL = "https://demo.roomful.co:2443/socket/"; //test

		public const int SOCKET_TIMEOUT_TIME = 50;
		public const int MAX_PANNELS_PER_ROOM = 20; 

		public const string SETTINGS_LOCATION = "Plugins/Roomful/Settings/Editor/Resources/";
		public const string PROPS_LOCATION = "Plugins/Roomful/Props/";

		private const string SettingsPath = "Plugins/Roomful/Resources";
		private const string SettingsAssetName = "AssetBundlesSettings";
		private const string SettingsAssetExtension = ".asset";

		//public List<PropInfo> AvaliableProps 	= new List<PropInfo>();

		private static AssetBundlesSettings _Instance = null;
		public static AssetBundlesSettings Instance {


			get {
				if (_Instance == null) {
					_Instance = Resources.Load(SettingsAssetName) as AssetBundlesSettings;

					if (_Instance == null) {

						_Instance = CreateInstance<AssetBundlesSettings>();


						EditorUtils.CreateFolder(SETTINGS_LOCATION);
						string fullPath = Path.Combine(Path.Combine("Assets", SETTINGS_LOCATION),
							SettingsAssetName + SettingsAssetExtension
						);

						AssetDatabase.CreateAsset(_Instance, fullPath);

					}

//					foreach (PropInfo prop in _Instance.AvaliableProps) {
//						prop.IsAvailable = pickedScenes.Contains(prop.ScenePath);
//					}
					
				}
				return _Instance;
			}
		}


		public static void Save() {
			
			EditorUtility.SetDirty(Instance);
			AssetDatabase.SaveAssets();


		}
	}
}

