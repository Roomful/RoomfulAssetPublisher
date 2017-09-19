using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

[Serializable]
public class ShaderProperty {

	public string PropertyName = string.Empty;
	public string  PropertyType = string.Empty;

	public Vector4 VectorValue;
	public float FloatValue;
	public Texture TextureValue;
}
