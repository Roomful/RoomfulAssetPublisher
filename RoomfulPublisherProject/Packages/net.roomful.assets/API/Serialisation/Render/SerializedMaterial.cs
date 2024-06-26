using System.Collections.Generic;
using UnityEngine;
using UnityEditor;

namespace net.roomful.assets.serialization
{
    public class SerializedMaterial : MonoBehaviour
    {
        public string MatName = string.Empty;
        public string ShaderName = string.Empty;
        public List<SerializedShaderProperty> ShadersProperties = new List<SerializedShaderProperty>();
        public List<string> ShaderKeywords = new List<string>();
        public int RenderQueue = -1;
        public MaterialGlobalIlluminationFlags IllumintaionFlags;

        public void Serialize(Material mat) {
#if UNITY_EDITOR

            MatName = mat.name.Replace("/", "");
            ShaderName = mat.shader.name;
            ShaderKeywords = new List<string>(mat.shaderKeywords);
            RenderQueue = mat.renderQueue;
            IllumintaionFlags = mat.globalIlluminationFlags;

            var shadersPropertyLength = ShaderUtil.GetPropertyCount(mat.shader);
            for (var i = 0; i < shadersPropertyLength; i++) {
                var property = new SerializedShaderProperty();

                var propertyName = ShaderUtil.GetPropertyName(mat.shader, i);
                var propertyType = ShaderUtil.GetPropertyType(mat.shader, i);

                property.PropertyName = propertyName;
                property.PropertyType = propertyType.ToString();

                switch (propertyType) {
                    case ShaderUtil.ShaderPropertyType.TexEnv:
                        if (mat.shader.name.Contains("Shader Graphs/Reflective") && propertyName == "_ReflectionTex")
                            break;
                        
                        property.SerializedTextureValue = new SerializedTexture();
                        if (mat.GetTexture(propertyName) != null) {

                            // Need to change compressions and reimport
                            var texPath = AssetDatabase.GetAssetPath(mat.GetTexture(propertyName));
                            var asset = (TextureImporter) AssetImporter.GetAtPath(texPath);
                            if (!asset.crunchedCompression || asset.textureCompression != TextureImporterCompression.Compressed || asset.compressionQuality != 100)
                            {
                                asset.crunchedCompression = true;
                                asset.textureCompression = TextureImporterCompression.Compressed;
                                asset.compressionQuality = 100;
                                asset.SaveAndReimport();
                            }

                            property.SerializedTextureValue.Serialize(mat.GetTexture(propertyName));
                        }

                        property.SerializedTextureValue.TextureScale = mat.GetTextureScale(propertyName);
                        property.SerializedTextureValue.TextureOffset = mat.GetTextureOffset(propertyName);
                        break;
                    case ShaderUtil.ShaderPropertyType.Float:
                    case ShaderUtil.ShaderPropertyType.Range:
                        property.FloatValue = mat.GetFloat(propertyName);

                        break;
                    case ShaderUtil.ShaderPropertyType.Vector:
                    case ShaderUtil.ShaderPropertyType.Color:
                        property.VectorValue = mat.GetVector(propertyName);

                        break;
                    default:
                        Debug.LogWarning("Unknown type");
                        break;
                }

                ShadersProperties.Add(property);
            }

#endif
        }
    }
}