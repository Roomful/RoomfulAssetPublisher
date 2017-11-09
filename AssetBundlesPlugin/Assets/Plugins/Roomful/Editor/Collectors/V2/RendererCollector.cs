using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_EDITOR
using UnityEditor;
#endif

using RF.AssetBundles.Serialization;


namespace RF.AssetWizzard
{
	
	public class RendererCollector : ICollector {

		public void Run(RF.AssetWizzard.PropAsset propAsset) {

            #if UNITY_EDITOR

			Renderer[] rens = propAsset.gameObject.GetComponentsInChildren<Renderer> (true);
            
            foreach (Renderer ren in rens) {
                SerializedMaterial[] materialsData = ren.gameObject.GetComponents<SerializedMaterial> ();

				if (materialsData.Length > 0) {
                    List<Material> exportedMterials = new List<Material> ();
                   
					foreach (SerializedMaterial sm in materialsData) {
						Material newMaterial = new Material(Shader.Find(sm.ShaderName));
                        newMaterial.name = sm.MatName;

                        if (PropDataBase.IsAssetExist<Material>(propAsset, newMaterial)) {
                           exportedMterials.Add(PropDataBase.LoadAsset<Material>(propAsset, newMaterial.name));

                        } else {
                            PropDataBase.SaveAsset<Material>(propAsset, newMaterial);

                            foreach (SerializedShaderProperty property in sm.ShadersProperties) {
                                ShaderPropertyType propertyType = (ShaderPropertyType)System.Enum.Parse(typeof(ShaderPropertyType), property.PropertyType);

                                switch (propertyType) {
                                    case ShaderPropertyType.TexEnv:
                                        if (property.SerializedTextureValue != null && property.SerializedTextureValue.MainTexture != null) {
                                            string texName = property.SerializedTextureValue.MainTexture.name;

                                            PropDataBase.SaveAsset<Texture>(propAsset, property.SerializedTextureValue.MainTexture);
                                            new TextureCollector().Run(propAsset, property.SerializedTextureValue);

                                            if (property.PropertyName.Equals("_BumpMap")) {
                                                PropDataBase.LoadAsset<Material>(propAsset, newMaterial.name).EnableKeyword("_NORMALMAP");
                                            }

                                            PropDataBase.LoadAsset<Material>(propAsset, newMaterial.name).SetTexture(property.PropertyName, PropDataBase.LoadAsset<Texture>(propAsset, texName));
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
                                            newMaterial.SetOverrideTag("RenderType","");
                                            newMaterial.SetInt("_SrcBlend",(int)UnityEngine.Rendering.BlendMode.One);
                                            newMaterial.SetInt("_DstBlend",(int)UnityEngine.Rendering.BlendMode.Zero);
                                            newMaterial.SetInt("_ZWrite",1);
                                            newMaterial.DisableKeyword("_ALPHATEST_ON");
                                            newMaterial.DisableKeyword("_ALPHABLEND_ON");
                                            newMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                                            newMaterial.renderQueue = -1;
                                            break;
                                        case 1: // Cut out
                                            newMaterial.SetOverrideTag("RenderType","TransparentCutout");
                                            newMaterial.SetInt("_SrcBlend",(int)UnityEngine.Rendering.BlendMode.One);
                                            newMaterial.SetInt("_DstBlend",(int)UnityEngine.Rendering.BlendMode.Zero);
                                            newMaterial.SetInt("_ZWrite",1);
                                            newMaterial.EnableKeyword("_ALPHATEST_ON");
                                            newMaterial.DisableKeyword("_ALPHABLEND_ON");
                                            newMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                                            newMaterial.renderQueue = 2450;
                                            break;
                                        case 2: // Fade
                                            newMaterial.SetOverrideTag("RenderType","Transparent");
                                            newMaterial.SetInt("_SrcBlend",(int)UnityEngine.Rendering.BlendMode.SrcAlpha);
                                            newMaterial.SetInt("_DstBlend",(int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                                            newMaterial.SetInt("_ZWrite",0);
                                            newMaterial.DisableKeyword("_ALPHATEST_ON");
                                            newMaterial.EnableKeyword("_ALPHABLEND_ON");
                                            newMaterial.DisableKeyword("_ALPHAPREMULTIPLY_ON");
                                            newMaterial.renderQueue = 3000;
                                            break;
                                        case 3: // Transparent
                                            newMaterial.SetOverrideTag("RenderType","Transparent");
                                            newMaterial.SetInt("_SrcBlend",(int)UnityEngine.Rendering.BlendMode.One);
                                            newMaterial.SetInt("_DstBlend",(int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
                                            newMaterial.SetInt("_ZWrite",0);
                                            newMaterial.DisableKeyword("_ALPHATEST_ON");
                                            newMaterial.DisableKeyword("_ALPHABLEND_ON");
                                            newMaterial.EnableKeyword("_ALPHAPREMULTIPLY_ON");
                                            newMaterial.renderQueue = 3000;
                                            break;
                                    }
                                }
                            }

                            exportedMterials.Add(PropDataBase.LoadAsset<Material>(propAsset, newMaterial.name));
                        }
                    }
                    
                    ren.sharedMaterials = exportedMterials.ToArray ();
                    
                    for (int i = materialsData.Length - 1; i >= 0; i--) {
						GameObject.DestroyImmediate(materialsData[i]); 
					}
                }
                
			}
            
			#endif
		}
	}
}