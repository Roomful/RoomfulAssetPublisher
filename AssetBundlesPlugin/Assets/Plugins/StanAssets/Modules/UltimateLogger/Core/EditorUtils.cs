////////////////////////////////////////////////////////////////////////////////
//  
// @module Ultimate Logger
// @author Osipov Stanislav (Stan's Assets) 
// @support support@stansassets.com
// @website https://stansassets.com
//
////////////////////////////////////////////////////////////////////////////////

using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
using System;
using System.IO;
using System.Threading;

namespace SA.UltimateLogger {

	public class EditorUtils  {


		//--------------------------------------
		// Config
		//--------------------------------------

		public const int VERSION_UNDEFINED = 0;
		public const string VERSION_UNDEFINED_STRING 	= "Undefined";
		public const string SUPPORT_EMAIL 				= "support@stansassets.com";
		public const string WEBSITE_ROOT_URL 			= "https://stansassets.com/";

		public const string BUNDLES_PATH             = "Plugins/StansAssets/Bundles/";
		public const string MODULS_PATH 	 		 = "Plugins/StansAssets/Modules/";
		public const string SUPPORT_MODULS_PATH 	 = "Plugins/StansAssets/Support/";


		public const string SETTINGS_REMOVE_PATH 	= SUPPORT_MODULS_PATH + "Settings/";
		public const string SETTINGS_PATH 			= SUPPORT_MODULS_PATH + "Settings/Resources/";


		public const string ANDROID_DESTANATION_PATH  = "Plugins/Android/";
		public const string ANDROID_SOURCE_PATH       = SUPPORT_MODULS_PATH + "NativeLibraries/Android/";


		public const string IOS_DESTANATION_PATH 	 = "Plugins/IOS/";
		public const string IOS_SOURCE_PATH       	 = SUPPORT_MODULS_PATH + "NativeLibraries/IOS/";



		//--------------------------------------
		// Fiels
		//--------------------------------------


		public static bool IsFolderExists(string folderPath) {
			if (folderPath.Equals (string.Empty)) {
				return false;
			}

			return Directory.Exists (GetFullPath(folderPath));
		}

		public static void CreateFolder(string folderPath) {
			if (!IsFolderExists (folderPath)) {
				Directory.CreateDirectory (GetFullPath (folderPath));

				AssetDatabase.Refresh();
			}
		}

		public static void DeleteFolder(string folderPath, bool refresh = true) {
			#if !UNITY_WEBPLAYER
			if (IsFolderExists (folderPath)) {

				Directory.Delete(GetFullPath(folderPath), true);

				if (refresh) {
					AssetDatabase.Refresh();
				}
			}
			#endif

			#if UNITY_WEBPLAYER
			Debug.LogWarning("FileStaticAPI::DeleteFolder is innored under wep player platfrom");
			#endif
		}


		private static string GetFullPath(string srcName) {
			if (srcName.Equals (string.Empty)) {
				return Application.dataPath;
			}

			if (srcName [0].Equals ('/')) {
				srcName.Remove(0, 1);
			}

			return Application.dataPath + "/" + srcName;
		}

		public static bool IsFileExists(string fileName) {
			if (fileName.Equals (string.Empty)) {
				return false;
			}

			return File.Exists (GetFullPath (fileName));
		}

		public static string Read(string fileName) {
		#if !UNITY_WEBPLAYER
				if(IsFileExists(fileName)) {
					return File.ReadAllText(GetFullPath (fileName));
				} else {
					return "";
				}
		#endif

		#if UNITY_WEBPLAYER
		Debug.LogWarning("FileStaticAPI::Read is innored under wep player platfrom");
		return "";
		#endif
		}


		public static void Write(string fileName, string contents) {



			CreateFolder (fileName.Substring (0, fileName.LastIndexOf ('/')));

			TextWriter tw = new StreamWriter(GetFullPath (fileName), false);
			tw.Write(contents);
			tw.Close(); 

			AssetDatabase.Refresh();
		}

	}

}


#endif