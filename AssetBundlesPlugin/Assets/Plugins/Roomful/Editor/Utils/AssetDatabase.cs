using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RF.AssetBundles.Serialization;

namespace RF.AssetWizzard.Editor
{
	public class AssetDatabase {
		private string m_pathRelativeAssetsFolder = "";
		private string m_pathRelativeProjectFolder = "Assets/";
        //--------------------------------------
        // Public Methods
        //--------------------------------------
		public AssetDatabase(string basePath) {
			if (!string.IsNullOrEmpty(basePath)) {
				m_pathRelativeAssetsFolder += basePath;
				m_pathRelativeProjectFolder += basePath; 
			}
		}

		public void SaveFontAsset(IAsset asset, SerializedText st) {
			string title = asset.GetTemplate().Title;

			ValidateBundleFolder (title);

			SaveFont (title, st.Font, st.FullFontName, st.FontFileContent);
		}

        public string SaveCubemapAsset(IAsset asset, SerializedEnviromnent e) {
            
            string title = asset.GetTemplate().Title;
            ValidateBundleFolder(title);

            string fullPath = GetFullFilePath(typeof(Cubemap), title, e.ReflectionCubemapFileName);
            string path = GetShortFilePath(typeof(Cubemap), title, e.ReflectionCubemapFileName);

            if (!FolderUtils.IsFileExists(path)) {
                FolderUtils.WriteBytes(fullPath, e.ReflectionCubemapFileData);
            }

            return fullPath;
        }

        public void SaveAnimationClipByData(IAsset asset, SerializedAnimationClip serializedClip) {
            string title = asset.GetTemplate().Title;

            ValidateBundleFolder(title);
			
            SaveAnimationClip(title, serializedClip.AnimationClipName, serializedClip.ClipData);
        }

        public void SaveAnimatorController(IAsset asset,  SerializedAnimatorController serializedAnimator) {
            string title = asset.GetTemplate().Title;
            ValidateBundleFolder(title);
            SaveAnimator(title, serializedAnimator.ControllerName, serializedAnimator.SerializedData);
	        if (serializedAnimator.HasAvatar()) {
		        SaveAvatar(title, serializedAnimator.SerializedAvatar);
	        }
	    }
        
        public void SaveAsset<T>(IAsset asset, T unityAsset) where T: Object {
            string title = asset.GetTemplate().Title;

            ValidateBundleFolder (title);

			System.Type t = typeof(T);

			if (t == typeof(Texture)) {
				SaveTexture (unityAsset as Texture, title);
			} else if (t == typeof(AnimationClip)) {
                SaveAnimationClip(unityAsset as AnimationClip, title);
            } else {
                SaveSimpleAsset(unityAsset, title);
            }
        }
		
		public T LoadAsset<T>(IAsset asset, string assetName) where T: Object {
			string fullName = assetName + GetExtensionByType (typeof(T));
			string fullPath = GetFullFilePath(typeof(T), asset.GetTemplate().Title, fullName);
			var t = UnityEditor.AssetDatabase.LoadAssetAtPath<T>(fullPath);
			return (T)UnityEditor.AssetDatabase.LoadAssetAtPath(fullPath, typeof(T));
		}

		public Font LoadFontAsset(IAsset asset, SerializedText st) {
			string fullPath = GetFullFilePath(typeof(Font), asset.GetTemplate().Title, st.FullFontName);

			return (Font)UnityEditor.AssetDatabase.LoadAssetAtPath(fullPath, typeof(Font));
		}

        public Cubemap LoadCubemapAsset(IAsset asset, SerializedEnviromnent e) {
            string fullPath = GetFullFilePath(typeof(Cubemap), asset.GetTemplate().Title, e.ReflectionCubemapFileName);

            return (Cubemap)UnityEditor.AssetDatabase.LoadAssetAtPath(fullPath, typeof(Cubemap));
        }

        public bool IsAssetExist<T>(IAsset asset, T unityAsset) where T : Object {
            string propTitle = asset.GetTemplate().Title;

            string path = GetShortFilePath(unityAsset, propTitle);

            return FolderUtils.IsFileExists(path);
        }

        //--------------------------------------
        // Save
        //--------------------------------------

        private void SaveTexture(Texture tex, string assetTitle) {
	        ValidateBundleFolder (assetTitle);
			string fullPath = GetFullFilePath(tex, assetTitle);
			string path = GetShortFilePath(tex, assetTitle);

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

        private void SaveFont(string assetTitle, Font font, string assetFullName, byte[] assetData) {
	        ValidateBundleFolder (assetTitle);
            string fullPath = GetFullFilePath(typeof(Font), assetTitle, assetFullName);
            string path = GetShortFilePath(typeof(Font), assetTitle, assetFullName);

            if (!FolderUtils.IsFileExists(path)) {
                FolderUtils.WriteBytes(fullPath, assetData);
            }
        }
        
		private void SaveAvatar(string assetTitle, SerializedAvatar avatar) {
			ValidateBundleFolder (assetTitle);
			string fullPath = GetFullFilePath(typeof(UnityEngine.Avatar), assetTitle, assetTitle, true);
			string path = GetShortFilePath(typeof(UnityEngine.Avatar), assetTitle, assetTitle, true);
            
			if (!FolderUtils.IsFileExists(path)) {
				FolderUtils.WriteBytes(fullPath, avatar.AvatarData);
			}
		}
		
        private void SaveAnimator(string assetTitle, string animatorName, byte[] assetData) {
	        ValidateBundleFolder (assetTitle);
            string fullPath = GetFullFilePath(typeof(UnityEditor.Animations.AnimatorController), assetTitle, animatorName, true);
            string path = GetShortFilePath(typeof(UnityEditor.Animations.AnimatorController), assetTitle, animatorName, true);
            
            if (!FolderUtils.IsFileExists(path)) {
                FolderUtils.WriteBytes(fullPath, assetData);
            }
        }

        private void SaveAnimationClip(string assetTitle, string clipName, byte[] assetData) {
	        ValidateBundleFolder (assetTitle);
            string fullPath = GetFullFilePath(typeof(AnimationClip), assetTitle, clipName, true);
            string path = GetShortFilePath(typeof(AnimationClip), assetTitle, clipName, true);

            if (!FolderUtils.IsFileExists(path)) {
                FolderUtils.WriteBytes(fullPath, assetData);
            }
        }

        private void SaveAnimationClip(AnimationClip clip, string assetTitle) {
	        ValidateBundleFolder (assetTitle);
            string fullPath = GetFullFilePath(clip, assetTitle);
            string path = GetShortFilePath(clip, assetTitle);

            if (!FolderUtils.IsFileExists(path)) {
                AnimationClip newClip = new AnimationClip();

                UnityEditor.EditorUtility.CopySerialized(clip, newClip);
                UnityEditor.AssetDatabase.CreateAsset(newClip, fullPath);
            }
        }

        private void SaveSimpleAsset<T>(T unityAsset, string assetTitle) where T : Object {
	        ValidateBundleFolder (assetTitle);
            string fullPath = GetFullFilePath(unityAsset, assetTitle);
            string path = GetShortFilePath(unityAsset, assetTitle);
	        
            if (!FolderUtils.IsFileExists(path)) {
                UnityEditor.AssetDatabase.CreateAsset(unityAsset, fullPath);
            }
        }


        //--------------------------------------
        // Folders And Files
        //--------------------------------------

        private void ValidateBundleFolder(string assetTitle) {
			string path = m_pathRelativeAssetsFolder + "/" + assetTitle;
			FolderUtils.CreateAssetComponentsFolder (path);
		}


        private string GetShortFilePath<T>(T unityAsset, string assetTitle) where T: Object {
			string folder = GetFolderByType (typeof(T));
			string assetName = unityAsset.name;
			string assetExtension = GetExtensionByType (typeof(T));

			return m_pathRelativeAssetsFolder + "/" + assetTitle + folder + assetName + assetExtension;
		}

		private string GetFullFilePath<T>(T unityAsset, string assetTitle) where T: Object {
			string folder = GetFolderByType (typeof(T));
			string assetName = unityAsset.name;
			string assetExtension = GetExtensionByType (typeof(T));

			return m_pathRelativeProjectFolder + "/" + assetTitle + folder + assetName + assetExtension;
		}
        
        private string GetShortFilePath(System.Type assetType, string assetTitle, string assetFullName, bool useExtension = false) {
			string folder = GetFolderByType (assetType);

            string path = m_pathRelativeAssetsFolder + "/" + assetTitle + folder + assetFullName;

            if (useExtension) {
                path += GetExtensionByType(assetType);
            }

            return path;
		}

		private string GetFullFilePath(System.Type assetType, string assetTitle, string assetFullName, bool useExtension = false) {
			string folder = GetFolderByType (assetType);

            string path = m_pathRelativeProjectFolder + "/" + assetTitle + folder + assetFullName;

            if (useExtension) {
                path += GetExtensionByType(assetType);
            }

            return path;
        }
        
        private const string m_TexturesExtension = ".png";
        private const string m_CubemapExtension = ".exr";
        private const string m_MeshesExtension = ".asset";
		private const string m_MaterialsExtension = ".mat";
		private const string m_AnimatorExtension = ".controller";
		private const string m_AnimationExtension = ".anim";
		private const string m_AvatarExtension = ".fbx";


        private const string m_FontsFolder = "/Fonts/";
		private const string m_TexturesFolder = "/Textures/";
        private const string m_CubemapFolder = "/Cubemaps/";
        private const string m_MeshesFolder = "/Meshes/";
		private const string m_MaterialsFolder = "/Materials/";
		private const string m_AnimatorFolder = "/Animations/Controller/";
		private const string m_AnimationsFolder = "/Animations/Clips/";
		private const string m_AvatarsFolder = "/Animations/Avatars/";


        private static string GetExtensionByType(System.Type t) {
			if (t == typeof(Texture)) return m_TexturesExtension;
			if (t == typeof(Mesh)) return m_MeshesExtension;
			if (t == typeof(Material)) return m_MaterialsExtension;
			if (t == typeof(UnityEditor.Animations.AnimatorController)) return m_AnimatorExtension;
			if (t == typeof(AnimationClip)) return m_AnimationExtension;
            if (t == typeof(Cubemap)) return m_CubemapExtension;
            if (t == typeof(Avatar)) return m_AvatarExtension;

            return string.Empty;
		}

		private static string GetFolderByType(System.Type t) {
			if (t == typeof(Texture)) return m_TexturesFolder;
			if (t == typeof(Font)) return m_FontsFolder;
			if (t == typeof(Mesh)) return m_MeshesFolder;
			if (t == typeof(Material)) return m_MaterialsFolder;
            if (t == typeof(UnityEditor.Animations.AnimatorController)) return m_AnimatorFolder;
            if (t == typeof(AnimationClip)) return m_AnimationsFolder;
            if (t == typeof(Cubemap)) return m_CubemapFolder;
            if (t == typeof(Avatar)) return m_AvatarsFolder;

            return string.Empty;
		}
	}
}
