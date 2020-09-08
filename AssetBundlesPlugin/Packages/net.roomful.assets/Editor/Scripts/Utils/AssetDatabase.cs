using UnityEngine;
using net.roomful.assets.serialization;

namespace net.roomful.assets.Editor
{
	public class AssetDatabase {
		private readonly string m_pathRelativeAssetsFolder = "";
		private readonly string m_pathRelativeProjectFolder = "Assets/";
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
			var title = asset.GetTemplate().Title;

			ValidateBundleFolder (title);

			SaveFont (title, st.Font, st.FullFontName, st.FontFileContent);
		}

        public string SaveCubemapAsset(IAsset asset, SerializedEnvironment e) {
            
            var title = asset.GetTemplate().Title;
            ValidateBundleFolder(title);

            var fullPath = GetFullFilePath(typeof(Cubemap), title, e.ReflectionCubemapFileName);
            var path = GetShortFilePath(typeof(Cubemap), title, e.ReflectionCubemapFileName);

            if (!FolderUtils.IsFileExists(path)) {
                FolderUtils.WriteBytes(fullPath, e.ReflectionCubemapFileData);
            }

            return fullPath;
        }

        public void SaveAnimationClipByData(IAsset asset, SerializedAnimationClip serializedClip) {
            var title = asset.GetTemplate().Title;

            ValidateBundleFolder(title);
			
            SaveAnimationClip(title, serializedClip.AnimationClipName, serializedClip.ClipData);
        }

        public void SaveAnimatorController(IAsset asset,  SerializedAnimatorController serializedAnimator) {
            var title = asset.GetTemplate().Title;
            ValidateBundleFolder(title);
            SaveAnimator(title, serializedAnimator.ControllerName, serializedAnimator.SerializedData);
	        if (serializedAnimator.HasAvatar()) {
		        SaveAvatar(title, serializedAnimator.SerializedAvatar);
	        }
	    }
        
        public void SaveAsset<T>(IAsset asset, T unityAsset) where T: Object {
            var title = asset.GetTemplate().Title;

            ValidateBundleFolder (title);

			var t = typeof(T);

			if (t == typeof(Texture)) {
				SaveTexture (unityAsset as Texture, title);
			} else if (t == typeof(AnimationClip)) {
                SaveAnimationClip(unityAsset as AnimationClip, title);
            } else {
                SaveSimpleAsset(unityAsset, title);
            }
        }
		
		public T LoadAsset<T>(IAsset asset, string assetName) where T: Object {
			var fullName = assetName + GetExtensionByType (typeof(T));
			var fullPath = GetFullFilePath(typeof(T), asset.GetTemplate().Title, fullName);
			Debug.Log("fullPath: " + fullPath);
			return (T)UnityEditor.AssetDatabase.LoadAssetAtPath(fullPath, typeof(T));
		}

		public Font LoadFontAsset(IAsset asset, SerializedText st) {
			var fullPath = GetFullFilePath(typeof(Font), asset.GetTemplate().Title, st.FullFontName);

			return (Font)UnityEditor.AssetDatabase.LoadAssetAtPath(fullPath, typeof(Font));
		}

        public Cubemap LoadCubemapAsset(IAsset asset, SerializedEnvironment e) {
            var fullPath = GetFullFilePath(typeof(Cubemap), asset.GetTemplate().Title, e.ReflectionCubemapFileName);
            return (Cubemap)UnityEditor.AssetDatabase.LoadAssetAtPath(fullPath, typeof(Cubemap));
        }

        public bool IsAssetExist<T>(IAsset asset, T unityAsset) where T : Object {
            var propTitle = asset.GetTemplate().Title;
            var path = GetShortFilePath(unityAsset, propTitle);
            return FolderUtils.IsFileExists(path);
        }

        //--------------------------------------
        // Save
        //--------------------------------------

        private void SaveTexture(Texture tex, string assetTitle) {
	        ValidateBundleFolder (assetTitle);
			var fullPath = GetFullFilePath(tex, assetTitle);
			var path = GetShortFilePath(tex, assetTitle);

			if (!FolderUtils.IsFileExists(path)) {
				var tmp = RenderTexture.GetTemporary(tex.width, tex.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.Linear);
				Graphics.Blit(tex, tmp);
				var previous = RenderTexture.active;
				RenderTexture.active = tmp;

				var myTexture2D = new Texture2D(tex.width, tex.height);
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
            var fullPath = GetFullFilePath(typeof(Font), assetTitle, assetFullName);
            var path = GetShortFilePath(typeof(Font), assetTitle, assetFullName);

            if (!FolderUtils.IsFileExists(path)) {
                FolderUtils.WriteBytes(fullPath, assetData);
            }
        }
        
		private void SaveAvatar(string assetTitle, SerializedAvatar avatar) {
			ValidateBundleFolder (assetTitle);
			var fullPath = GetFullFilePath(typeof(Avatar), assetTitle, assetTitle, true);
			var path = GetShortFilePath(typeof(Avatar), assetTitle, assetTitle, true);
            
			if (!FolderUtils.IsFileExists(path)) {
				FolderUtils.WriteBytes(fullPath, avatar.AvatarData);
			}
		}
		
        private void SaveAnimator(string assetTitle, string animatorName, byte[] assetData) {
	        ValidateBundleFolder (assetTitle);
            var fullPath = GetFullFilePath(typeof(UnityEditor.Animations.AnimatorController), assetTitle, animatorName, true);
            var path = GetShortFilePath(typeof(UnityEditor.Animations.AnimatorController), assetTitle, animatorName, true);
            
            if (!FolderUtils.IsFileExists(path)) {
                FolderUtils.WriteBytes(fullPath, assetData);
            }
        }

        private void SaveAnimationClip(string assetTitle, string clipName, byte[] assetData) {
	        ValidateBundleFolder (assetTitle);
            var fullPath = GetFullFilePath(typeof(AnimationClip), assetTitle, clipName, true);
            var path = GetShortFilePath(typeof(AnimationClip), assetTitle, clipName, true);
            if (!FolderUtils.IsFileExists(path)) {
                FolderUtils.WriteBytes(fullPath, assetData);
            }
        }

        private void SaveAnimationClip(AnimationClip clip, string assetTitle) {
	        ValidateBundleFolder (assetTitle);
            var fullPath = GetFullFilePath(clip, assetTitle);
            var path = GetShortFilePath(clip, assetTitle);

            if (!FolderUtils.IsFileExists(path)) {
                var newClip = new AnimationClip();

                UnityEditor.EditorUtility.CopySerialized(clip, newClip);
                UnityEditor.AssetDatabase.CreateAsset(newClip, fullPath);
            }
        }

        private void SaveSimpleAsset<T>(T unityAsset, string assetTitle) where T : Object {
	        ValidateBundleFolder (assetTitle);
            var fullPath = GetFullFilePath(unityAsset, assetTitle);
            var path = GetShortFilePath(unityAsset, assetTitle);
	        
            if (!FolderUtils.IsFileExists(path)) {
                UnityEditor.AssetDatabase.CreateAsset(unityAsset, fullPath);
            }
        }


        //--------------------------------------
        // Folders And Files
        //--------------------------------------

        private void ValidateBundleFolder(string assetTitle) {
			var path = m_pathRelativeAssetsFolder + assetTitle;
			FolderUtils.CreateAssetComponentsFolder (path);
		}


        private string GetShortFilePath<T>(T unityAsset, string assetTitle) where T: Object {
			var folder = GetFolderByType (typeof(T));
			var assetName = unityAsset.name;
			var assetExtension = GetExtensionByType (typeof(T));

			return m_pathRelativeAssetsFolder + assetTitle + folder + assetName + assetExtension;
		}

		private string GetFullFilePath<T>(T unityAsset, string assetTitle) where T: Object {
			var folder = GetFolderByType (typeof(T));
			var assetName = unityAsset.name;
			var assetExtension = GetExtensionByType (typeof(T));

			return m_pathRelativeProjectFolder + assetTitle + folder + assetName + assetExtension;
		}
        
        private string GetShortFilePath(System.Type assetType, string assetTitle, string assetFullName, bool useExtension = false) {
			var folder = GetFolderByType (assetType);

            var path = m_pathRelativeAssetsFolder + assetTitle + folder + assetFullName;

            if (useExtension) {
                path += GetExtensionByType(assetType);
            }

            return path;
		}

		private string GetFullFilePath(System.Type assetType, string assetTitle, string assetFullName, bool useExtension = false) {
			var folder = GetFolderByType (assetType);

            var path = m_pathRelativeProjectFolder + assetTitle + folder + assetFullName;

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
