using UnityEngine;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
#if UNITY_EDITOR
using UnityEditor;
#endif

namespace net.roomful.assets {
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
			CreateFolder (folderPath + "/Materials");
            CreateFolder(folderPath +  "/Cubemaps");
            CreateFolder (folderPath + "/Meshes");
			CreateFolder (folderPath + "/Textures");
            CreateFolder (folderPath + "/Fonts");
            CreateFolder (folderPath + "/Animations");
            CreateFolder (folderPath + "/Animations/Controller");
            CreateFolder (folderPath + "/Animations/Clips");
            CreateFolder (folderPath + "/Animations/Avatars");
        }

		public static List<string> GetSubfolders(string folderPath) {
			var result = new List<string>();
			if (IsFolderExists(folderPath)) {
				result.AddRange(Directory.GetDirectories(GetFullPath(folderPath)).ToList());
			}

			return result;
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

                var path = GetFullPath(folderPath);

                Directory.Delete(GetFullPath(folderPath), true);
                DeleteFile(path + ".meta", false);


                if (refresh) { AssetDatabase.Refresh(); }
            }
		}

        public static void DeleteFile(string filePath, bool refresh = true) {
            if(IsFileExists(filePath)) {

                File.Delete(filePath);

                if (refresh) { AssetDatabase.Refresh(); }
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
			try {
				var fileStream = new  FileStream(fileName, FileMode.OpenOrCreate, FileAccess.Write);
				fileStream.Write(data, 0, data.Length);
				fileStream.Close();
				AssetDatabase.Refresh();
			}
			catch (ArgumentException e) {
				Debug.LogError(e); 
			}

		}
		#endif
	}
}
