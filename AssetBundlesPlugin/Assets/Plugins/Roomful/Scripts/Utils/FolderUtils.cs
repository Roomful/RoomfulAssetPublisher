using UnityEngine;
using System;
using System.IO;
using System.Threading;

#if UNITY_EDITOR
using UnityEditor;
#endif

namespace RF.AssetWizzard {
	public class FolderUtils  {
		//--------------------------------------
		// Config
		//--------------------------------------


		//--------------------------------------
		// Files
		//--------------------------------------
		#if UNITY_EDITOR

		public static void CreateAssetComponentsFolder(string folderPath) {

			CreateFolder (folderPath);
			CreateFolder (folderPath+"/Materials");
			CreateFolder (folderPath+"/Meshes");
			CreateFolder (folderPath+"/Textures");
            CreateFolder (folderPath + "/Fonts");
            CreateFolder (folderPath + "/Animations");
            CreateFolder (folderPath + "/Animations/Controller");
            CreateFolder (folderPath + "/Animations/Clips");
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

		public static void DeleteFolder(string folderPath, bool refresh = true) {
			if (IsFolderExists (folderPath)) {

				Directory.Delete(GetFullPath(folderPath), true);

				if (refresh) {
					AssetDatabase.Refresh();
				}
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

		public static bool IsFileExists(string fileName) {
			if (fileName.Equals (string.Empty)) {
				return false;
			}

			return File.Exists (GetFullPath (fileName));
		}

		public static string Read(string fileName) {
			if(IsFileExists(fileName)) {
				return File.ReadAllText(GetFullPath (fileName));
			} else {
				return "";
			}
		}

		public static void Write(string fileName, string contents) {
			CreateFolder (fileName.Substring (0, fileName.LastIndexOf ('/')));

			TextWriter tw = new StreamWriter(GetFullPath (fileName), false);
			tw.Write(contents);
			tw.Close(); 

			AssetDatabase.Refresh();
		}

		public static void WriteBytes(string fileName, byte[] data) {
			
			System.IO.FileStream _FileStream = new System.IO.FileStream(fileName, System.IO.FileMode.OpenOrCreate, System.IO.FileAccess.Write);
			
			_FileStream.Write(data, 0, data.Length);

			_FileStream.Close();

			AssetDatabase.Refresh();
		}
		#endif
	}
}
