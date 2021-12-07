Shader "Custom UI Shader"
{
    Properties
    {
        Colour("Colour", Color) = (1, 0, 0, 0)
        [NoScaleOffset]UITexture("UITexture", 2D) = "white" {}
        [NoScaleOffset]UITexture_1("UITexture (1)", 2D) = "white" {}
        Intensity("Intensity", Vector) = (10, 0, 0, 0)
        [HideInInspector][NoScaleOffset]unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}
    }
    SubShader
    {
        Tags
        {
            "RenderPipeline"="UniversalPipeline"
            "RenderType"="Transparent"
            "UniversalMaterialType" = "Unlit"
            "Queue"="Transparent"
        }
        
        // Render State
        Blend SrcAlpha OneMinusSrcAlpha
        ZTest[unity_GUIZTestMode]
        ZWrite Off
        Cull Off
        Lighting Off
        
        Pass
        {
            Name "Sprite Unlit"
            Tags
            {
                "LightMode" = "Universal2D"
            }

            

            // Debug
            // <None>

            // --------------------------------------------------
            // Pass

            HLSLPROGRAM

            // Pragmas
            #pragma target 2.0
        #pragma exclude_renderers d3d11_9x
        #pragma vertex vert
        #pragma fragment frag

            // DotsInstancingOptions: <None>
            // HybridV1InjectedBuiltinProperties: <None>

            // Keywords
            // PassKeywords: <None>
            // GraphKeywords: <None>

            // Defines
            #define _SURFACE_TYPE_TRANSPARENT 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define ATTRIBUTES_NEED_COLOR
            #define VARYINGS_NEED_TEXCOORD0
            #define VARYINGS_NEED_COLOR
            #define FEATURES_GRAPH_VERTEX
            /* WARNING: $splice Could not find named fragment 'PassInstancing' */
            #define SHADERPASS SHADERPASS_SPRITEUNLIT
            /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

            // --------------------------------------------------
            // Structs and Packing

            struct Attributes
        {
            float3 positionOS : POSITION;
            float3 normalOS : NORMAL;
            float4 tangentOS : TANGENT;
            float4 uv0 : TEXCOORD0;
            float4 color : COLOR;
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
            float4 positionCS : SV_POSITION;
            float4 texCoord0;
            float4 color;
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
            float4 uv0;
        };
        struct VertexDescriptionInputs
        {
            float3 ObjectSpaceNormal;
            float3 ObjectSpaceTangent;
            float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
            float4 positionCS : SV_POSITION;
            float4 interp0 : TEXCOORD0;
            float4 interp1 : TEXCOORD1;
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };

            PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            output.positionCS = input.positionCS;
            output.interp0.xyzw =  input.texCoord0;
            output.interp1.xyzw =  input.color;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.interp0.xyzw;
            output.color = input.interp1.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }

            // --------------------------------------------------
            // Graph

            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
        float4 Colour;
        float4 UITexture_TexelSize;
        float4 UITexture_1_TexelSize;
        float4 Intensity;
        CBUFFER_END

        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(UITexture);
        SAMPLER(samplerUITexture);
        TEXTURE2D(UITexture_1);
        SAMPLER(samplerUITexture_1);

            // Graph Functions
            
        void Unity_InvertColors_float(float In, float InvertColors, out float Out)
        {
            Out = abs(InvertColors - In);
        }

        void Unity_Multiply_float(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }

        void Unity_Add_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A + B;
        }

            // Graph Vertex
            struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };

        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }

            // Graph Pixel
            struct SurfaceDescription
        {
            float3 BaseColor;
            float Alpha;
        };

        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            UnityTexture2D _Property_ab6c8914a1a74f33b8c5fc88bf74f501_Out_0 = UnityBuildTexture2DStructNoScale(UITexture);
            float4 _SampleTexture2D_2c4acb462928408eae84f24c10555e35_RGBA_0 = SAMPLE_TEXTURE2D(_Property_ab6c8914a1a74f33b8c5fc88bf74f501_Out_0.tex, _Property_ab6c8914a1a74f33b8c5fc88bf74f501_Out_0.samplerstate, IN.uv0.xy);
            float _SampleTexture2D_2c4acb462928408eae84f24c10555e35_R_4 = _SampleTexture2D_2c4acb462928408eae84f24c10555e35_RGBA_0.r;
            float _SampleTexture2D_2c4acb462928408eae84f24c10555e35_G_5 = _SampleTexture2D_2c4acb462928408eae84f24c10555e35_RGBA_0.g;
            float _SampleTexture2D_2c4acb462928408eae84f24c10555e35_B_6 = _SampleTexture2D_2c4acb462928408eae84f24c10555e35_RGBA_0.b;
            float _SampleTexture2D_2c4acb462928408eae84f24c10555e35_A_7 = _SampleTexture2D_2c4acb462928408eae84f24c10555e35_RGBA_0.a;
            float _InvertColors_03b46b8db41c4f59990fd099e63dd84a_Out_1;
            float _InvertColors_03b46b8db41c4f59990fd099e63dd84a_InvertColors = float (1
        );    Unity_InvertColors_float(_SampleTexture2D_2c4acb462928408eae84f24c10555e35_R_4, _InvertColors_03b46b8db41c4f59990fd099e63dd84a_InvertColors, _InvertColors_03b46b8db41c4f59990fd099e63dd84a_Out_1);
            float4 _Property_7668ba53463847208aea3fd4b8991120_Out_0 = Intensity;
            UnityTexture2D _Property_959b054e5cef4acd87bb7b10a4b9ef6d_Out_0 = UnityBuildTexture2DStructNoScale(UITexture_1);
            float4 _SampleTexture2D_21f29a0996844ef98d4ef5a020c52dc5_RGBA_0 = SAMPLE_TEXTURE2D(_Property_959b054e5cef4acd87bb7b10a4b9ef6d_Out_0.tex, _Property_959b054e5cef4acd87bb7b10a4b9ef6d_Out_0.samplerstate, IN.uv0.xy);
            float _SampleTexture2D_21f29a0996844ef98d4ef5a020c52dc5_R_4 = _SampleTexture2D_21f29a0996844ef98d4ef5a020c52dc5_RGBA_0.r;
            float _SampleTexture2D_21f29a0996844ef98d4ef5a020c52dc5_G_5 = _SampleTexture2D_21f29a0996844ef98d4ef5a020c52dc5_RGBA_0.g;
            float _SampleTexture2D_21f29a0996844ef98d4ef5a020c52dc5_B_6 = _SampleTexture2D_21f29a0996844ef98d4ef5a020c52dc5_RGBA_0.b;
            float _SampleTexture2D_21f29a0996844ef98d4ef5a020c52dc5_A_7 = _SampleTexture2D_21f29a0996844ef98d4ef5a020c52dc5_RGBA_0.a;
            float4 _Property_f0e2961d13264dfeb9ee0546eba6e05c_Out_0 = Colour;
            float4 _Multiply_903fa9a55c414d0294e5a322ca075d13_Out_2;
            Unity_Multiply_float((_SampleTexture2D_21f29a0996844ef98d4ef5a020c52dc5_A_7.xxxx), _Property_f0e2961d13264dfeb9ee0546eba6e05c_Out_0, _Multiply_903fa9a55c414d0294e5a322ca075d13_Out_2);
            float4 _Multiply_9371071f52ca42dab5d912f8755ecd6b_Out_2;
            Unity_Multiply_float(_Property_7668ba53463847208aea3fd4b8991120_Out_0, _Multiply_903fa9a55c414d0294e5a322ca075d13_Out_2, _Multiply_9371071f52ca42dab5d912f8755ecd6b_Out_2);
            float4 _Add_226d79294ae244dbbbbc21aabd2e6f9e_Out_2;
            Unity_Add_float4((_InvertColors_03b46b8db41c4f59990fd099e63dd84a_Out_1.xxxx), _Multiply_9371071f52ca42dab5d912f8755ecd6b_Out_2, _Add_226d79294ae244dbbbbc21aabd2e6f9e_Out_2);
            float _Split_89cde499cc9b40d09aff2178eb4b653a_R_1 = _Add_226d79294ae244dbbbbc21aabd2e6f9e_Out_2[0];
            float _Split_89cde499cc9b40d09aff2178eb4b653a_G_2 = _Add_226d79294ae244dbbbbc21aabd2e6f9e_Out_2[1];
            float _Split_89cde499cc9b40d09aff2178eb4b653a_B_3 = _Add_226d79294ae244dbbbbc21aabd2e6f9e_Out_2[2];
            float _Split_89cde499cc9b40d09aff2178eb4b653a_A_4 = _Add_226d79294ae244dbbbbc21aabd2e6f9e_Out_2[3];
            float _InvertColors_fa56984b960b4695886b4bb5f082b179_Out_1;
            float _InvertColors_fa56984b960b4695886b4bb5f082b179_InvertColors = float (0
        );    Unity_InvertColors_float(_Split_89cde499cc9b40d09aff2178eb4b653a_R_1, _InvertColors_fa56984b960b4695886b4bb5f082b179_InvertColors, _InvertColors_fa56984b960b4695886b4bb5f082b179_Out_1);
            surface.BaseColor = (_Add_226d79294ae244dbbbbc21aabd2e6f9e_Out_2.xyz);
            surface.Alpha = _InvertColors_fa56984b960b4695886b4bb5f082b179_Out_1;
            return surface;
        }

            // --------------------------------------------------
            // Build Graph Inputs

            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);

            output.ObjectSpaceNormal =           input.normalOS;
            output.ObjectSpaceTangent =          input.tangentOS.xyz;
            output.ObjectSpacePosition =         input.positionOS;

            return output;
        }
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);





            output.uv0 =                         input.texCoord0;
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

            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SpriteUnlitPass.hlsl"

            ENDHLSL
        }
        Pass
        {
            Name "Sprite Unlit"
            Tags
            {
                "LightMode" = "UniversalForward"
            }

            // Render State
            Cull Off
        Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
        ZTest LEqual
        ZWrite Off

            // Debug
            // <None>

            // --------------------------------------------------
            // Pass

            HLSLPROGRAM

            // Pragmas
            #pragma target 2.0
        #pragma exclude_renderers d3d11_9x
        #pragma vertex vert
        #pragma fragment frag

            // DotsInstancingOptions: <None>
            // HybridV1InjectedBuiltinProperties: <None>

            // Keywords
            // PassKeywords: <None>
            // GraphKeywords: <None>

            // Defines
            #define _SURFACE_TYPE_TRANSPARENT 1
            #define ATTRIBUTES_NEED_NORMAL
            #define ATTRIBUTES_NEED_TANGENT
            #define ATTRIBUTES_NEED_TEXCOORD0
            #define ATTRIBUTES_NEED_COLOR
            #define VARYINGS_NEED_TEXCOORD0
            #define VARYINGS_NEED_COLOR
            #define FEATURES_GRAPH_VERTEX
            /* WARNING: $splice Could not find named fragment 'PassInstancing' */
            #define SHADERPASS SHADERPASS_SPRITEFORWARD
            /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

            // Includes
            #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"

            // --------------------------------------------------
            // Structs and Packing

            struct Attributes
        {
            float3 positionOS : POSITION;
            float3 normalOS : NORMAL;
            float4 tangentOS : TANGENT;
            float4 uv0 : TEXCOORD0;
            float4 color : COLOR;
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : INSTANCEID_SEMANTIC;
            #endif
        };
        struct Varyings
        {
            float4 positionCS : SV_POSITION;
            float4 texCoord0;
            float4 color;
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };
        struct SurfaceDescriptionInputs
        {
            float4 uv0;
        };
        struct VertexDescriptionInputs
        {
            float3 ObjectSpaceNormal;
            float3 ObjectSpaceTangent;
            float3 ObjectSpacePosition;
        };
        struct PackedVaryings
        {
            float4 positionCS : SV_POSITION;
            float4 interp0 : TEXCOORD0;
            float4 interp1 : TEXCOORD1;
            #if UNITY_ANY_INSTANCING_ENABLED
            uint instanceID : CUSTOM_INSTANCE_ID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            uint stereoTargetEyeIndexAsBlendIdx0 : BLENDINDICES0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            uint stereoTargetEyeIndexAsRTArrayIdx : SV_RenderTargetArrayIndex;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            FRONT_FACE_TYPE cullFace : FRONT_FACE_SEMANTIC;
            #endif
        };

            PackedVaryings PackVaryings (Varyings input)
        {
            PackedVaryings output;
            output.positionCS = input.positionCS;
            output.interp0.xyzw =  input.texCoord0;
            output.interp1.xyzw =  input.color;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }
        Varyings UnpackVaryings (PackedVaryings input)
        {
            Varyings output;
            output.positionCS = input.positionCS;
            output.texCoord0 = input.interp0.xyzw;
            output.color = input.interp1.xyzw;
            #if UNITY_ANY_INSTANCING_ENABLED
            output.instanceID = input.instanceID;
            #endif
            #if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
            output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
            #endif
            #if (defined(UNITY_STEREO_INSTANCING_ENABLED))
            output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
            #endif
            #if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
            output.cullFace = input.cullFace;
            #endif
            return output;
        }

            // --------------------------------------------------
            // Graph

            // Graph Properties
            CBUFFER_START(UnityPerMaterial)
        float4 Colour;
        float4 UITexture_TexelSize;
        float4 UITexture_1_TexelSize;
        float4 Intensity;
        CBUFFER_END

        // Object and Global properties
        SAMPLER(SamplerState_Linear_Repeat);
        TEXTURE2D(UITexture);
        SAMPLER(samplerUITexture);
        TEXTURE2D(UITexture_1);
        SAMPLER(samplerUITexture_1);

            // Graph Functions
            
        void Unity_InvertColors_float(float In, float InvertColors, out float Out)
        {
            Out = abs(InvertColors - In);
        }

        void Unity_Multiply_float(float4 A, float4 B, out float4 Out)
        {
            Out = A * B;
        }

        void Unity_Add_float4(float4 A, float4 B, out float4 Out)
        {
            Out = A + B;
        }

            // Graph Vertex
            struct VertexDescription
        {
            float3 Position;
            float3 Normal;
            float3 Tangent;
        };

        VertexDescription VertexDescriptionFunction(VertexDescriptionInputs IN)
        {
            VertexDescription description = (VertexDescription)0;
            description.Position = IN.ObjectSpacePosition;
            description.Normal = IN.ObjectSpaceNormal;
            description.Tangent = IN.ObjectSpaceTangent;
            return description;
        }

            // Graph Pixel
            struct SurfaceDescription
        {
            float3 BaseColor;
            float Alpha;
        };

        SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
        {
            SurfaceDescription surface = (SurfaceDescription)0;
            UnityTexture2D _Property_ab6c8914a1a74f33b8c5fc88bf74f501_Out_0 = UnityBuildTexture2DStructNoScale(UITexture);
            float4 _SampleTexture2D_2c4acb462928408eae84f24c10555e35_RGBA_0 = SAMPLE_TEXTURE2D(_Property_ab6c8914a1a74f33b8c5fc88bf74f501_Out_0.tex, _Property_ab6c8914a1a74f33b8c5fc88bf74f501_Out_0.samplerstate, IN.uv0.xy);
            float _SampleTexture2D_2c4acb462928408eae84f24c10555e35_R_4 = _SampleTexture2D_2c4acb462928408eae84f24c10555e35_RGBA_0.r;
            float _SampleTexture2D_2c4acb462928408eae84f24c10555e35_G_5 = _SampleTexture2D_2c4acb462928408eae84f24c10555e35_RGBA_0.g;
            float _SampleTexture2D_2c4acb462928408eae84f24c10555e35_B_6 = _SampleTexture2D_2c4acb462928408eae84f24c10555e35_RGBA_0.b;
            float _SampleTexture2D_2c4acb462928408eae84f24c10555e35_A_7 = _SampleTexture2D_2c4acb462928408eae84f24c10555e35_RGBA_0.a;
            float _InvertColors_03b46b8db41c4f59990fd099e63dd84a_Out_1;
            float _InvertColors_03b46b8db41c4f59990fd099e63dd84a_InvertColors = float (1
        );    Unity_InvertColors_float(_SampleTexture2D_2c4acb462928408eae84f24c10555e35_R_4, _InvertColors_03b46b8db41c4f59990fd099e63dd84a_InvertColors, _InvertColors_03b46b8db41c4f59990fd099e63dd84a_Out_1);
            float4 _Property_7668ba53463847208aea3fd4b8991120_Out_0 = Intensity;
            UnityTexture2D _Property_959b054e5cef4acd87bb7b10a4b9ef6d_Out_0 = UnityBuildTexture2DStructNoScale(UITexture_1);
            float4 _SampleTexture2D_21f29a0996844ef98d4ef5a020c52dc5_RGBA_0 = SAMPLE_TEXTURE2D(_Property_959b054e5cef4acd87bb7b10a4b9ef6d_Out_0.tex, _Property_959b054e5cef4acd87bb7b10a4b9ef6d_Out_0.samplerstate, IN.uv0.xy);
            float _SampleTexture2D_21f29a0996844ef98d4ef5a020c52dc5_R_4 = _SampleTexture2D_21f29a0996844ef98d4ef5a020c52dc5_RGBA_0.r;
            float _SampleTexture2D_21f29a0996844ef98d4ef5a020c52dc5_G_5 = _SampleTexture2D_21f29a0996844ef98d4ef5a020c52dc5_RGBA_0.g;
            float _SampleTexture2D_21f29a0996844ef98d4ef5a020c52dc5_B_6 = _SampleTexture2D_21f29a0996844ef98d4ef5a020c52dc5_RGBA_0.b;
            float _SampleTexture2D_21f29a0996844ef98d4ef5a020c52dc5_A_7 = _SampleTexture2D_21f29a0996844ef98d4ef5a020c52dc5_RGBA_0.a;
            float4 _Property_f0e2961d13264dfeb9ee0546eba6e05c_Out_0 = Colour;
            float4 _Multiply_903fa9a55c414d0294e5a322ca075d13_Out_2;
            Unity_Multiply_float((_SampleTexture2D_21f29a0996844ef98d4ef5a020c52dc5_A_7.xxxx), _Property_f0e2961d13264dfeb9ee0546eba6e05c_Out_0, _Multiply_903fa9a55c414d0294e5a322ca075d13_Out_2);
            float4 _Multiply_9371071f52ca42dab5d912f8755ecd6b_Out_2;
            Unity_Multiply_float(_Property_7668ba53463847208aea3fd4b8991120_Out_0, _Multiply_903fa9a55c414d0294e5a322ca075d13_Out_2, _Multiply_9371071f52ca42dab5d912f8755ecd6b_Out_2);
            float4 _Add_226d79294ae244dbbbbc21aabd2e6f9e_Out_2;
            Unity_Add_float4((_InvertColors_03b46b8db41c4f59990fd099e63dd84a_Out_1.xxxx), _Multiply_9371071f52ca42dab5d912f8755ecd6b_Out_2, _Add_226d79294ae244dbbbbc21aabd2e6f9e_Out_2);
            float _Split_89cde499cc9b40d09aff2178eb4b653a_R_1 = _Add_226d79294ae244dbbbbc21aabd2e6f9e_Out_2[0];
            float _Split_89cde499cc9b40d09aff2178eb4b653a_G_2 = _Add_226d79294ae244dbbbbc21aabd2e6f9e_Out_2[1];
            float _Split_89cde499cc9b40d09aff2178eb4b653a_B_3 = _Add_226d79294ae244dbbbbc21aabd2e6f9e_Out_2[2];
            float _Split_89cde499cc9b40d09aff2178eb4b653a_A_4 = _Add_226d79294ae244dbbbbc21aabd2e6f9e_Out_2[3];
            float _InvertColors_fa56984b960b4695886b4bb5f082b179_Out_1;
            float _InvertColors_fa56984b960b4695886b4bb5f082b179_InvertColors = float (0
        );    Unity_InvertColors_float(_Split_89cde499cc9b40d09aff2178eb4b653a_R_1, _InvertColors_fa56984b960b4695886b4bb5f082b179_InvertColors, _InvertColors_fa56984b960b4695886b4bb5f082b179_Out_1);
            surface.BaseColor = (_Add_226d79294ae244dbbbbc21aabd2e6f9e_Out_2.xyz);
            surface.Alpha = _InvertColors_fa56984b960b4695886b4bb5f082b179_Out_1;
            return surface;
        }

            // --------------------------------------------------
            // Build Graph Inputs

            VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
        {
            VertexDescriptionInputs output;
            ZERO_INITIALIZE(VertexDescriptionInputs, output);

            output.ObjectSpaceNormal =           input.normalOS;
            output.ObjectSpaceTangent =          input.tangentOS.xyz;
            output.ObjectSpacePosition =         input.positionOS;

            return output;
        }
            SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
        {
            SurfaceDescriptionInputs output;
            ZERO_INITIALIZE(SurfaceDescriptionInputs, output);





            output.uv0 =                         input.texCoord0;
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

            #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
        #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SpriteUnlitPass.hlsl"

            ENDHLSL
        }
    }
    FallBack "Hidden/Shader Graph/FallbackError"
}