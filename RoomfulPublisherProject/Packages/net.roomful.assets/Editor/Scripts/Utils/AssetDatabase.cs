using System;
using System.IO;
using UnityEngine;
using net.roomful.assets.serialization;
using UnityEditor;
using Object = UnityEngine.Object;

namespace net.roomful.assets.editor
{
	internal class AssetDatabase {
		private readonly string m_pathRelativeAssetsFolder = "";
		private readonly string m_pathRelativeProjectFolder = "Assets/";
		public static bool s_uniqueMeshesFound;
        //--------------------------------------
        // Public Methods
        //--------------------------------------
		public AssetDatabase(string basePath) {
			if (!string.IsNullOrEmpty(basePath)) {
				m_pathRelativeAssetsFolder += basePath;
				m_pathRelativeProjectFolder += basePath;
			}
		}

		public void SaveFontAsset(IAssetBundle asset, SerializedText st) {
			var title = asset.Title;

			ValidateBundleFolder (title);

			SaveFont (title, st.Font, st.FullFontName, st.FontFileContent);
		}

        public string SaveCubemapAsset(IAssetBundle asset, SerializedEnvironment e) {

            var title = asset.Title;
            ValidateBundleFolder(title);

            var fullPath = GetFullFilePath(typeof(Cubemap), title, e.ReflectionCubemapFileName);
            var path = GetShortFilePath(typeof(Cubemap), title, e.ReflectionCubemapFileName);

            if (!FolderUtils.IsFileExists(path)) {
                FolderUtils.WriteBytes(fullPath, e.ReflectionCubemapFileData);
            }

            return fullPath;
        }

        public void SaveAnimationClipByData(IAssetBundle asset, SerializedAnimationClip serializedClip) {
            var title = asset.Title;

            ValidateBundleFolder(title);

            SaveAnimationClip(title, serializedClip.AnimationClipName, serializedClip.ClipData);
        }

        public void SaveAnimatorController(IAssetBundle asset,  SerializedAnimatorController serializedAnimator) {
            var title = asset.Title;
            ValidateBundleFolder(title);
            SaveAnimator(title, serializedAnimator.ControllerName, serializedAnimator.SerializedData);
	        if (serializedAnimator.HasAvatar()) {
		        SaveAvatar(title, serializedAnimator);
	        }
	    }

        public void SaveAsset<T>(IAssetBundle asset, T unityAsset) where T: Object {
            var title = asset.Title;

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

		public T LoadAsset<T>(IAssetBundle asset, string assetName) where T: Object {
			var fullName = assetName + GetExtensionByType (typeof(T));
			var fullPath = GetFullFilePath(typeof(T), asset.Title, fullName);
			return (T)UnityEditor.AssetDatabase.LoadAssetAtPath(fullPath, typeof(T));
		}

		public Font LoadFontAsset(IAssetBundle asset, SerializedText st) {
			var fullPath = GetFullFilePath(typeof(Font), asset.Title, st.FullFontName);

			return (Font)UnityEditor.AssetDatabase.LoadAssetAtPath(fullPath, typeof(Font));
		}

        public Cubemap LoadCubemapAsset(IAssetBundle asset, SerializedEnvironment e) {
            var fullPath = GetFullFilePath(typeof(Cubemap), asset.Title, e.ReflectionCubemapFileName);
            return (Cubemap)UnityEditor.AssetDatabase.LoadAssetAtPath(fullPath, typeof(Cubemap));
        }

        public bool IsAssetExist<T>(IAssetBundle asset, T unityAsset) where T : Object {
            var propTitle = asset.Title;
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

			var tmp = RenderTexture.GetTemporary(tex.width, tex.height, 0, RenderTextureFormat.Default, RenderTextureReadWrite.sRGB);
			Graphics.Blit(tex, tmp);
			var previous = RenderTexture.active;
			RenderTexture.active = tmp;

			var myTexture2D = new Texture2D(tex.width, tex.height);
			myTexture2D.name = tex.name;
			myTexture2D.ReadPixels(new Rect(0, 0, tmp.width, tmp.height), 0, 0);
			myTexture2D.Apply();

			RenderTexture.active = previous;
			RenderTexture.ReleaseTemporary(tmp);

			if (!FolderUtils.IsFileExists(path)) {
				FolderUtils.WriteBytes(fullPath, myTexture2D.EncodeToPNG());
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

		private void SaveAvatar(string assetTitle, SerializedAnimatorController sac) {
			ValidateBundleFolder (assetTitle);
			var fullPath = GetFullFilePath(typeof(Avatar), assetTitle, assetTitle, true);
			var path = GetShortFilePath(typeof(Avatar), assetTitle, assetTitle, true);

			if (!FolderUtils.IsFileExists(path)) {
				var avatarCopy = ScriptableObject.Instantiate(sac.GetComponent<Animator>().avatar);
				UnityEditor.AssetDatabase.CreateAsset(avatarCopy, fullPath);
			}
		}

        private void SaveAnimator(string assetTitle, string animatorName, byte[] assetData) {
	        ValidateBundleFolder (assetTitle);
            var fullPath = GetFullFilePath(typeof(UnityEditor.Animations.AnimatorController), assetTitle, animatorName, true);
            var path = GetShortFilePath(typeof(UnityEditor.Animations.AnimatorController), assetTitle, animatorName, true);

            if (!FolderUtils.IsFileExists(path)) {
                FolderUtils.WriteBytes(fullPath, assetData);
                
                // Mega stupid hack to make Animator work after a couple of reuploads during one Unity session
                var text = File.ReadAllText(fullPath);
                text += "\n";
                File.WriteAllText(fullPath, text);
                // See description here https://github.com/Roomful/RoomfulUnity1/issues/5512
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

            var newClip = new AnimationClip();
            UnityEditor.EditorUtility.CopySerialized(clip, newClip);
            if (!FolderUtils.IsFileExists(path)) {
                UnityEditor.AssetDatabase.CreateAsset(newClip, fullPath);
            }
        }

        private void SaveSimpleAsset<T>(T unityAsset, string assetTitle) where T : Object {
	        ValidateBundleFolder (assetTitle);
            var fullPath = GetFullFilePath(unityAsset, assetTitle);
            var path = GetShortFilePath(unityAsset, assetTitle);

            if (!FolderUtils.IsFileExists(path))
	            UnityEditor.AssetDatabase.CreateAsset(unityAsset, fullPath);
            else if (unityAsset is Mesh)
	            CheckIfMeshIsUniqueAndSave(unityAsset, assetTitle, fullPath);
        }


        //--------------------------------------
        // Folders And Files
        //--------------------------------------

        private void ValidateBundleFolder(string assetTitle) {
			var path = m_pathRelativeAssetsFolder + assetTitle;
			FolderUtils.CreateAssetComponentsFolder (path);
		}
        
        private void CheckIfMeshIsUniqueAndSave<T>(T unityAsset, string assetTitle, string pathToCompare) where T : Object {
	        var mesh = unityAsset as Mesh;
	        var tempAsset = new Mesh();
	        foreach(var property in typeof(Mesh).GetProperties()) {
		        if (property.GetSetMethod() != null && property.GetGetMethod() != null)
			        property.SetValue(tempAsset, property.GetValue(mesh, null), null);
	        }
	        
	        Directory.CreateDirectory("Assets/Plugins/Roomful/Editor/Bundles/" + assetTitle + "/TempMeshes/");
	        var tempMeshPath = "Assets/Plugins/Roomful/Editor/Bundles/" + assetTitle + "/TempMeshes/" + tempAsset.name.Trim() + GetExtensionByType (typeof(T));
	        UnityEditor.AssetDatabase.CreateAsset(tempAsset, tempMeshPath);

	        var files = Directory.GetFiles("Assets/Plugins/Roomful/Editor/Bundles/" + assetTitle + "/Meshes/");

	        var isUnique = false;
	        foreach (var file in files) {
		        var localMesh = UnityEditor.AssetDatabase.LoadAssetAtPath<Mesh>(file);
		        if (localMesh != null && tempAsset.name == localMesh.name && tempAsset.bounds != localMesh.bounds) {
			        isUnique = true;
			        break;
		        }
	        }
	        
	        if (isUnique) {
				Debug.Log("Unique mesh --> " + unityAsset.name + " <-- with the same name is found. Saving with unique name... ");
				var i = 0;
				var path = GetShortFilePath(unityAsset.GetType(), assetTitle, unityAsset.name.Trim() + "_" + i, true);
				var fullPath = GetFullFilePath(unityAsset.GetType(), assetTitle, unityAsset.name.Trim() + "_" + i, true);
				while (FolderUtils.IsFileExists(path)) {
					i++;
					path = GetShortFilePath(unityAsset.GetType(), assetTitle, unityAsset.name.Trim() + "_" + i, true);
					fullPath = GetFullFilePath(unityAsset.GetType(), assetTitle, unityAsset.name.Trim() + "_" + i, true);
				}

				s_uniqueMeshesFound = true;
				UnityEditor.AssetDatabase.CreateAsset(unityAsset, fullPath);
	        }
        }

        private string GetShortFilePath<T>(T unityAsset, string assetTitle) where T: Object {
			var folder = GetFolderByType (typeof(T));
			var assetName = unityAsset.name;
			var assetExtension = GetExtensionByType (typeof(T));

			return m_pathRelativeAssetsFolder + assetTitle + folder + assetName.Trim() + assetExtension;
		}

		private string GetFullFilePath<T>(T unityAsset, string assetTitle) where T: Object {
			var folder = GetFolderByType (typeof(T));
			var assetName = unityAsset.name;
			var assetExtension = GetExtensionByType (typeof(T));

			return m_pathRelativeProjectFolder + assetTitle + folder + assetName.Trim() + assetExtension;
		}

        private string GetShortFilePath(System.Type assetType, string assetTitle, string assetFullName, bool useExtension = false) {
			var folder = GetFolderByType (assetType);

            var path = m_pathRelativeAssetsFolder + assetTitle + folder + assetFullName.Trim();

            if (useExtension) {
                path += GetExtensionByType(assetType);
            }

            return path;
		}

		private string GetFullFilePath(System.Type assetType, string assetTitle, string assetFullName, bool useExtension = false) {
			var folder = GetFolderByType (assetType);

            var path = m_pathRelativeProjectFolder + assetTitle + folder + assetFullName.Trim();

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
		private const string m_AvatarExtension = ".asset";
        private const string m_PhysicMaterialExtension = ".physicMaterial";


        private const string m_FontsFolder = "/Fonts/";
		private const string m_TexturesFolder = "/Textures/";
        private const string m_CubemapFolder = "/Cubemaps/";
        private const string m_MeshesFolder = "/Meshes/";
		private const string m_MaterialsFolder = "/Materials/";
		private const string m_AnimatorFolder = "/Animations/Controller/";
		private const string m_AnimationsFolder = "/Animations/Clips/";
		private const string m_AvatarsFolder = "/Animations/Avatars/";
        private const string m_PhysicMaterialsFolder = "/PhysicMaterials/";


        private static string GetExtensionByType(System.Type t) {
			if (t == typeof(Texture)) return m_TexturesExtension;
			if (t == typeof(Mesh)) return m_MeshesExtension;
			if (t == typeof(Material)) return m_MaterialsExtension;
			if (t == typeof(UnityEditor.Animations.AnimatorController)) return m_AnimatorExtension;
			if (t == typeof(AnimationClip)) return m_AnimationExtension;
            if (t == typeof(Cubemap)) return m_CubemapExtension;
            if (t == typeof(Avatar)) return m_AvatarExtension;
            if (t == typeof(PhysicMaterial)) return m_PhysicMaterialExtension;

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
            if (t == typeof(PhysicMaterial)) return m_PhysicMaterialsFolder;

            return string.Empty;
		}
	}
}
