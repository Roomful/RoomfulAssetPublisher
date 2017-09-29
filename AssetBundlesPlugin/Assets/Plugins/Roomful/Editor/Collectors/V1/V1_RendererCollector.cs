using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF.AssetWizzard
{

	public class V1_RendererCollector : ICollector {

		public void Run(RF.AssetWizzard.PropAsset propAsset) {
			
			Renderer[] rens = propAsset.GetComponentsInChildren<Renderer> ();
			foreach (Renderer ren in rens) {
				if (ren.sharedMaterials.Length > 0) {
					List<Material> recreatedMterials = new List<Material> ();

					foreach (Material m in ren.sharedMaterials) {
						if(m == null) {
							continue;
						}

						Material newMaterial = new Material(m);
						newMaterial.name = m.name.Replace("/", "");

						int shadersPropertyLength = UnityEditor.ShaderUtil.GetPropertyCount (newMaterial.shader);
						for (int i = 0; i < shadersPropertyLength; i++) {

							string propertyName = UnityEditor.ShaderUtil.GetPropertyName (newMaterial.shader, i);
							UnityEditor.ShaderUtil.ShaderPropertyType propertyType = UnityEditor.ShaderUtil.GetPropertyType (newMaterial.shader, i);

							if (propertyType == UnityEditor.ShaderUtil.ShaderPropertyType.TexEnv) {
								Texture tex = newMaterial.GetTexture(propertyName);

								if (tex != null) {
									string texName = tex.name;

									PropDataBase.SaveAsset<Texture> (propAsset, tex);

									if (propertyName.Equals("_BumpMap")) {
										string path = UnityEditor.AssetDatabase.GetAssetPath(PropDataBase.LoadAsset<Texture>(propAsset, texName));

										UnityEditor.TextureImporter ti = (UnityEditor.TextureImporter)UnityEditor.TextureImporter.GetAtPath(path);
										UnityEditor.TextureImporterSettings settings = new UnityEditor.TextureImporterSettings();
										ti.ReadTextureSettings(settings);
										settings.textureType = UnityEditor.TextureImporterType.NormalMap;
										ti.SetTextureSettings(settings);
										ti.SaveAndReimport();
									}

									newMaterial.SetTexture(propertyName, PropDataBase.LoadAsset<Texture>(propAsset, texName));
								}
							}

							if (propertyName.Equals("_Mode")) {
								int renderMode = (int) newMaterial.GetFloat(propertyName);
								switch (renderMode) {
								case 0: //Opaque
									newMaterial.renderQueue = -1;
									break;
								case 1: // Cut out
									newMaterial.renderQueue = 2450;
									break;
								case 2: // Fade
									newMaterial.renderQueue = 3000;
									break;
								case 3: // Transparent
									newMaterial.renderQueue = 3000;
									break;
								}
							}

						}

						PropDataBase.SaveAsset<Material> (propAsset, newMaterial);

						recreatedMterials.Add (PropDataBase.LoadAsset<Material>(propAsset, newMaterial.name));
					}

					ren.materials = recreatedMterials.ToArray ();
				}
			}
		}
	}
}