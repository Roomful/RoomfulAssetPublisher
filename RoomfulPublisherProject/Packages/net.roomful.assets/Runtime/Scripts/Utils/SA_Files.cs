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
using System.IO;

namespace SA.Common.Util
{
    internal static class Files
    {
        private static bool IsFileExists(string fileName) {
            if (fileName.Equals(string.Empty)) {
                return false;
            }

            return File.Exists(GetFullPath(fileName));
        }

        public static byte[] ReadBytes(string filePath) {
#if !UNITY_WEBPLAYER
            if (IsFileExists(filePath)) {
                return File.ReadAllBytes(GetFullPath(filePath));
            }
            else {
                return new byte[0];
            }
#endif

#if UNITY_WEBPLAYER
			Debug.LogWarning("FileStaticAPI::Read is innored under wep player platfrom");
r			eturn new byte[0];
#endif
        }

        private static bool IsFolderExists(string folderPath) {
            if (folderPath.Equals(string.Empty)) {
                return false;
            }

            return Directory.Exists(GetFullPath(folderPath));
        }

        private static string GetFullPath(string srcName) {
            if (srcName.Equals(string.Empty)) {
                return Application.dataPath;
            }

            if (srcName[0].Equals('/')) {
                srcName.Remove(0, 1);
            }

            return Application.dataPath + "/" + srcName;
        }
    }
}

#endif