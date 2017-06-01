Shader "Roomful/Silhouette" {
	Properties {
		//_MainTex ("Albedo (RGB)", 2D) = "white" {}
		_Color ("Color", Color) = (1,1,1,1)
		_Emission("Emission", Color) = (0,0,0)

		_Amount("Extrusion Amount", Range(-1,1)) = 0.001
	}

	SubShader {
		Tags {
			"RenderType"="Transparent"
			"Queue"     ="Transparent+10"
		}
		LOD 200
		
		CGPROGRAM
		#pragma surface surf Standard fullforwardshadows alpha:fade vertex:vert
		#pragma target 3.0

		struct Input {
			float2 uv_MainTex;
			// "surf" needs to have 2 parameters and we can't leave "Input" empty, so let's leave it with a float
			// but ignore all the texture related operations, since we don't use textures
		};
		
		fixed4 _Color;
		fixed3 _Emission;

		float _Amount;


		void vert(inout appdata_full v) {
			v.vertex.xyz += v.normal * _Amount;
		}


		void surf (Input IN, inout SurfaceOutputStandard o) {
			//fixed4 c = /*tex2D (MainTex, IN.uv_MainTex) **/ Color; // Leaving this here in case we change our mind about textures
			fixed4 c = _Color;
			o.Albedo = c.rgb;

			o.Emission = _Emission;
			o.Alpha = c.a;
		}
		ENDCG
	}
	FallBack "Diffuse"
}