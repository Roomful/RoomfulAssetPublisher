﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RF.AssetWizzard;
using RF.AssetBundles.Serialisation;

namespace RF.AssetBundles {
	public class PropDataBase : MonoBehaviour {
		#if UNITY_EDITOR
		public static void SaveFontAsset(PropAsset propAsset, SerializedText st) {
			string propTitle = propAsset.Template.Title;

			ValidateBundleFolder (propTitle);

			SaveFont (propTitle, st.Font, st.FullFontName, st.FontFileContent);
		}

		public static void SaveAsset<T>(PropAsset propAsset, T asset) where T: Object {
			string propTitle = propAsset.Template.Title;

			ValidateBundleFolder (propTitle);

			System.Type t = typeof(T);

			if (t == typeof(Texture)) {
				SaveTexture (asset as Texture, propTitle);
			}

			if (t == typeof(Material)) {
				SaveMaterial (asset as Material, propTitle);
			}

			if (t == typeof(Mesh)) {
				SaveMesh (asset as Mesh, propTitle);
			}
		}


		public static T LoadAsset<T>(PropAsset propAsset, string assetName) where T: Object {
			string fullName = assetName + GetExtensionByType (typeof(T));
			string fullPath = GetFullFilePath(typeof(T), propAsset.Template.Title, fullName);

			return (T)UnityEditor.AssetDatabase.LoadAssetAtPath(fullPath, typeof(T));
		}

		public static Font LoadFontAsset(PropAsset propAsset, SerializedText st) {
			string fullPath = GetFullFilePath(typeof(Font), propAsset.Template.Title, st.FullFontName);

			return (Font)UnityEditor.AssetDatabase.LoadAssetAtPath(fullPath, typeof(Font));
		}

		public static void ClearOldDataFolder(PropAsset propAsset) {
			string path = AssetBundlesSettings.AssetBundlesPath + "/" + propAsset.Template.Title;

			if (FolderUtils.IsFolderExists(path)) {
				FolderUtils.DeleteFolder (path);
			}
		}

		private static void SaveTexture(Texture tex, string propTitle) {
			string fullPath = GetFullFilePath(tex, propTitle);
			string path = GetShortFilePath(tex, propTitle);

			if (!FolderUtils.IsFileExists(path)) {
				RenderTexture tmp = RenderTexture.GetTemporary(tex.width, tex.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
				Graphics.Blit(tex, tmp);
				RenderTexture previous = RenderTexture.active;
				RenderTexture.active = tmp;

				Texture2D myTexture2D = new Texture2D(tex.width, tex.height);
				myTexture2D.name = tex.name;
				myTexture2D.ReadPixels(new Rect(0, 0, tmp.width, tmp.height), 0, 0);
				myTexture2D.Apply();

				RenderTexture.active = previous;
				RenderTexture.ReleaseTemporary(tmp);

				FolderUtils.WriteBytes (fullPath, myTexture2D.EncodeToPNG ());
			}
		}

		private static void SaveMaterial(Material mat, string propTitle) {
			string fullPath = GetFullFilePath(mat, propTitle);
			string path = GetShortFilePath(mat, propTitle);

			if (!FolderUtils.IsFileExists (path)) {
				UnityEditor.AssetDatabase.CreateAsset (mat, fullPath);
			}
		}

		private static void SaveMesh(Mesh mesh, string propTitle) {
			string fullPath = GetFullFilePath(mesh, propTitle);
			string path = GetShortFilePath(mesh, propTitle);

			if (!FolderUtils.IsFileExists (path)) {
				UnityEditor.AssetDatabase.CreateAsset(mesh, fullPath);
			}
		}

		private static void SaveFont(string propTitle, Font font, string assetFullName, byte[] assetData) {
			string fullPath = GetFullFilePath(typeof(Font), propTitle, assetFullName);
			string path = GetShortFilePath(typeof(Font), propTitle, assetFullName);

			if (!FolderUtils.IsFileExists (path)) {
				FolderUtils.WriteBytes (fullPath, assetData);

			}
		}

		private static void ValidateBundleFolder(string propTitle) {
			string path = AssetBundlesSettings.AssetBundlesPath + "/" + propTitle;

			if (!FolderUtils.IsFolderExists(path)) {
				FolderUtils.CreateAssetComponentsFolder (path);
			}
		}

		private static string GetShortFilePath<T>(T asset, string propTitle) where T: Object {
			string folder = GetFolderByType (typeof(T));
			string assetName = asset.name;
			string assetExtension = GetExtensionByType (typeof(T));

			return AssetBundlesSettings.AssetBundlesPath + "/" + propTitle + folder + assetName + assetExtension;
		}

		private static string GetFullFilePath<T>(T asset, string propTitle) where T: Object {
			string folder = GetFolderByType (typeof(T));
			string assetName = asset.name;
			string assetExtension = GetExtensionByType (typeof(T));

			return AssetBundlesSettings.AssetBundlesPathFull + "/" + propTitle + folder + assetName + assetExtension;
		}

		private static string GetShortFilePath(System.Type assetType, string propTitle, string assetFullName) {
			string folder = GetFolderByType (assetType);

			return AssetBundlesSettings.AssetBundlesPath + "/" + propTitle + folder + assetFullName;
		}

		private static string GetFullFilePath(System.Type assetType, string propTitle, string assetFullName) {
			string folder = GetFolderByType (assetType);

			return AssetBundlesSettings.AssetBundlesPathFull + "/" + propTitle + folder + assetFullName;
		}

		private const string m_TexturesExtension = ".png";
		private const string m_MeshesExtension = ".asset";
		private const string m_MaterialsExtension = ".mat";

		private const string m_FontsFolder = "/Fonts/";
		private const string m_TexturesFolder = "/Textures/";
		private const string m_MeshesFolder = "/Meshes/";
		private const string m_MaterialsFolder = "/Materials/";

		private static string GetExtensionByType(System.Type t) {
			if (t == typeof(Texture)) return m_TexturesExtension;
			if (t == typeof(Mesh)) return m_MeshesExtension;
			if (t == typeof(Material)) return m_MaterialsExtension;

			return string.Empty;
		}

		private static string GetFolderByType(System.Type t) {
			if (t == typeof(Texture)) return m_TexturesFolder;
			if (t == typeof(Font)) return m_FontsFolder;
			if (t == typeof(Mesh)) return m_MeshesFolder;
			if (t == typeof(Material)) return m_MaterialsFolder;

			return string.Empty;
		}

		#endif
	}
}