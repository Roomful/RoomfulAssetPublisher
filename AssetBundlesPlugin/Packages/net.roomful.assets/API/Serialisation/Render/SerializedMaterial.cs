using System.Collections.Generic;
using UnityEngine;

namespace net.roomful.assets.serialization {
	public class SerializedMaterial : MonoBehaviour {

		public string MatName = string.Empty;
		public string ShaderName = string.Empty;
		public List<SerializedShaderProperty> ShadersProperties = new List<SerializedShaderProperty> ();
		public List<string> ShaderKeywords = new List<string> ();
        public int RenderQueue = -1;

        public void Serialize(Material mat) {
#if UNITY_EDITOR

			MatName = mat.name.Replace("/", "");
			ShaderName = mat.shader.name;
            ShaderKeywords = new List<string>(mat.shaderKeywords);
            RenderQueue = mat.renderQueue;

            var shadersPropertyLength = UnityEditor.ShaderUtil.GetPropertyCount (mat.shader);
			for (var i = 0; i < shadersPropertyLength; i++) {
				var property = new SerializedShaderProperty();

				var propertyName = UnityEditor.ShaderUtil.GetPropertyName (mat.shader, i);
                var propertyType = UnityEditor.ShaderUtil.GetPropertyType (mat.shader, i);

				property.PropertyName = propertyName;
				property.PropertyType = propertyType.ToString();

				switch(propertyType) {
				case UnityEditor.ShaderUtil.ShaderPropertyType.TexEnv:
                   property.SerializedTextureValue = new SerializedTexture();
                   if (mat.GetTexture(propertyName) != null) {
					    property.SerializedTextureValue.Serialize(mat.GetTexture(propertyName));
                   }
                   property.SerializedTextureValue.TextureScale = mat.GetTextureScale(propertyName);
                   property.SerializedTextureValue.TextureOffset = mat.GetTextureOffset(propertyName);

                    break;
				case UnityEditor.ShaderUtil.ShaderPropertyType.Float:
				case UnityEditor.ShaderUtil.ShaderPropertyType.Range:
					property.FloatValue = mat.GetFloat(propertyName);

					break;
				case UnityEditor.ShaderUtil.ShaderPropertyType.Vector:
				case UnityEditor.ShaderUtil.ShaderPropertyType.Color:
					property.VectorValue = mat.GetVector(propertyName);

					break;
                    default:
                        Debug.Log("Inknow type");
                        break;
				}

				ShadersProperties.Add(property);
			}

#endif
		}
    }
}