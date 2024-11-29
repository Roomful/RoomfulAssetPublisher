// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "kShading/Blur"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            float _offset;

            sampler2D _MainTex;
            float4 _MainTex_TexelSize;

            fixed4 frag (v2f input) : SV_Target
            {
                float2 res = _MainTex_TexelSize.xy;
                
                fixed4 col;
                col = tex2D( _MainTex, input.uv );
                col += tex2D( _MainTex, input.uv + float2( _offset, _offset ) * res );
                col += tex2D( _MainTex, input.uv + float2( _offset, -_offset ) * res );
                col += tex2D( _MainTex, input.uv + float2( -_offset, _offset ) * res );
                col += tex2D( _MainTex, input.uv + float2( -_offset, -_offset ) * res );
                
                col += tex2D( _MainTex, input.uv + float2( 0.0f, _offset ) * res );
                col += tex2D( _MainTex, input.uv + float2( 0.0f, -_offset ) * res );
                col += tex2D( _MainTex, input.uv + float2( _offset, 0.0f ) * res );
                col += tex2D( _MainTex, input.uv + float2( -_offset, 0.0f ) * res );
                
                col.rgb /= 9.0f;

                return col;
            }
            ENDCG
        }
    }
}
