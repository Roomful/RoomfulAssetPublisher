using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RF.AssetBundles.Serialization {
	public class SerializedMaterial : MonoBehaviour {

		public string MatName = string.Empty;
		public string ShaderName = string.Empty;
		public List<SerializedShaderProperty> ShadersProperties = new List<SerializedShaderProperty> ();
		public List<string> ShaderKeywords = new List<string> ();

        public void Serialize(Material mat) {
#if UNITY_EDITOR

			MatName = mat.name.Replace("/", "");
			ShaderName = mat.shader.name;

            foreach (string keyword in mat.shaderKeywords) {
                if (!ShaderKeywords.Contains(keyword)) {
                    ShaderKeywords.Add(keyword);
                }
            }
           
            int shadersPropertyLength = UnityEditor.ShaderUtil.GetPropertyCount (mat.shader);
			for (int i = 0; i < shadersPropertyLength; i++) {
				SerializedShaderProperty property = new SerializedShaderProperty();

				string propertyName = UnityEditor.ShaderUtil.GetPropertyName (mat.shader, i);
				UnityEditor.ShaderUtil.ShaderPropertyType propertyType = UnityEditor.ShaderUtil.GetPropertyType (mat.shader, i);

				property.PropertyName = propertyName;
				property.PropertyType = propertyType.ToString();

				switch(propertyType) {
				case UnityEditor.ShaderUtil.ShaderPropertyType.TexEnv:
					if (mat.GetTexture(propertyName) != null) {
						property.SerializedTextureValue = new SerializedTexture();
						property.SerializedTextureValue.Serialize(mat.GetTexture(propertyName));
					}

					break;
				case UnityEditor.ShaderUtil.ShaderPropertyType.Float:
				case UnityEditor.ShaderUtil.ShaderPropertyType.Range:
					property.FloatValue = mat.GetFloat(propertyName);

					break;
				case UnityEditor.ShaderUtil.ShaderPropertyType.Vector:
				case UnityEditor.ShaderUtil.ShaderPropertyType.Color:
					property.VectorValue = mat.GetVector(propertyName);

					break;
				}

				ShadersProperties.Add(property);
			}

#endif
		}
    }
}