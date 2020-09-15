using System;
using net.roomful.assets.serialization;
using StansAssets.Foundation;
using UnityEngine;

namespace net.roomful.assets
{
    public static class MaterialDeserializer
    {
        public static Material CreteMaterial(SerializedMaterial sm) {
            var shader = Shader.Find(sm.ShaderName);
            if (shader == null) {
                shader = Shader.Find("Standard");
                Debug.LogWarning($"{nameof(MaterialDeserializer)}: Shader +" + sm.ShaderName + " wasn't found, fallback Standard");
            }

            var newMaterial = new Material(shader);
            newMaterial.name = sm.MatName;
            return newMaterial;
        }

        public static void ApplySerializedProperties(Material material, SerializedMaterial sm) {
            ApplySerializedProperties(material, sm, serializedTexture => serializedTexture.MainTexture);
        }

        public static void ApplySerializedProperties(Material material, SerializedMaterial sm, Func<SerializedTexture, Texture> textureProcessorFunc) {
            material.DisableKeyword("_NORMALMAP");
            material.DisableKeyword("_ALPHATEST_ON");
            material.DisableKeyword("_ALPHABLEND_ON");
            material.DisableKeyword("_ALPHAPREMULTIPLY_ON");
            material.DisableKeyword("_EMISSION");
            material.DisableKeyword("_PARALLAXMAP");
            material.DisableKeyword("_DETAIL_MULX2");
            material.DisableKeyword("_METALLICGLOSSMAP");
            material.DisableKeyword("_SPECGLOSSMAP");

            foreach (var keyword in sm.ShaderKeywords) {
                material.EnableKeyword(keyword);
            }

            material.renderQueue = sm.RenderQueue;

            foreach (var property in sm.ShadersProperties) {
                var propertyType = EnumUtility.ParseEnum<ShaderPropertyType>(property.PropertyType);
                switch (propertyType) {
                    case ShaderPropertyType.TexEnv:
                        if (property.SerializedTextureValue != null) {
                            if (property.SerializedTextureValue.MainTexture != null) {
                                material.SetTexture(property.PropertyName,textureProcessorFunc.Invoke(property.SerializedTextureValue));
                            }

                            if (property.PropertyName.Equals("_BumpMap")) {
                                material.EnableKeyword("_NORMALMAP");
                            }

                            if (material.HasProperty(property.PropertyName)) {
                                material.SetTextureScale(property.PropertyName, property.SerializedTextureValue.TextureScale);
                                material.SetTextureOffset(property.PropertyName, property.SerializedTextureValue.TextureOffset);
                            }
                        }

                        break;
                    case ShaderPropertyType.Float:
                    case ShaderPropertyType.Range:
                        material.SetFloat(property.PropertyName, property.FloatValue);
                        break;
                    case ShaderPropertyType.Vector:
                        material.SetVector(property.PropertyName, property.VectorValue);
                        break;
                    case ShaderPropertyType.Color:
                        material.SetColor(property.PropertyName, property.VectorValue);
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }

                if (property.PropertyName.Equals("_Mode")) {
                    var renderMode = (int) property.FloatValue;
                    switch (renderMode) {
                        case 0: //Opaque
                            material.SetOverrideTag("RenderType", "");
                            break;
                        case 1: // Cut out
                            material.SetOverrideTag("RenderType", "TransparentCutout");
                            break;
                        case 2: // Fade
                            material.SetOverrideTag("RenderType", "Transparent");
                            break;
                        case 3: // Transparent
                            material.SetOverrideTag("RenderType", "Transparent");
                            break;
                    }
                }
            }
        }
    }
}