using System.Collections.Generic;
using UnityEngine;

using RF.AssetBundles.Serialization;
using System;

namespace RF.AssetWizzard.Editor
{
	
	public class RendererCollector : BaseCollector {

		public override void Run(IAsset asset) {

			Renderer[] rens = asset.gameObject.GetComponentsInChildren<Renderer> (true);
            foreach (Renderer ren in rens) {
                SerializedMaterial[] materialsData = ren.gameObject.GetComponents<SerializedMaterial> ();

				if (materialsData.Length > 0) {
                    List<Material> exportedMterials = new List<Material> ();
                   
					foreach (SerializedMaterial sm in materialsData) {
                        Material m = DeserealizeMaterial(sm, asset);
                        exportedMterials.Add(m);
                    }
                    
                    ren.sharedMaterials = exportedMterials.ToArray ();
                    
                    for (int i = materialsData.Length - 1; i >= 0; i--) {
						GameObject.DestroyImmediate(materialsData[i]); 
					}
                }
			}

            Projector[] projectors = asset.gameObject.GetComponentsInChildren<Projector>();
            foreach (Projector p in projectors) {

                SerializedMaterial sm = p.gameObject.GetComponent<SerializedMaterial>();

                if (sm != null) {
                    Material m = DeserealizeMaterial(sm, asset);
                    p.material = m;

                    GameObject.DestroyImmediate(sm);
                }
            }
        }



        private Material DeserealizeMaterial(SerializedMaterial sm, IAsset asset) {
            var shader = Shader.Find(sm.ShaderName);
            if (shader == null) {
                shader = Shader.Find("Standard");
            }
            Material newMaterial = new Material(shader);
            newMaterial.name = sm.MatName;
 
    
            if (AssetDatabase.IsAssetExist<Material>(asset, newMaterial)) {
                return AssetDatabase.LoadAsset<Material>(asset, newMaterial.name);
            } else {

                
                newMaterial.DisableKeyword("_NORMALMAP");
                newMaterial.DisableKeyword("_ALPHATEST_ON");
                newMaterial.DisableKeyword("_ALPHABLEND_ON");
                newMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                newMaterial.DisableKeyword("_EMISSION");
                newMaterial.DisableKeyword("_PARALLAXMAP");
                newMaterial.DisableKeyword("_DETAIL_MULX2");
                newMaterial.DisableKeyword("_METALLICGLOSSMAP");
                newMaterial.DisableKeyword("_SPECGLOSSMAP");

                newMaterial.shaderKeywords = sm.ShaderKeywords.ToArray();
                foreach (string keyword in newMaterial.shaderKeywords) {
                    newMaterial.EnableKeyword(keyword);
                }

                newMaterial.renderQueue = sm.RenderQueue;




                foreach (SerializedShaderProperty property in sm.ShadersProperties) {
                    ShaderPropertyType propertyType = (ShaderPropertyType)System.Enum.Parse(typeof(ShaderPropertyType), property.PropertyType);

                    switch (propertyType) {
                        case ShaderPropertyType.TexEnv:
                            if (property.SerializedTextureValue != null) {

                                if(property.SerializedTextureValue.MainTexture != null) {
                                    string texName = property.SerializedTextureValue.MainTexture.name;
                                    AssetDatabase.SaveAsset<Texture>(asset, property.SerializedTextureValue.MainTexture);
                                    var textureCollector = new TextureCollector();
                                    textureCollector.SetAssetDatabase(AssetDatabase);
                                    textureCollector.Run(asset, property.SerializedTextureValue);

                                    if (property.PropertyName.Equals("_BumpMap")) {
                                        AssetDatabase.LoadAsset<Material>(asset, newMaterial.name).EnableKeyword("_NORMALMAP");
                                    }
                                    newMaterial.SetTexture(property.PropertyName, AssetDatabase.LoadAsset<Texture>(asset, texName));
                                }

                                newMaterial.SetTextureScale(property.PropertyName, property.SerializedTextureValue.TextureScale);
                                newMaterial.SetTextureOffset(property.PropertyName, property.SerializedTextureValue.TextureOffset);
                            }

                           
                            break;

                        case ShaderPropertyType.Float:
                        case ShaderPropertyType.Range:
                            newMaterial.SetFloat(property.PropertyName, property.FloatValue);
                            break;

                        case ShaderPropertyType.Vector:
                            newMaterial.SetVector(property.PropertyName, property.VectorValue);

                            break;
                        case ShaderPropertyType.Color:
                            newMaterial.SetColor(property.PropertyName, property.VectorValue);

                            break;
                    }

                    if (property.PropertyName.Equals("_Mode")) {
                        int renderMode = (int)property.FloatValue;
                        switch (renderMode) {
                            case 0: //Opaque
                                newMaterial.SetOverrideTag("RenderType", "");

                                break;
                            case 1: // Cut out
                                newMaterial.SetOverrideTag("RenderType", "TransparentCutout");
                                break;
                            case 2: // Fade
                                newMaterial.SetOverrideTag("RenderType", "Transparent");
                                break;
                            case 3: // Transparent
                                newMaterial.SetOverrideTag("RenderType", "Transparent");
                                break;
                        }
                    }


                    AssetDatabase.SaveAsset<Material>(asset, newMaterial);
                }

                return AssetDatabase.LoadAsset<Material>(asset, newMaterial.name);
            }
        }

	}
}