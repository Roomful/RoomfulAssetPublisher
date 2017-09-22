﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF.AssetBundles {
	
	public class RendererCollector : ICollector {

		public void Run(RF.AssetWizzard.PropAsset propAsset) {
			#if UNITY_EDITOR

			Renderer[] rens = propAsset.gameObject.GetComponentsInChildren<Renderer> ();

			foreach (Renderer ren in rens) {
				SerializedMaterial[] materialsData = ren.gameObject.GetComponents<SerializedMaterial> ();

				if (materialsData.Length > 0) {
					List<Material> exportedMterials = new List<Material> ();

					foreach (SerializedMaterial sm in materialsData) {
						Material newMaterial = new Material(Shader.Find(sm.ShaderName));
						newMaterial.name = sm.MatName;

						foreach (ShaderProperty property in sm.ShadersProperties) {
							ShaderPropertyType propertyType = (ShaderPropertyType)System.Enum.Parse(typeof(ShaderPropertyType), property.PropertyType);

							switch(propertyType) {
							case ShaderPropertyType.TexEnv:

								if (property.TextureValue != null) {
									string texName = property.TextureValue.name;

									PropDataBase.SaveAsset<Texture> (propAsset, property.TextureValue);

									if (property.PropertyName.Equals("_BumpMap")) {
										string path = UnityEditor.AssetDatabase.GetAssetPath(PropDataBase.LoadAsset<Texture>(propAsset, texName));

										UnityEditor.TextureImporter ti = (UnityEditor.TextureImporter)UnityEditor.TextureImporter.GetAtPath(path);
										UnityEditor.TextureImporterSettings settings = new UnityEditor.TextureImporterSettings();
										ti.ReadTextureSettings(settings);
										settings.textureType = UnityEditor.TextureImporterType.NormalMap;
										ti.SetTextureSettings(settings);
										ti.SaveAndReimport();
									}

									newMaterial.SetTexture(property.PropertyName, PropDataBase.LoadAsset<Texture>(propAsset, texName));
								}
								break;

							case ShaderPropertyType.Float:
							case ShaderPropertyType.Range:
								newMaterial.SetFloat (property.PropertyName, property.FloatValue);
								break;

							case ShaderPropertyType.Vector:
								newMaterial.SetVector (property.PropertyName, property.VectorValue);

								break;
							case ShaderPropertyType.Color:
								newMaterial.SetColor (property.PropertyName, property.VectorValue);

								break;
							}

							if (property.PropertyName.Equals("_Mode")) {
								int renderMode = (int) property.FloatValue;
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

						exportedMterials.Add (PropDataBase.LoadAsset<Material>(propAsset, newMaterial.name));
					}

					ren.materials = exportedMterials.ToArray ();

					for (int i = materialsData.Length - 1; i >= 0; i--) {
						GameObject.DestroyImmediate(materialsData[i]); 
					}
				}
			}
			#endif
		}
	}
}