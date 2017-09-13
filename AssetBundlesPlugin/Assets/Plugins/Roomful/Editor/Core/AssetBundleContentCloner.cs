using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using UnityEngine.Rendering;

namespace RF.AssetWizzard.Editor {
	public static class AssetBundleContentCloner  {

		private static PropAsset s_clonedProp = null;

		public static void Clone(PropAsset prop) {
			s_clonedProp = prop;

			ValidateBundleFolder ();

            foreach (Renderer renderer in prop.GetComponentsInChildren<Renderer>()) {
                var propMaterials = new List<Material>();
                var textureNames = new string[] { "_MainTex", "_BumpMap", "_MetallicGlossMap", "_EmissionMap", "_OcclusionMap", "_DetailAlbedoMap", "_DetailNormalMap", "_ParallaxMap"};

                foreach (Material material in renderer.sharedMaterials) {
                    if(material == null) {
                        continue;
                    }
                
                    Material newMat = RecreateMaterial(material);
                    foreach (var texName in textureNames) {
                        if (!material.HasProperty(texName)) {
                            continue;
                        }

                        Texture texture = material.GetTexture(texName);
	                    if (texture != null) {
		                    SaveTexture(texture);
		                    if (texName == "_BumpMap") {
			                    texture = LoadNormalMapFromFolder(GetFullTexturePath(texture));
		                    }
		                    else {
			                    texture = LoadTextureFromFolder(GetFullTexturePath(texture));
		                    }
		                    newMat.SetTexture(texName, texture);
                        }
                    }
                    propMaterials.Add(newMat);
                }

                renderer.sharedMaterials = propMaterials.ToArray();
            }

            foreach (MeshFilter meshFilter in prop.GetComponentsInChildren<MeshFilter>()) {
                Mesh mesh = meshFilter.GetComponent<MeshFilter>().sharedMesh;

                if (mesh != null) {
                    Mesh newMesh = RecreateMesh(mesh);
                    meshFilter.GetComponent<MeshFilter>().sharedMesh = newMesh;
                }
            }
			s_clonedProp = null;
		}

		private static Material RecreateMaterial(Material mat) {
			string cleanedMatName = mat.name.Replace("/", "");

			string fullPath = AssetBundlesSettings.AssetBundlesPathFull + "/" + s_clonedProp.Template.Title + "/Materials/" + cleanedMatName + ".mat";
			string path = AssetBundlesSettings.AssetBundlesPath + "/" + s_clonedProp.Template.Title + "/Materials/" + cleanedMatName + ".mat";

			if (!FolderUtils.IsFileExists(path)) {
				Material newMat = new Material (mat);
				if (mat.HasProperty("_Mode")) {
					int renderMode = (int) mat.GetFloat("_Mode");
					switch (renderMode) {
						case 0: //Opaque
							newMat.renderQueue = -1;
							break;
						case 1: // Cut out
							newMat.renderQueue = 2450;
							break;
						case 2: // Fade
							newMat.renderQueue = 3000;
							break;
						case 3: // Transparent
							newMat.renderQueue = 3000;
							break;
					}
				}
				SaveMaterialToFolder (newMat, fullPath);
				
			}

			return LoadMaterialFromFolder(fullPath);
		}

		private static string GetFullTexturePath(Texture tex) {
			return AssetBundlesSettings.AssetBundlesPathFull + "/" + s_clonedProp.Template.Title + "/Textures/" + tex.name + ".png";
		}
		
		private static string GetShortTexturePath(Texture tex) {
			return AssetBundlesSettings.AssetBundlesPath + "/" + s_clonedProp.Template.Title + "/Textures/" + tex.name + ".png";
		}
		
		private static void SaveTexture(Texture tex) {
			string fullPath = GetFullTexturePath(tex);
			string path = GetShortTexturePath(tex);

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
		}

		private static Mesh RecreateMesh(Mesh mesh) {
			string fullPath = AssetBundlesSettings.AssetBundlesPathFull + "/" + s_clonedProp.Template.Title + "/Meshes/" + mesh.name + ".asset";
			string path = AssetBundlesSettings.AssetBundlesPath + "/" + s_clonedProp.Template.Title + "/Meshes/" + mesh.name + ".asset";

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
			string path = AssetBundlesSettings.AssetBundlesPath + "/" + s_clonedProp.Template.Title;

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

		private static Texture2D LoadTextureFromFolder(string filePath) {
			return (Texture2D)AssetDatabase.LoadAssetAtPath(filePath, typeof(Texture2D));
		}

		private static Texture2D LoadNormalMapFromFolder(string filePath) {
			TextureImporter ti = (TextureImporter)TextureImporter.GetAtPath(filePath);
			TextureImporterSettings settings = new TextureImporterSettings();
			ti.ReadTextureSettings(settings);
			settings.textureType = TextureImporterType.NormalMap;
			ti.SetTextureSettings(settings);
			ti.SaveAndReimport();
			return (Texture2D)AssetDatabase.LoadAssetAtPath(filePath, typeof(Texture2D));
		}
		
		private static void SaveMeshToFolder(Mesh mesh, string folderPath) {
			AssetDatabase.CreateAsset(mesh, folderPath);
		}

		private static Mesh LoadMeshFromFolder(string folderPath) {
			return (Mesh)AssetDatabase.LoadAssetAtPath(folderPath, typeof(Mesh));
		}
	}
}
