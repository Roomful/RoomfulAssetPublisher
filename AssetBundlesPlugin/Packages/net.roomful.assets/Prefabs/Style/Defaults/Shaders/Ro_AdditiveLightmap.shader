
// http://forum.unity3d.com/threads/a-truly-lightmap-shader.22630/
// http://blenderartists.org/forum/showthread.php?242197-Lightmapping.&p=2025091&viewfull=1#post2025091

Shader "Custom/Ro_AdditiveLightmap"
{
	Properties
	{
		_Color("Main Color", Color) = (1,1,1,1)
		_MainTex("Texture", 2D) = "white" {}
		_LightMap("LightMap", 2D) = "white" {}
		_Contrast("Contrast", Range(1, 10)) = 1
	}

		SubShader
	{
		Tags{ "RenderType" = "Opaque" }
		LOD 200

		CGPROGRAM
		#pragma surface surf Lambert nodynlightmap
		struct Input {
			float2 uv_MainTex;
			float2 uv2_LightMap;
		};


		sampler2D _MainTex;
		sampler2D _LightMap;
		fixed4 _Color;
		
		float _Contrast;
		float _Attenuation;

		void surf(Input IN, inout SurfaceOutput o)
		{
			half4 col = tex2D(_MainTex, IN.uv_MainTex) * _Color;
			half4 lm = tex2D(_LightMap, IN.uv2_LightMap);

			_Attenuation = 1 / _Contrast;

			// Applying the lightmap in the Albedo gives the truest results
			// Using a contrast value we can expand the range of the lightmap in such a way that the dark is darker and the light is lighter
			// This translates into a much nicer dynamic range in the texture
			o.Albedo = col * (lm * _Contrast);

			// This much would be enough if the texture was unlit, but in this shader we accept influence of external lights
			// This means that without external lights, we only see black
			// We fix this by applying the lightmap in the Emission channel of the shader

			// Emission is never dark. Even low values like 0.1 or 0.5 contribute to lighten the material, which is a problem
			// For that reason we're using an attenuation value to artificially darken the lightmap, preventing the material from looking all white and washed out
			o.Emission = o.Albedo * _Attenuation;
			
			// Not really sure why this is here, since this shader doesn't take transparency
			// But it was in the bulit-in shader, so I'm leaving it here
			o.Alpha = lm.a * _Color.a;
		}

		ENDCG
		
	}
}