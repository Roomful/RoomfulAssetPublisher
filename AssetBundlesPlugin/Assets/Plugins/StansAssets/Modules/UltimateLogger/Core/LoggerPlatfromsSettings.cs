////////////////////////////////////////////////////////////////////////////////
//  
// @module Ultimate Logger
// @author Osipov Stanislav (Stan's Assets) 
// @support support@stansassets.com
// @website https://stansassets.com
//
////////////////////////////////////////////////////////////////////////////////

using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


#if UNITY_EDITOR
using UnityEditor;
#endif

namespace SA.UltimateLogger {

	public class LoggerPlatfromsSettings : ScriptableObject {


		//--------------------------------------
		// Const
		//--------------------------------------

		public const string SETTINGS_LOCATION = "Plugins/StansAssets/Support/Settings/Resources/";


		//--------------------------------------
		// Settings
		//--------------------------------------


		public bool iOS_LogsRecord = true;
		public bool iOS_OverrideLogsOutput = true;


		public bool Android_LogsRecord = true;
		public bool Android_OverrideLogsOutput = true;


		//--------------------------------------
		// Initialisations
		//--------------------------------------


		private const string UL_SettingsAssetName = "LoggerPlatfromsSettings";
		private const string UL_SettingsAssetExtension = ".asset";

		private static LoggerPlatfromsSettings instance = null;
		public static LoggerPlatfromsSettings Instance {

			get {
				if (instance == null) {
					instance = Resources.Load(UL_SettingsAssetName) as LoggerPlatfromsSettings;

					if (instance == null) {

						// If not found, autocreate the asset object.
						instance = CreateInstance<LoggerPlatfromsSettings>();
						#if UNITY_EDITOR


						SA.UltimateLogger.EditorUtils.CreateFolder(SETTINGS_LOCATION);
						string fullPath = Path.Combine(Path.Combine("Assets", SETTINGS_LOCATION),
							UL_SettingsAssetName + UL_SettingsAssetExtension
						);

						AssetDatabase.CreateAsset(instance, fullPath);

						#endif
					}
				}
				return instance;
			}
		}



		public void Save() {
			#if UNITY_EDITOR
			EditorUtility.SetDirty(Instance);
			#endif
		}

	}

}