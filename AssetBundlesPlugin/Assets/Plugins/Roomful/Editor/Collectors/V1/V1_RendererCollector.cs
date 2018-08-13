using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF.AssetWizzard.Editor
{

	public class V1_RendererCollector : ICollector {

		public void Run(IAsset asset) {
			
			Renderer[] rens = asset.gameObject.GetComponentsInChildren<Renderer> (true);
			foreach (Renderer ren in rens) {
				if (ren.sharedMaterials.Length > 0) {
					List<Material> recreatedMterials = new List<Material> ();

					foreach (Material m in ren.sharedMaterials) {
						if(m == null) {
							continue;
						}

						Material newMaterial = new Material(m);
                        newMaterial.shader = Shader.Find(newMaterial.shader.name);

                        newMaterial.name = m.name.Replace("/", "");

                        AssetDatabase.SaveAsset<Material>(asset, newMaterial);

                        int shadersPropertyLength = UnityEditor.ShaderUtil.GetPropertyCount (newMaterial.shader);
						for (int i = 0; i < shadersPropertyLength; i++) {

							string propertyName = UnityEditor.ShaderUtil.GetPropertyName (newMaterial.shader, i);
							UnityEditor.ShaderUtil.ShaderPropertyType propertyType = UnityEditor.ShaderUtil.GetPropertyType (newMaterial.shader, i);

							if (propertyType == UnityEditor.ShaderUtil.ShaderPropertyType.TexEnv) {
								Texture tex = newMaterial.GetTexture(propertyName);

								if (tex != null) {
									string texName = tex.name;

									AssetDatabase.SaveAsset<Texture> (asset, tex);

									if (propertyName.Equals("_BumpMap")) {
										ReimportTextureAsNormalMap(asset, texName);
									}

                                    AssetDatabase.LoadAsset<Material>(asset, newMaterial.name).SetTexture(propertyName, AssetDatabase.LoadAsset<Texture>(asset, texName));
								}
							}

							if (propertyName.Equals("_Mode")) {
								int renderMode = (int) newMaterial.GetFloat(propertyName);
								switch (renderMode) {
								case 0: //Opaque
                                        AssetDatabase.LoadAsset<Material>(asset, newMaterial.name).renderQueue = -1;
									break;
								case 1: // Cut out
                                        AssetDatabase.LoadAsset<Material>(asset, newMaterial.name).renderQueue = 2450;
									break;
								case 2: // Fade
                                        AssetDatabase.LoadAsset<Material>(asset, newMaterial.name).renderQueue = 3000;
									break;
								case 3: // Transparent
                                        AssetDatabase.LoadAsset<Material>(asset, newMaterial.name).renderQueue = 3000;
									break;
								}
							}

						}
                        
						recreatedMterials.Add (AssetDatabase.LoadAsset<Material>(asset, newMaterial.name));
					}

					ren.materials = recreatedMterials.ToArray ();
				}
			}
		}

		private static void ReimportTextureAsNormalMap(IAsset asset, string texName) {
			string path = UnityEditor.AssetDatabase.GetAssetPath(AssetDatabase.LoadAsset<Texture>(asset, texName));

			UnityEditor.TextureImporter ti = (UnityEditor.TextureImporter) UnityEditor.TextureImporter.GetAtPath(path);
			UnityEditor.TextureImporterSettings settings = new UnityEditor.TextureImporterSettings();
			ti.ReadTextureSettings(settings);
			settings.textureType = UnityEditor.TextureImporterType.NormalMap;
			ti.SetTextureSettings(settings);
			ti.SaveAndReimport();
		}
	}
}