using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace RF.AssetWizzard.Editor {
	public static class AssetBundleContentCloner  {

		private static PropAsset clonedProp = null;

		public static void Clone(PropAsset prop) {
			clonedProp = prop;

			ValidateBundleFolder ();

			foreach (MeshRenderer mr in prop.GetComponentsInChildren<MeshRenderer>(true)) {
				List<Material> propMaterials = new List<Material> ();

				foreach (Material mat in mr.sharedMaterials) {
					Material newMat = RecreateMaterial (mat);

					if (mat.mainTexture == null) {
						Debug.Log (mr.gameObject.name, mr.gameObject);
					} else {
						newMat.mainTexture = RecreateTexture (mat.mainTexture);
					}

					propMaterials.Add (newMat);
				}

				mr.sharedMaterials = propMaterials.ToArray();

				Mesh newMesh = RecreateMesh (mr.GetComponent<MeshFilter>().sharedMesh);
				mr.GetComponent<MeshFilter> ().sharedMesh = newMesh;
			}

			clonedProp = null;
		}

		private static Material RecreateMaterial(Material mat) {
			string cleanedMatName = mat.name.Replace("/", "");

			string fullPath = AssetBundlesSettings.AssetBundlesPathFull + "/" + clonedProp.Template.Title + "/Materials/" + cleanedMatName + ".mat";
			string path = AssetBundlesSettings.AssetBundlesPath + "/" + clonedProp.Template.Title + "/Materials/" + cleanedMatName + ".mat";

			if (!FolderUtils.IsFileExists(path)) {
				Material newMat = new Material (mat);

				SaveMaterialToFolder (newMat, fullPath);
			}

			return LoadMaterialFromFolder(fullPath);
		}

		private static Texture2D RecreateTexture(Texture tex) {
			string fullPath = AssetBundlesSettings.AssetBundlesPathFull + "/" + clonedProp.Template.Title + "/Textures/" + tex.name + ".png";
			string path = AssetBundlesSettings.AssetBundlesPath + "/" + clonedProp.Template.Title + "/Textures/" + tex.name + ".png";

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

				SaveTextureToFolder (myTexture2D, fullPath);
			}

			return LoadTextureFromFolder(fullPath);
		}

		private static Mesh RecreateMesh(Mesh mesh) {
			string fullPath = AssetBundlesSettings.AssetBundlesPathFull + "/" + clonedProp.Template.Title + "/Meshes/" + mesh.name + ".asset";
			string path = AssetBundlesSettings.AssetBundlesPath + "/" + clonedProp.Template.Title + "/Meshes/" + mesh.name + ".asset";

			if (!FolderUtils.IsFileExists (path)) {
				Mesh newmesh = new Mesh();
				newmesh.vertices = mesh.vertices;
				newmesh.triangles = mesh.triangles;
				newmesh.uv = mesh.uv;
				newmesh.normals = mesh.normals;
				newmesh.colors = mesh.colors;
				newmesh.tangents = mesh.tangents;

				SaveMeshToFolder (newmesh, fullPath);
			}

			return LoadMeshFromFolder(fullPath);
		}

		private static void ValidateBundleFolder() {
			string path = AssetBundlesSettings.AssetBundlesPath + "/" + clonedProp.Template.Title;

			if (FolderUtils.IsFolderExists(path)) {
				FolderUtils.DeleteFolder (path);
			}

			FolderUtils.CreateAssetComponentsFolder (path);
		}

		private static void SaveMaterialToFolder(Material mat, string folderPath) {
			AssetDatabase.CreateAsset(mat, folderPath);
		}

		private static Material LoadMaterialFromFolder(string folderPath) {
			return (Material)AssetDatabase.LoadAssetAtPath(folderPath, typeof(Material));
		}

		private static void SaveTextureToFolder(Texture2D tex, string folderPath) {
			FolderUtils.WriteBytes (folderPath, tex.EncodeToPNG ());
		}

		private static Texture2D LoadTextureFromFolder(string folderPath) {
			return (Texture2D)AssetDatabase.LoadAssetAtPath(folderPath, typeof(Texture2D));
		}

		private static void SaveMeshToFolder(Mesh mesh, string folderPath) {
			AssetDatabase.CreateAsset(mesh, folderPath);
		}

		private static Mesh LoadMeshFromFolder(string folderPath) {
			return (Mesh)AssetDatabase.LoadAssetAtPath(folderPath, typeof(Mesh));
		}
	}
}
