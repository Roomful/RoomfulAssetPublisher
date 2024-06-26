Shader "NatureManufacture Shaders/Debug/Flowmap Direction"
{
    Properties
    {
        [NoScaleOffset]_Direction("Direction", 2D) = "white" {}
        _TextureSize("Texture Size", Vector) = (246, 246, 0, 0)
        [ToggleUI]_RotateUV("Rotate UV", Float) = 1
        [ToggleUI]Boolean_F2B1E91A("Boolean", Float) = 0
        [NonModifiableTextureData][NoScaleOffset]_TexelSize_7CEA6DD4_Texture_1("Texture2D", 2D) = "white" {}
    }
    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "RenderType"="Opaque"
            "Queue"="Geometry+0"
        }
        
        Pass
        {
            Name "Pass"
            Tags 
            { 
                // LightMode: <None>
            }
           
            // Render State
            Blend One Zero, One Zero
            Cull Back
            ZTest LEqual
            ZWrite On
            // ColorMask: <None>
            
        
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
        
            // Debug
            // <None>
        
            // --------------------------------------------------
            // Pass
        
            // Pragmas
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0
            #pragma multi_compile_instancing
        
            // Keywords
            #pragma multi_compile _ LIGHTMAP_ON
            #pragma multi_compile _ DIRLIGHTMAP_COMBINED
            #pragma shader_feature _ _SAMPLE_GI
            // GraphKeywords: <None>
            
            // Defines
            #define _AlphaClip 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define ATTRIBUTES_NEED_TEXCOORD1
            #define ATTRIBUTES_NEED_TEXCOORD2
            #define ATTRIBUTES_NEED_TEXCOORD3
            #define VARYINGS_NEED_TEXCOORD0
            #define VARYINGS_NEED_TEXCOORD1
            #define VARYINGS_NEED_TEXCOORD2
            #define VARYINGS_NEED_TEXCOORD3
            #define SHADERPASS_UNLIT
        
            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        
            // --------------------------------------------------
            // Graph
        
            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
            float2 _TextureSize;
            float _RotateUV;
            float Boolean_F2B1E91A;
            CBUFFER_END
            TEXTURE2D(_Direction); SAMPLER(sampler_Direction); float4 _Direction_TexelSize;
            TEXTURE2D(_TexelSize_7CEA6DD4_Texture_1); SAMPLER(sampler_TexelSize_7CEA6DD4_Texture_1); float4 _TexelSize_7CEA6DD4_Texture_1_TexelSize;
            SAMPLER(_SampleTexture2DLOD_8992BD21_Sampler_3_Linear_Repeat);
        
            // Graph Functions
            
            void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
            {
                Out = UV * Tiling + Offset;
            }
            
            void Unity_Divide_float2(float2 A, float2 B, out float2 Out)
            {
                Out = A / B;
            }
            
            void Unity_Fraction_float2(float2 In, out float2 Out)
            {
                Out = frac(In);
            }
            
            void Unity_Multiply_float(float2 A, float2 B, out float2 Out)
            {
                Out = A * B;
            }
            
            void Unity_Subtract_float2(float2 A, float2 B, out float2 Out)
            {
                Out = A - B;
            }
            
            void Unity_Multiply_float(float A, float B, out float Out)
            {
                Out = A * B;
            }
            
            void Unity_Comparison_Greater_float(float A, float B, out float Out)
            {
                Out = A > B ? 1 : 0;
            }
            
            void Unity_Divide_float(float A, float B, out float Out)
            {
                Out = A / B;
            }
            
            void Unity_Arctangent_float(float In, out float Out)
            {
                Out = atan(In);
            }
            
            void Unity_Add_float(float A, float B, out float Out)
            {
                Out = A + B;
            }
            
            void Unity_Branch_float(float Predicate, float True, float False, out float Out)
            {
                Out = lerp(False, True, Predicate);
            }
            
            void Unity_Rotate_Radians_float(float2 UV, float2 Center, float Rotation, out float2 Out)
            {
                //rotation matrix
                UV -= Center;
                float s = sin(Rotation);
                float c = cos(Rotation);
            
                //center rotation matrix
                float2x2 rMatrix = float2x2(c, -s, s, c);
                rMatrix *= 0.5;
                rMatrix += 0.5;
                rMatrix = rMatrix*2 - 1;
            
                //multiply the UVs by the rotation matrix
                UV.xy = mul(UV.xy, rMatrix);
                UV += Center;
            
                Out = UV;
            }
            
            void Unity_Distance_float2(float2 A, float2 B, out float Out)
            {
                Out = distance(A, B);
            }
            
            void Unity_Clamp_float(float In, float Min, float Max, out float Out)
            {
                Out = clamp(In, Min, Max);
            }
            
            void Unity_Subtract_float(float A, float B, out float Out)
            {
                Out = A - B;
            }
            
            void Unity_Absolute_float(float In, out float Out)
            {
                Out = abs(In);
            }
            
            void Unity_Power_float(float A, float B, out float Out)
            {
                Out = pow(A, B);
            }
            
            void Unity_Lerp_float3(float3 A, float3 B, float3 T, out float3 Out)
            {
                Out = lerp(A, B, T);
            }
            
            void Unity_Branch_float3(float Predicate, float3 True, float3 False, out float3 Out)
            {
                Out = lerp(False, True, Predicate);
            }
            
            void Unity_Multiply_float(float3 A, float3 B, out float3 Out)
            {
                Out = A * B;
            }
        
            // Graph Vertex
            // GraphVertex: <None>
            
            // Graph Pixel
            struct SurfaceDescriptionInputs
            {
                float4 uv0;
                float4 uv1;
                float4 uv2;
                float4 uv3;
            };
            
            struct SurfaceDescription
            {
                float3 Color;
                float Alpha;
                float AlphaClipThreshold;
            };
            
            SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
            {
                SurfaceDescription surface = (SurfaceDescription)0;
                float4 _UV_423724E9_Out_0 = IN.uv0;
                float2 _TilingAndOffset_7FA2A59D_Out_3;
                Unity_TilingAndOffset_float((_UV_423724E9_Out_0.xy), float2 (2, 2), float2 (0, 0), _TilingAndOffset_7FA2A59D_Out_3);
                float2 _Vector2_7498D23E_Out_0 = float2(1, 1);
                float2 _Vector2_6903E1D9_Out_0 = float2(30, 30);
                float2 _Divide_4A1CD8EF_Out_2;
                Unity_Divide_float2(_Vector2_7498D23E_Out_0, _Vector2_6903E1D9_Out_0, _Divide_4A1CD8EF_Out_2);
                float2 _Divide_26E01EC5_Out_2;
                Unity_Divide_float2(_TilingAndOffset_7FA2A59D_Out_3, _Divide_4A1CD8EF_Out_2, _Divide_26E01EC5_Out_2);
                float2 _Fraction_43413307_Out_1;
                Unity_Fraction_float2(_Divide_26E01EC5_Out_2, _Fraction_43413307_Out_1);
                float2 _Multiply_D1CC8BD5_Out_2;
                Unity_Multiply_float(_Fraction_43413307_Out_1, _Divide_4A1CD8EF_Out_2, _Multiply_D1CC8BD5_Out_2);
                float2 _Vector2_F0C904E8_Out_0 = float2(246, 246);
                float _TexelSize_7CEA6DD4_Width_0 = _TexelSize_7CEA6DD4_Texture_1_TexelSize.z;
                float _TexelSize_7CEA6DD4_Height_2 = _TexelSize_7CEA6DD4_Texture_1_TexelSize.w;
                float2 _Vector2_7333DC_Out_0 = float2(_TexelSize_7CEA6DD4_Width_0, _TexelSize_7CEA6DD4_Height_2);
                float2 _Multiply_CED398EA_Out_2;
                Unity_Multiply_float(_Vector2_F0C904E8_Out_0, _Vector2_7333DC_Out_0, _Multiply_CED398EA_Out_2);
                float2 _Vector2_8BAEA7F5_Out_0 = float2(10, 10);
                float2 _Multiply_C50BCDFF_Out_2;
                Unity_Multiply_float(_Vector2_7333DC_Out_0, _Vector2_8BAEA7F5_Out_0, _Multiply_C50BCDFF_Out_2);
                float2 _Subtract_A91F83E3_Out_2;
                Unity_Subtract_float2(_Multiply_CED398EA_Out_2, _Multiply_C50BCDFF_Out_2, _Subtract_A91F83E3_Out_2);
                float2 _Multiply_6787B782_Out_2;
                Unity_Multiply_float(_Multiply_D1CC8BD5_Out_2, _Subtract_A91F83E3_Out_2, _Multiply_6787B782_Out_2);
                float2 _Vector2_AC47F18A_Out_0 = float2(0.1, 0.1);
                float2 _Multiply_C5FB853B_Out_2;
                Unity_Multiply_float(_Multiply_6787B782_Out_2, _Vector2_AC47F18A_Out_0, _Multiply_C5FB853B_Out_2);
                float4 _UV_329B19A_Out_0 = IN.uv3;
                float _Split_8187D1E5_R_1 = _UV_329B19A_Out_0[0];
                float _Split_8187D1E5_G_2 = _UV_329B19A_Out_0[1];
                float _Split_8187D1E5_B_3 = _UV_329B19A_Out_0[2];
                float _Split_8187D1E5_A_4 = _UV_329B19A_Out_0[3];
                float _Multiply_EA31EA94_Out_2;
                Unity_Multiply_float(_Split_8187D1E5_G_2, -1, _Multiply_EA31EA94_Out_2);
                float _Comparison_A5CAC47B_Out_2;
                Unity_Comparison_Greater_float(_Multiply_EA31EA94_Out_2, 0, _Comparison_A5CAC47B_Out_2);
                float _Divide_559969E6_Out_2;
                Unity_Divide_float(_Split_8187D1E5_R_1, _Multiply_EA31EA94_Out_2, _Divide_559969E6_Out_2);
                float _Arctangent_9052141C_Out_1;
                Unity_Arctangent_float(_Divide_559969E6_Out_2, _Arctangent_9052141C_Out_1);
                float _Add_19EC7E1E_Out_2;
                Unity_Add_float(_Arctangent_9052141C_Out_1, 3.141592, _Add_19EC7E1E_Out_2);
                float _Branch_21642C59_Out_3;
                Unity_Branch_float(_Comparison_A5CAC47B_Out_2, _Arctangent_9052141C_Out_1, _Add_19EC7E1E_Out_2, _Branch_21642C59_Out_3);
                float2 _Rotate_9EA39ADE_Out_3;
                Unity_Rotate_Radians_float(_Multiply_C5FB853B_Out_2, float2 (0.5, 0.5), _Branch_21642C59_Out_3, _Rotate_9EA39ADE_Out_3);
                float4 _SampleTexture2DLOD_8992BD21_RGBA_0 = SAMPLE_TEXTURE2D_LOD(_Direction, sampler_Direction, _Rotate_9EA39ADE_Out_3, 0);
                float _SampleTexture2DLOD_8992BD21_R_5 = _SampleTexture2DLOD_8992BD21_RGBA_0.r;
                float _SampleTexture2DLOD_8992BD21_G_6 = _SampleTexture2DLOD_8992BD21_RGBA_0.g;
                float _SampleTexture2DLOD_8992BD21_B_7 = _SampleTexture2DLOD_8992BD21_RGBA_0.b;
                float _SampleTexture2DLOD_8992BD21_A_8 = _SampleTexture2DLOD_8992BD21_RGBA_0.a;
                float _Comparison_1DC9309_Out_2;
                Unity_Comparison_Greater_float(_Split_8187D1E5_G_2, 0.001, _Comparison_1DC9309_Out_2);
                float3 _Vector3_9D2D4CCD_Out_0 = float3(0, 0, 0);
                float2 _Vector2_BA8118B5_Out_0 = float2(_Split_8187D1E5_R_1, _Split_8187D1E5_G_2);
                float _Distance_5207624D_Out_2;
                Unity_Distance_float2(_Vector2_BA8118B5_Out_0, float2(0, 0), _Distance_5207624D_Out_2);
                float _Multiply_59276D84_Out_2;
                Unity_Multiply_float(_Distance_5207624D_Out_2, 1.6, _Multiply_59276D84_Out_2);
                float _Clamp_FD12B1F1_Out_3;
                Unity_Clamp_float(_Multiply_59276D84_Out_2, 0, 1, _Clamp_FD12B1F1_Out_3);
                float _Subtract_748FB75_Out_2;
                Unity_Subtract_float(1, _Clamp_FD12B1F1_Out_3, _Subtract_748FB75_Out_2);
                float3 _Vector3_52D06542_Out_0 = float3(1, _Subtract_748FB75_Out_2, 0);
                float _Absolute_8271CB58_Out_1;
                Unity_Absolute_float(_Clamp_FD12B1F1_Out_3, _Absolute_8271CB58_Out_1);
                float _Power_E844E46B_Out_2;
                Unity_Power_float(_Absolute_8271CB58_Out_1, 0.16, _Power_E844E46B_Out_2);
                float _Clamp_E954B347_Out_3;
                Unity_Clamp_float(_Power_E844E46B_Out_2, 0, 1, _Clamp_E954B347_Out_3);
                float3 _Lerp_EAA7552E_Out_3;
                Unity_Lerp_float3(_Vector3_9D2D4CCD_Out_0, _Vector3_52D06542_Out_0, (_Clamp_E954B347_Out_3.xxx), _Lerp_EAA7552E_Out_3);
                float3 _Vector3_F6367D0E_Out_0 = float3(0, _Subtract_748FB75_Out_2, 1);
                float3 _Lerp_D4023798_Out_3;
                Unity_Lerp_float3(_Vector3_9D2D4CCD_Out_0, _Vector3_F6367D0E_Out_0, (_Clamp_E954B347_Out_3.xxx), _Lerp_D4023798_Out_3);
                float3 _Branch_C12262C0_Out_3;
                Unity_Branch_float3(_Comparison_1DC9309_Out_2, _Lerp_EAA7552E_Out_3, _Lerp_D4023798_Out_3, _Branch_C12262C0_Out_3);
                float3 _Multiply_6A6F4939_Out_2;
                Unity_Multiply_float((_SampleTexture2DLOD_8992BD21_A_8.xxx), _Branch_C12262C0_Out_3, _Multiply_6A6F4939_Out_2);
                surface.Color = _Multiply_6A6F4939_Out_2;
                surface.Alpha = 1;
                surface.AlphaClipThreshold = 0.5;
                return surface;
            }
        
            // --------------------------------------------------
            // Structs and Packing
        
            // Generated Type: Attributes
            struct Attributes
            {
                float3 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float4 tangentOS : TANGENT;
                float4 uv0 : TEXCOORD0;
                float4 uv1 : TEXCOORD1;
                float4 uv2 : TEXCOORD2;
                float4 uv3 : TEXCOORD3;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : INSTANCEID_SEMANTIC;
                #endif
            };
        
            // Generated Type: Varyings
            struct Varyings
            {
                float4 positionCS : SV_Position;
                float4 texCoord0;
                float4 texCoord1;
                float4 texCoord2;
                float4 texCoord3;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                #endif
            };
            
            // Generated Type: PackedVaryings
            struct PackedVaryings
            {
                float4 positionCS : SV_Position;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                #endif
                float4 interp00 : TEXCOORD0;
                float4 interp01 : TEXCOORD1;
                float4 interp02 : TEXCOORD2;
                float4 interp03 : TEXCOORD3;
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                #endif
            };
            
            // Packed Type: Varyings
            PackedVaryings PackVaryings(Varyings input)
            {
                PackedVaryings output;
                output.positionCS = input.positionCS;
                output.interp00.xyzw = input.texCoord0;
                output.interp01.xyzw = input.texCoord1;
                output.interp02.xyzw = input.texCoord2;
                output.interp03.xyzw = input.texCoord3;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                output.cullFace = input.cullFace;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                #endif
                return output;
            }
            
            // Unpacked Type: Varyings
            Varyings UnpackVaryings(PackedVaryings input)
            {
                Varyings output;
                output.positionCS = input.positionCS;
                output.texCoord0 = input.interp00.xyzw;
                output.texCoord1 = input.interp01.xyzw;
                output.texCoord2 = input.interp02.xyzw;
                output.texCoord3 = input.interp03.xyzw;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                output.cullFace = input.cullFace;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                #endif
                return output;
            }
        
            // --------------------------------------------------
            // Build Graph Inputs
        
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
            {
                SurfaceDescriptionInputs output;
                ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
            
                output.uv0 =                         input.texCoord0;
                output.uv1 =                         input.texCoord1;
                output.uv2 =                         input.texCoord2;
                output.uv3 =                         input.texCoord3;
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
            #else
            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
            #endif
            #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
            
                return output;
            }
            
        
            // --------------------------------------------------
            // Main
        
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/UnlitPass.hlsl"
        
            ENDHLSL
        }
        
        Pass
        {
            Name "ShadowCaster"
            Tags 
            { 
                "LightMode" = "ShadowCaster"
            }
           
            // Render State
            Blend One Zero, One Zero
            Cull Back
            ZTest LEqual
            ZWrite On
            // ColorMask: <None>
            
        
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
        
            // Debug
            // <None>
        
            // --------------------------------------------------
            // Pass
        
            // Pragmas
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0
            #pragma multi_compile_instancing
        
            // Keywords
            #pragma shader_feature _ _SMOOTHNESS_TEXTURE_ALBEDO_CHANNEL_A
            // GraphKeywords: <None>
            
            // Defines
            #define _AlphaClip 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define SHADERPASS_SHADOWCASTER
        
            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        
            // --------------------------------------------------
            // Graph
        
            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
            float2 _TextureSize;
            float _RotateUV;
            float Boolean_F2B1E91A;
            CBUFFER_END
            TEXTURE2D(_Direction); SAMPLER(sampler_Direction); float4 _Direction_TexelSize;
        
            // Graph Functions
            // GraphFunctions: <None>
        
            // Graph Vertex
            // GraphVertex: <None>
            
            // Graph Pixel
            struct SurfaceDescriptionInputs
            {
            };
            
            struct SurfaceDescription
            {
                float Alpha;
                float AlphaClipThreshold;
            };
            
            SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
            {
                SurfaceDescription surface = (SurfaceDescription)0;
                surface.Alpha = 1;
                surface.AlphaClipThreshold = 0.5;
                return surface;
            }
        
            // --------------------------------------------------
            // Structs and Packing
        
            // Generated Type: Attributes
            struct Attributes
            {
                float3 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float4 tangentOS : TANGENT;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : INSTANCEID_SEMANTIC;
                #endif
            };
        
            // Generated Type: Varyings
            struct Varyings
            {
                float4 positionCS : SV_Position;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                #endif
            };
            
            // Generated Type: PackedVaryings
            struct PackedVaryings
            {
                float4 positionCS : SV_Position;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                #endif
            };
            
            // Packed Type: Varyings
            PackedVaryings PackVaryings(Varyings input)
            {
                PackedVaryings output;
                output.positionCS = input.positionCS;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                output.cullFace = input.cullFace;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                #endif
                return output;
            }
            
            // Unpacked Type: Varyings
            Varyings UnpackVaryings(PackedVaryings input)
            {
                Varyings output;
                output.positionCS = input.positionCS;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                output.cullFace = input.cullFace;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                #endif
                return output;
            }
        
            // --------------------------------------------------
            // Build Graph Inputs
        
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
            {
                SurfaceDescriptionInputs output;
                ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
            
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
            #else
            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
            #endif
            #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
            
                return output;
            }
            
        
            // --------------------------------------------------
            // Main
        
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShadowCasterPass.hlsl"
        
            ENDHLSL
        }
        
        Pass
        {
            Name "DepthOnly"
            Tags 
            { 
                "LightMode" = "DepthOnly"
            }
           
            // Render State
            Blend One Zero, One Zero
            Cull Back
            ZTest LEqual
            ZWrite On
            ColorMask 0
            
        
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag
        
            // Debug
            // <None>
        
            // --------------------------------------------------
            // Pass
        
            // Pragmas
            #pragma prefer_hlslcc gles
            #pragma exclude_renderers d3d11_9x
            #pragma target 2.0
            #pragma multi_compile_instancing
        
            // Keywords
            // PassKeywords: <None>
            // GraphKeywords: <None>
            
            // Defines
            #define _AlphaClip 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define SHADERPASS_DEPTHONLY
        
            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
        
            // --------------------------------------------------
            // Graph
        
            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
            float2 _TextureSize;
            float _RotateUV;
            float Boolean_F2B1E91A;
            CBUFFER_END
            TEXTURE2D(_Direction); SAMPLER(sampler_Direction); float4 _Direction_TexelSize;
        
            // Graph Functions
            // GraphFunctions: <None>
        
            // Graph Vertex
            // GraphVertex: <None>
            
            // Graph Pixel
            struct SurfaceDescriptionInputs
            {
            };
            
            struct SurfaceDescription
            {
                float Alpha;
                float AlphaClipThreshold;
            };
            
            SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
            {
                SurfaceDescription surface = (SurfaceDescription)0;
                surface.Alpha = 1;
                surface.AlphaClipThreshold = 0.5;
                return surface;
            }
        
            // --------------------------------------------------
            // Structs and Packing
        
            // Generated Type: Attributes
            struct Attributes
            {
                float3 positionOS : POSITION;
                float3 normalOS : NORMAL;
                float4 tangentOS : TANGENT;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : INSTANCEID_SEMANTIC;
                #endif
            };
        
            // Generated Type: Varyings
            struct Varyings
            {
                float4 positionCS : SV_Position;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                #endif
            };
            
            // Generated Type: PackedVaryings
            struct PackedVaryings
            {
                float4 positionCS : SV_Position;
                #if UNITY_ANY_INSTANCING_ENABLED
                uint instanceID : CUSTOM_INSTANCE_ID;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
                #endif
            };
            
            // Packed Type: Varyings
            PackedVaryings PackVaryings(Varyings input)
            {
                PackedVaryings output;
                output.positionCS = input.positionCS;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                output.cullFace = input.cullFace;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                #endif
                return output;
            }
            
            // Unpacked Type: Varyings
            Varyings UnpackVaryings(PackedVaryings input)
            {
                Varyings output;
                output.positionCS = input.positionCS;
                #if UNITY_ANY_INSTANCING_ENABLED
                output.instanceID = input.instanceID;
                #endif
                #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
                output.cullFace = input.cullFace;
                #endif
                #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
                output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
                #endif
                #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
                output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
                #endif
                return output;
            }
        
            // --------------------------------------------------
            // Build Graph Inputs
        
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
            {
                SurfaceDescriptionInputs output;
                ZERO_INITIALIZE(SurfaceDescriptionInputs, output);
            
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN output.FaceSign =                    IS_FRONT_VFACE(input.cullFace, true, false);
            #else
            #define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
            #endif
            #undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
            
                return output;
            }
            
        
            // --------------------------------------------------
            // Main
        
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/DepthOnlyPass.hlsl"
        
            ENDHLSL
        }
        
    }
    FallBack "Hidden/Shader Graph/FallbackError"
}
