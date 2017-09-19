using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SerializedMaterial : MonoBehaviour {

	public string ShaderName = string.Empty;
	public List<ShaderProperty> ShadersProperties = new List<ShaderProperty> ();

	public void ImportMaterial(Material mat) {
		#if UNITY_EDITOR

		ShaderName = mat.shader.name;

		int shadersPropertyLength = UnityEditor.ShaderUtil.GetPropertyCount (mat.shader);
		for (int i = 0; i < shadersPropertyLength; i++) {
			ShaderProperty property = new ShaderProperty();

			string propertyName = UnityEditor.ShaderUtil.GetPropertyName (mat.shader, i);
			UnityEditor.ShaderUtil.ShaderPropertyType propertyType = UnityEditor.ShaderUtil.GetPropertyType (mat.shader, i);

			property.PropertyName = propertyName;
			property.PropertyType = propertyType.ToString();

			switch(propertyType) {
			case UnityEditor.ShaderUtil.ShaderPropertyType.TexEnv:
				property.TextureValue = mat.GetTexture(propertyName);

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

	public Material ExportMaterial() {
		Material mat = new Material(Shader.Find(ShaderName));

		foreach (ShaderProperty property in ShadersProperties) {
			ShaderPropertyType propertyType = (ShaderPropertyType)System.Enum.Parse(typeof(ShaderPropertyType), property.PropertyType);

			switch(propertyType) {
			case ShaderPropertyType.TexEnv:
				mat.SetTexture (property.PropertyName, property.TextureValue);

				break;
			case ShaderPropertyType.Float:
			case ShaderPropertyType.Range:
				mat.SetFloat (property.PropertyName, property.FloatValue);
				break;

			case ShaderPropertyType.Vector:
				mat.SetVector (property.PropertyName, property.VectorValue);
				break;
			case ShaderPropertyType.Color:
				mat.SetColor (property.PropertyName, property.VectorValue);

				break;
			}
		}

		return mat;
	}
}
