////////////////////////////////////////////////////////////////////////////////
//
// @module Assets Common Lib
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

namespace SA.Common.Util {

	public static class Files {


		public static bool IsFileExists(string fileName) {
			if (fileName.Equals (string.Empty)) {
				return false;
			}

			return File.Exists (GetFullPath (fileName));
		}

		public static void Write(string fileName, string contents) {



			CreateFolder (fileName.Substring (0, fileName.LastIndexOf ('/')));

			TextWriter tw = new StreamWriter(GetFullPath (fileName), false);
			tw.Write(contents);
			tw.Close();

			AssetDatabase.Refresh();

			//File.WriteAllText(GetFullPath (fileName), contents);
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
U.LogWarning("FileStaticAPI::Read is innored under wep player platfrom");
return "";
#endif
		}

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

		private static string GetFullPath(string srcName) {
			if (srcName.Equals (string.Empty)) {
				return Application.dataPath;
			}

			if (srcName [0].Equals ('/')) {
				srcName.Remove(0, 1);
			}

			return Application.dataPath + "/" + srcName;
		}
	}

}

#endif
