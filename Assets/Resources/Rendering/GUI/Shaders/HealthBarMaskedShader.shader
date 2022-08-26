Shader "HealthBarMaskedShader"
{
    Properties
    {
        [NoScaleOffset] _MainTex("MainTex", 2D) = "white" {}
        [HDR]_Color("Color", Color) = (0.003346536, 0.9911022, 0.08437622, 1)
        _Tilling("Tilling", Vector) = (1, 1, 0, 0)
        [NoScaleOffset]_Fill_Tex("Fill Tex", 2D) = "white" {}
        _Speed_Modifier("Speed Modifier", Float) = 4
        [HDR]_Border_Color("Border Color", Color) = (0.003346536, 0.9911022, 0.08437622, 1)
        _Border_Thickness("Border Thickness", Range(0, 0.1)) = 0.08
        [HideInInspector][NoScaleOffset]unity_Lightmaps("unity_Lightmaps", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_LightmapsInd("unity_LightmapsInd", 2DArray) = "" {}
        [HideInInspector][NoScaleOffset]unity_ShadowMasks("unity_ShadowMasks", 2DArray) = "" {}

        _Stencil("Stencil ID", Float) = 0
        _StencilComp("StencilComp", Float) = 8
        _StencilOp("StencilOp", Float) = 0
        _StencilReadMask("StencilReadMask", Float) = 255
        _StencilWriteMask("StencilWriteMask", Float) = 255
        _ColorMask("ColorMask", Float) = 15
    }
        SubShader
    {
        Tags
        {
            "RenderPipeline" = "UniversalPipeline"
            "RenderType" = "Transparent"
            "UniversalMaterialType" = "Lit"
            "Queue" = "Transparent"
            "ShaderGraphShader" = "true"
            "ShaderGraphTargetId" = ""
        }
        Pass
        {
            Name "Sprite Lit"
            Tags
            {
                "LightMode" = "Universal2D"
            }

        // Render State
        Cull Off
    Blend SrcAlpha OneMinusSrcAlpha, One OneMinusSrcAlpha
    ZTest[unity_GUIZTestMode]
    ZWrite Off

        // Debug
        // <None>

        // --------------------------------------------------
        // Pass

            Stencil{
                Ref[_Stencil]
                Comp[_StencilComp]
                Pass[_StencilOp]
                ReadMask[_StencilReadMask]
                WriteMask[_StencilWriteMask]
            }
            ColorMask[_ColorMask]
        HLSLPROGRAM

        // Pragmas
        #pragma target 2.0
    #pragma exclude_renderers d3d11_9x
    #pragma vertex vert
    #pragma fragment frag

        // DotsInstancingOptions: <None>
        // HybridV1InjectedBuiltinProperties: <None>

        // Keywords
        #pragma multi_compile _ USE_SHAPE_LIGHT_TYPE_0
    #pragma multi_compile _ USE_SHAPE_LIGHT_TYPE_1
    #pragma multi_compile _ USE_SHAPE_LIGHT_TYPE_2
    #pragma multi_compile _ USE_SHAPE_LIGHT_TYPE_3
    #pragma multi_compile _ DEBUG_DISPLAY
        // GraphKeywords: <None>

        // Defines
        #define _SURFACE_TYPE_TRANSPARENT 1
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define ATTRIBUTES_NEED_COLOR
        #define VARYINGS_NEED_POSITION_WS
        #define VARYINGS_NEED_TEXCOORD0
        #define VARYINGS_NEED_COLOR
        #define VARYINGS_NEED_SCREENPOSITION
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_SPRITELIT
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

        // Includes
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreInclude' */

        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/LightingUtility.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

        // --------------------------------------------------
        // Structs and Packing

        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */

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
         float3 positionWS;
         float4 texCoord0;
         float4 color;
         float4 screenPosition;
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
         float3 TimeParameters;
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
         float3 interp0 : INTERP0;
         float4 interp1 : INTERP1;
         float4 interp2 : INTERP2;
         float4 interp3 : INTERP3;
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

        PackedVaryings PackVaryings(Varyings input)
    {
        PackedVaryings output;
        ZERO_INITIALIZE(PackedVaryings, output);
        output.positionCS = input.positionCS;
        output.interp0.xyz = input.positionWS;
        output.interp1.xyzw = input.texCoord0;
        output.interp2.xyzw = input.color;
        output.interp3.xyzw = input.screenPosition;
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

    Varyings UnpackVaryings(PackedVaryings input)
    {
        Varyings output;
        output.positionCS = input.positionCS;
        output.positionWS = input.interp0.xyz;
        output.texCoord0 = input.interp1.xyzw;
        output.color = input.interp2.xyzw;
        output.screenPosition = input.interp3.xyzw;
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
float4 _MainTex_TexelSize;
float4 _Color;
float2 _Tilling;
float4 _Fill_Tex_TexelSize;
float _Speed_Modifier;
float4 _Border_Color;
float _Border_Thickness;
CBUFFER_END

// Object and Global properties
SAMPLER(SamplerState_Linear_Repeat);
TEXTURE2D(_MainTex);
SAMPLER(sampler_MainTex);
TEXTURE2D(_Fill_Tex);
SAMPLER(sampler_Fill_Tex);

// Graph Includes
// GraphIncludes: <None>

// -- Property used by ScenePickingPass
#ifdef SCENEPICKINGPASS
float4 _SelectionID;
#endif

// -- Properties used by SceneSelectionPass
#ifdef SCENESELECTIONPASS
int _ObjectId;
int _PassValue;
#endif

// Graph Functions

void Unity_Divide_float(float A, float B, out float Out)
{
    Out = A / B;
}

void Unity_Sine_float(float In, out float Out)
{
    Out = sin(In);
}

void Unity_Twirl_float(float2 UV, float2 Center, float Strength, float2 Offset, out float2 Out)
{
    float2 delta = UV - Center;
    float angle = Strength * length(delta);
    float x = cos(angle) * delta.x - sin(angle) * delta.y;
    float y = sin(angle) * delta.x + cos(angle) * delta.y;
    Out = float2(x + Center.x + Offset.x, y + Center.y + Offset.y);
}

void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
{
    Out = UV * Tiling + Offset;
}

void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
{
    Out = A * B;
}

void Unity_Add_float(float A, float B, out float Out)
{
    Out = A + B;
}

void Unity_Subtract_float(float A, float B, out float Out)
{
    Out = A - B;
}

void Unity_Add_float4(float4 A, float4 B, out float4 Out)
{
    Out = A + B;
}

/* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */

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

    #ifdef FEATURES_GRAPH_VERTEX
Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
{
return output;
}
#define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
#endif

// Graph Pixel
struct SurfaceDescription
{
    float3 BaseColor;
    float Alpha;
    float4 SpriteMask;
};

SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
{
    SurfaceDescription surface = (SurfaceDescription)0;
    UnityTexture2D _Property_4cb4ba5a13354711b23cf3fc242c46e3_Out_0 = UnityBuildTexture2DStructNoScale(_Fill_Tex);
    float _Property_867ed292f7194dfb8aa78de18bf02e81_Out_0 = _Speed_Modifier;
    float _Divide_19e7a7061c074e148a747e3a74a3a76b_Out_2;
    Unity_Divide_float(IN.TimeParameters.x, _Property_867ed292f7194dfb8aa78de18bf02e81_Out_0, _Divide_19e7a7061c074e148a747e3a74a3a76b_Out_2);
    float _Sine_5e26a3e9462e4dcfb0031a8126ab4e54_Out_1;
    Unity_Sine_float(_Divide_19e7a7061c074e148a747e3a74a3a76b_Out_2, _Sine_5e26a3e9462e4dcfb0031a8126ab4e54_Out_1);
    float2 _Twirl_b19bbc06b45d4ffba9508a1171608bbd_Out_4;
    Unity_Twirl_float(IN.uv0.xy, float2 (0.5, 0.5), _Sine_5e26a3e9462e4dcfb0031a8126ab4e54_Out_1, float2 (0, 0), _Twirl_b19bbc06b45d4ffba9508a1171608bbd_Out_4);
    float2 _Property_6f13c625372048f2a99e715e703dd767_Out_0 = _Tilling;
    float2 _Vector2_e05e47cff0084416bc4ba6052c679d6c_Out_0 = float2(_Divide_19e7a7061c074e148a747e3a74a3a76b_Out_2, 0);
    float2 _TilingAndOffset_840bcddcf1684decb0ecbd40f34b28f6_Out_3;
    Unity_TilingAndOffset_float(_Twirl_b19bbc06b45d4ffba9508a1171608bbd_Out_4, _Property_6f13c625372048f2a99e715e703dd767_Out_0, _Vector2_e05e47cff0084416bc4ba6052c679d6c_Out_0, _TilingAndOffset_840bcddcf1684decb0ecbd40f34b28f6_Out_3);
    float4 _SampleTexture2D_799351af0a764d2e93a5e1cf1faf0f67_RGBA_0 = SAMPLE_TEXTURE2D(_Property_4cb4ba5a13354711b23cf3fc242c46e3_Out_0.tex, _Property_4cb4ba5a13354711b23cf3fc242c46e3_Out_0.samplerstate, _Property_4cb4ba5a13354711b23cf3fc242c46e3_Out_0.GetTransformedUV(_TilingAndOffset_840bcddcf1684decb0ecbd40f34b28f6_Out_3));
    float _SampleTexture2D_799351af0a764d2e93a5e1cf1faf0f67_R_4 = _SampleTexture2D_799351af0a764d2e93a5e1cf1faf0f67_RGBA_0.r;
    float _SampleTexture2D_799351af0a764d2e93a5e1cf1faf0f67_G_5 = _SampleTexture2D_799351af0a764d2e93a5e1cf1faf0f67_RGBA_0.g;
    float _SampleTexture2D_799351af0a764d2e93a5e1cf1faf0f67_B_6 = _SampleTexture2D_799351af0a764d2e93a5e1cf1faf0f67_RGBA_0.b;
    float _SampleTexture2D_799351af0a764d2e93a5e1cf1faf0f67_A_7 = _SampleTexture2D_799351af0a764d2e93a5e1cf1faf0f67_RGBA_0.a;
    float4 _Property_1e1a68af1d9f44b9839a9c84c505cbc2_Out_0 = IsGammaSpace() ? LinearToSRGB(_Color) : _Color;
    float4 _Multiply_a773e2c9580b42f4b3f73aa07fc9e86a_Out_2;
    Unity_Multiply_float4_float4(_SampleTexture2D_799351af0a764d2e93a5e1cf1faf0f67_RGBA_0, _Property_1e1a68af1d9f44b9839a9c84c505cbc2_Out_0, _Multiply_a773e2c9580b42f4b3f73aa07fc9e86a_Out_2);
    UnityTexture2D _Property_0c637757ac674c3c91062e332580745b_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
    float4 _SampleTexture2D_b51b4a310aa34fc49bdc3f5fa4f7cf8d_RGBA_0 = SAMPLE_TEXTURE2D(_Property_0c637757ac674c3c91062e332580745b_Out_0.tex, _Property_0c637757ac674c3c91062e332580745b_Out_0.samplerstate, _Property_0c637757ac674c3c91062e332580745b_Out_0.GetTransformedUV(IN.uv0.xy));
    float _SampleTexture2D_b51b4a310aa34fc49bdc3f5fa4f7cf8d_R_4 = _SampleTexture2D_b51b4a310aa34fc49bdc3f5fa4f7cf8d_RGBA_0.r;
    float _SampleTexture2D_b51b4a310aa34fc49bdc3f5fa4f7cf8d_G_5 = _SampleTexture2D_b51b4a310aa34fc49bdc3f5fa4f7cf8d_RGBA_0.g;
    float _SampleTexture2D_b51b4a310aa34fc49bdc3f5fa4f7cf8d_B_6 = _SampleTexture2D_b51b4a310aa34fc49bdc3f5fa4f7cf8d_RGBA_0.b;
    float _SampleTexture2D_b51b4a310aa34fc49bdc3f5fa4f7cf8d_A_7 = _SampleTexture2D_b51b4a310aa34fc49bdc3f5fa4f7cf8d_RGBA_0.a;
    float _Property_b8027c0abfdc447caab82eef5443b3c4_Out_0 = _Border_Thickness;
    float _Add_3ad1be4b08824b09a7beb0333646c08e_Out_2;
    Unity_Add_float(_Property_b8027c0abfdc447caab82eef5443b3c4_Out_0, 1, _Add_3ad1be4b08824b09a7beb0333646c08e_Out_2);
    float _Divide_291340d611a74a1d972187c0a11b322e_Out_2;
    Unity_Divide_float(_Property_b8027c0abfdc447caab82eef5443b3c4_Out_0, -2, _Divide_291340d611a74a1d972187c0a11b322e_Out_2);
    float2 _TilingAndOffset_ff4716ad54d44b1bb269e66531e0c435_Out_3;
    Unity_TilingAndOffset_float(IN.uv0.xy, (_Add_3ad1be4b08824b09a7beb0333646c08e_Out_2.xx), (_Divide_291340d611a74a1d972187c0a11b322e_Out_2.xx), _TilingAndOffset_ff4716ad54d44b1bb269e66531e0c435_Out_3);
    float4 _SampleTexture2D_b1dd456a0aed4a3297c0fe18e5cfa833_RGBA_0 = SAMPLE_TEXTURE2D(_Property_0c637757ac674c3c91062e332580745b_Out_0.tex, _Property_0c637757ac674c3c91062e332580745b_Out_0.samplerstate, _Property_0c637757ac674c3c91062e332580745b_Out_0.GetTransformedUV(_TilingAndOffset_ff4716ad54d44b1bb269e66531e0c435_Out_3));
    float _SampleTexture2D_b1dd456a0aed4a3297c0fe18e5cfa833_R_4 = _SampleTexture2D_b1dd456a0aed4a3297c0fe18e5cfa833_RGBA_0.r;
    float _SampleTexture2D_b1dd456a0aed4a3297c0fe18e5cfa833_G_5 = _SampleTexture2D_b1dd456a0aed4a3297c0fe18e5cfa833_RGBA_0.g;
    float _SampleTexture2D_b1dd456a0aed4a3297c0fe18e5cfa833_B_6 = _SampleTexture2D_b1dd456a0aed4a3297c0fe18e5cfa833_RGBA_0.b;
    float _SampleTexture2D_b1dd456a0aed4a3297c0fe18e5cfa833_A_7 = _SampleTexture2D_b1dd456a0aed4a3297c0fe18e5cfa833_RGBA_0.a;
    float _Subtract_f1b0ba6b22fd423995cb67608087e950_Out_2;
    Unity_Subtract_float(_SampleTexture2D_b51b4a310aa34fc49bdc3f5fa4f7cf8d_A_7, _SampleTexture2D_b1dd456a0aed4a3297c0fe18e5cfa833_A_7, _Subtract_f1b0ba6b22fd423995cb67608087e950_Out_2);
    float4 _Property_1ea9bd2f0954453d8085632e5cc3073c_Out_0 = IsGammaSpace() ? LinearToSRGB(_Border_Color) : _Border_Color;
    float4 _Multiply_80378d73b5a44a899dfc137e1e758a2c_Out_2;
    Unity_Multiply_float4_float4((_Subtract_f1b0ba6b22fd423995cb67608087e950_Out_2.xxxx), _Property_1ea9bd2f0954453d8085632e5cc3073c_Out_0, _Multiply_80378d73b5a44a899dfc137e1e758a2c_Out_2);
    float4 _Add_3078c56f7ad446bd9c223ccad3653fe6_Out_2;
    Unity_Add_float4(_Multiply_a773e2c9580b42f4b3f73aa07fc9e86a_Out_2, _Multiply_80378d73b5a44a899dfc137e1e758a2c_Out_2, _Add_3078c56f7ad446bd9c223ccad3653fe6_Out_2);
    surface.BaseColor = (_Add_3078c56f7ad446bd9c223ccad3653fe6_Out_2.xyz);
    surface.Alpha = _SampleTexture2D_b51b4a310aa34fc49bdc3f5fa4f7cf8d_A_7;
    surface.SpriteMask = IsGammaSpace() ? float4(1, 1, 1, 1) : float4 (SRGBToLinear(float3(1, 1, 1)), 1);
    return surface;
}

// --------------------------------------------------
// Build Graph Inputs

VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
{
    VertexDescriptionInputs output;
    ZERO_INITIALIZE(VertexDescriptionInputs, output);

    output.ObjectSpaceNormal = input.normalOS;
    output.ObjectSpaceTangent = input.tangentOS.xyz;
    output.ObjectSpacePosition = input.positionOS;

    return output;
}
    SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
{
    SurfaceDescriptionInputs output;
    ZERO_INITIALIZE(SurfaceDescriptionInputs, output);







    output.uv0 = input.texCoord0;
    output.TimeParameters = _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN                output.FaceSign =                                   IS_FRONT_VFACE(input.cullFace, true, false);
#else
#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
#endif
#undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

    return output;
}

    // --------------------------------------------------
    // Main

    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
#include "Packages/com.unity.render-pipelines.universal/Editor/2D/ShaderGraph/Includes/SpriteLitPass.hlsl"

    ENDHLSL
}
Pass
{
    Name "Sprite Normal"
    Tags
    {
        "LightMode" = "NormalsRendering"
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
        #define VARYINGS_NEED_NORMAL_WS
        #define VARYINGS_NEED_TANGENT_WS
        #define VARYINGS_NEED_TEXCOORD0
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_SPRITENORMAL
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

        // Includes
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreInclude' */

        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/Shaders/2D/Include/NormalsRenderingShared.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

        // --------------------------------------------------
        // Structs and Packing

        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */

        struct Attributes
    {
         float3 positionOS : POSITION;
         float3 normalOS : NORMAL;
         float4 tangentOS : TANGENT;
         float4 uv0 : TEXCOORD0;
        #if UNITY_ANY_INSTANCING_ENABLED
         uint instanceID : INSTANCEID_SEMANTIC;
        #endif
    };
    struct Varyings
    {
         float4 positionCS : SV_POSITION;
         float3 normalWS;
         float4 tangentWS;
         float4 texCoord0;
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
         float3 TangentSpaceNormal;
         float4 uv0;
         float3 TimeParameters;
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
         float3 interp0 : INTERP0;
         float4 interp1 : INTERP1;
         float4 interp2 : INTERP2;
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

        PackedVaryings PackVaryings(Varyings input)
    {
        PackedVaryings output;
        ZERO_INITIALIZE(PackedVaryings, output);
        output.positionCS = input.positionCS;
        output.interp0.xyz = input.normalWS;
        output.interp1.xyzw = input.tangentWS;
        output.interp2.xyzw = input.texCoord0;
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

    Varyings UnpackVaryings(PackedVaryings input)
    {
        Varyings output;
        output.positionCS = input.positionCS;
        output.normalWS = input.interp0.xyz;
        output.tangentWS = input.interp1.xyzw;
        output.texCoord0 = input.interp2.xyzw;
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
float4 _MainTex_TexelSize;
float4 _Color;
float2 _Tilling;
float4 _Fill_Tex_TexelSize;
float _Speed_Modifier;
float4 _Border_Color;
float _Border_Thickness;
CBUFFER_END

// Object and Global properties
SAMPLER(SamplerState_Linear_Repeat);
TEXTURE2D(_MainTex);
SAMPLER(sampler_MainTex);
TEXTURE2D(_Fill_Tex);
SAMPLER(sampler_Fill_Tex);

// Graph Includes
// GraphIncludes: <None>

// -- Property used by ScenePickingPass
#ifdef SCENEPICKINGPASS
float4 _SelectionID;
#endif

// -- Properties used by SceneSelectionPass
#ifdef SCENESELECTIONPASS
int _ObjectId;
int _PassValue;
#endif

// Graph Functions

void Unity_Divide_float(float A, float B, out float Out)
{
    Out = A / B;
}

void Unity_Sine_float(float In, out float Out)
{
    Out = sin(In);
}

void Unity_Twirl_float(float2 UV, float2 Center, float Strength, float2 Offset, out float2 Out)
{
    float2 delta = UV - Center;
    float angle = Strength * length(delta);
    float x = cos(angle) * delta.x - sin(angle) * delta.y;
    float y = sin(angle) * delta.x + cos(angle) * delta.y;
    Out = float2(x + Center.x + Offset.x, y + Center.y + Offset.y);
}

void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
{
    Out = UV * Tiling + Offset;
}

void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
{
    Out = A * B;
}

void Unity_Add_float(float A, float B, out float Out)
{
    Out = A + B;
}

void Unity_Subtract_float(float A, float B, out float Out)
{
    Out = A - B;
}

void Unity_Add_float4(float4 A, float4 B, out float4 Out)
{
    Out = A + B;
}

/* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */

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

    #ifdef FEATURES_GRAPH_VERTEX
Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
{
return output;
}
#define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
#endif

// Graph Pixel
struct SurfaceDescription
{
    float3 BaseColor;
    float Alpha;
    float3 NormalTS;
};

SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
{
    SurfaceDescription surface = (SurfaceDescription)0;
    UnityTexture2D _Property_4cb4ba5a13354711b23cf3fc242c46e3_Out_0 = UnityBuildTexture2DStructNoScale(_Fill_Tex);
    float _Property_867ed292f7194dfb8aa78de18bf02e81_Out_0 = _Speed_Modifier;
    float _Divide_19e7a7061c074e148a747e3a74a3a76b_Out_2;
    Unity_Divide_float(IN.TimeParameters.x, _Property_867ed292f7194dfb8aa78de18bf02e81_Out_0, _Divide_19e7a7061c074e148a747e3a74a3a76b_Out_2);
    float _Sine_5e26a3e9462e4dcfb0031a8126ab4e54_Out_1;
    Unity_Sine_float(_Divide_19e7a7061c074e148a747e3a74a3a76b_Out_2, _Sine_5e26a3e9462e4dcfb0031a8126ab4e54_Out_1);
    float2 _Twirl_b19bbc06b45d4ffba9508a1171608bbd_Out_4;
    Unity_Twirl_float(IN.uv0.xy, float2 (0.5, 0.5), _Sine_5e26a3e9462e4dcfb0031a8126ab4e54_Out_1, float2 (0, 0), _Twirl_b19bbc06b45d4ffba9508a1171608bbd_Out_4);
    float2 _Property_6f13c625372048f2a99e715e703dd767_Out_0 = _Tilling;
    float2 _Vector2_e05e47cff0084416bc4ba6052c679d6c_Out_0 = float2(_Divide_19e7a7061c074e148a747e3a74a3a76b_Out_2, 0);
    float2 _TilingAndOffset_840bcddcf1684decb0ecbd40f34b28f6_Out_3;
    Unity_TilingAndOffset_float(_Twirl_b19bbc06b45d4ffba9508a1171608bbd_Out_4, _Property_6f13c625372048f2a99e715e703dd767_Out_0, _Vector2_e05e47cff0084416bc4ba6052c679d6c_Out_0, _TilingAndOffset_840bcddcf1684decb0ecbd40f34b28f6_Out_3);
    float4 _SampleTexture2D_799351af0a764d2e93a5e1cf1faf0f67_RGBA_0 = SAMPLE_TEXTURE2D(_Property_4cb4ba5a13354711b23cf3fc242c46e3_Out_0.tex, _Property_4cb4ba5a13354711b23cf3fc242c46e3_Out_0.samplerstate, _Property_4cb4ba5a13354711b23cf3fc242c46e3_Out_0.GetTransformedUV(_TilingAndOffset_840bcddcf1684decb0ecbd40f34b28f6_Out_3));
    float _SampleTexture2D_799351af0a764d2e93a5e1cf1faf0f67_R_4 = _SampleTexture2D_799351af0a764d2e93a5e1cf1faf0f67_RGBA_0.r;
    float _SampleTexture2D_799351af0a764d2e93a5e1cf1faf0f67_G_5 = _SampleTexture2D_799351af0a764d2e93a5e1cf1faf0f67_RGBA_0.g;
    float _SampleTexture2D_799351af0a764d2e93a5e1cf1faf0f67_B_6 = _SampleTexture2D_799351af0a764d2e93a5e1cf1faf0f67_RGBA_0.b;
    float _SampleTexture2D_799351af0a764d2e93a5e1cf1faf0f67_A_7 = _SampleTexture2D_799351af0a764d2e93a5e1cf1faf0f67_RGBA_0.a;
    float4 _Property_1e1a68af1d9f44b9839a9c84c505cbc2_Out_0 = IsGammaSpace() ? LinearToSRGB(_Color) : _Color;
    float4 _Multiply_a773e2c9580b42f4b3f73aa07fc9e86a_Out_2;
    Unity_Multiply_float4_float4(_SampleTexture2D_799351af0a764d2e93a5e1cf1faf0f67_RGBA_0, _Property_1e1a68af1d9f44b9839a9c84c505cbc2_Out_0, _Multiply_a773e2c9580b42f4b3f73aa07fc9e86a_Out_2);
    UnityTexture2D _Property_0c637757ac674c3c91062e332580745b_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
    float4 _SampleTexture2D_b51b4a310aa34fc49bdc3f5fa4f7cf8d_RGBA_0 = SAMPLE_TEXTURE2D(_Property_0c637757ac674c3c91062e332580745b_Out_0.tex, _Property_0c637757ac674c3c91062e332580745b_Out_0.samplerstate, _Property_0c637757ac674c3c91062e332580745b_Out_0.GetTransformedUV(IN.uv0.xy));
    float _SampleTexture2D_b51b4a310aa34fc49bdc3f5fa4f7cf8d_R_4 = _SampleTexture2D_b51b4a310aa34fc49bdc3f5fa4f7cf8d_RGBA_0.r;
    float _SampleTexture2D_b51b4a310aa34fc49bdc3f5fa4f7cf8d_G_5 = _SampleTexture2D_b51b4a310aa34fc49bdc3f5fa4f7cf8d_RGBA_0.g;
    float _SampleTexture2D_b51b4a310aa34fc49bdc3f5fa4f7cf8d_B_6 = _SampleTexture2D_b51b4a310aa34fc49bdc3f5fa4f7cf8d_RGBA_0.b;
    float _SampleTexture2D_b51b4a310aa34fc49bdc3f5fa4f7cf8d_A_7 = _SampleTexture2D_b51b4a310aa34fc49bdc3f5fa4f7cf8d_RGBA_0.a;
    float _Property_b8027c0abfdc447caab82eef5443b3c4_Out_0 = _Border_Thickness;
    float _Add_3ad1be4b08824b09a7beb0333646c08e_Out_2;
    Unity_Add_float(_Property_b8027c0abfdc447caab82eef5443b3c4_Out_0, 1, _Add_3ad1be4b08824b09a7beb0333646c08e_Out_2);
    float _Divide_291340d611a74a1d972187c0a11b322e_Out_2;
    Unity_Divide_float(_Property_b8027c0abfdc447caab82eef5443b3c4_Out_0, -2, _Divide_291340d611a74a1d972187c0a11b322e_Out_2);
    float2 _TilingAndOffset_ff4716ad54d44b1bb269e66531e0c435_Out_3;
    Unity_TilingAndOffset_float(IN.uv0.xy, (_Add_3ad1be4b08824b09a7beb0333646c08e_Out_2.xx), (_Divide_291340d611a74a1d972187c0a11b322e_Out_2.xx), _TilingAndOffset_ff4716ad54d44b1bb269e66531e0c435_Out_3);
    float4 _SampleTexture2D_b1dd456a0aed4a3297c0fe18e5cfa833_RGBA_0 = SAMPLE_TEXTURE2D(_Property_0c637757ac674c3c91062e332580745b_Out_0.tex, _Property_0c637757ac674c3c91062e332580745b_Out_0.samplerstate, _Property_0c637757ac674c3c91062e332580745b_Out_0.GetTransformedUV(_TilingAndOffset_ff4716ad54d44b1bb269e66531e0c435_Out_3));
    float _SampleTexture2D_b1dd456a0aed4a3297c0fe18e5cfa833_R_4 = _SampleTexture2D_b1dd456a0aed4a3297c0fe18e5cfa833_RGBA_0.r;
    float _SampleTexture2D_b1dd456a0aed4a3297c0fe18e5cfa833_G_5 = _SampleTexture2D_b1dd456a0aed4a3297c0fe18e5cfa833_RGBA_0.g;
    float _SampleTexture2D_b1dd456a0aed4a3297c0fe18e5cfa833_B_6 = _SampleTexture2D_b1dd456a0aed4a3297c0fe18e5cfa833_RGBA_0.b;
    float _SampleTexture2D_b1dd456a0aed4a3297c0fe18e5cfa833_A_7 = _SampleTexture2D_b1dd456a0aed4a3297c0fe18e5cfa833_RGBA_0.a;
    float _Subtract_f1b0ba6b22fd423995cb67608087e950_Out_2;
    Unity_Subtract_float(_SampleTexture2D_b51b4a310aa34fc49bdc3f5fa4f7cf8d_A_7, _SampleTexture2D_b1dd456a0aed4a3297c0fe18e5cfa833_A_7, _Subtract_f1b0ba6b22fd423995cb67608087e950_Out_2);
    float4 _Property_1ea9bd2f0954453d8085632e5cc3073c_Out_0 = IsGammaSpace() ? LinearToSRGB(_Border_Color) : _Border_Color;
    float4 _Multiply_80378d73b5a44a899dfc137e1e758a2c_Out_2;
    Unity_Multiply_float4_float4((_Subtract_f1b0ba6b22fd423995cb67608087e950_Out_2.xxxx), _Property_1ea9bd2f0954453d8085632e5cc3073c_Out_0, _Multiply_80378d73b5a44a899dfc137e1e758a2c_Out_2);
    float4 _Add_3078c56f7ad446bd9c223ccad3653fe6_Out_2;
    Unity_Add_float4(_Multiply_a773e2c9580b42f4b3f73aa07fc9e86a_Out_2, _Multiply_80378d73b5a44a899dfc137e1e758a2c_Out_2, _Add_3078c56f7ad446bd9c223ccad3653fe6_Out_2);
    surface.BaseColor = (_Add_3078c56f7ad446bd9c223ccad3653fe6_Out_2.xyz);
    surface.Alpha = _SampleTexture2D_b51b4a310aa34fc49bdc3f5fa4f7cf8d_A_7;
    surface.NormalTS = IN.TangentSpaceNormal;
    return surface;
}

// --------------------------------------------------
// Build Graph Inputs

VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
{
    VertexDescriptionInputs output;
    ZERO_INITIALIZE(VertexDescriptionInputs, output);

    output.ObjectSpaceNormal = input.normalOS;
    output.ObjectSpaceTangent = input.tangentOS.xyz;
    output.ObjectSpacePosition = input.positionOS;

    return output;
}
    SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
{
    SurfaceDescriptionInputs output;
    ZERO_INITIALIZE(SurfaceDescriptionInputs, output);





    output.TangentSpaceNormal = float3(0.0f, 0.0f, 1.0f);


    output.uv0 = input.texCoord0;
    output.TimeParameters = _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN                output.FaceSign =                                   IS_FRONT_VFACE(input.cullFace, true, false);
#else
#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
#endif
#undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

    return output;
}

    // --------------------------------------------------
    // Main

    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
#include "Packages/com.unity.render-pipelines.universal/Editor/2D/ShaderGraph/Includes/SpriteNormalPass.hlsl"

    ENDHLSL
}
Pass
{
    Name "SceneSelectionPass"
    Tags
    {
        "LightMode" = "SceneSelectionPass"
    }

        // Render State
        Cull Off

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
        #define VARYINGS_NEED_TEXCOORD0
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_DEPTHONLY
    #define SCENESELECTIONPASS 1

        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

        // Includes
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreInclude' */

        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

        // --------------------------------------------------
        // Structs and Packing

        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */

        struct Attributes
    {
         float3 positionOS : POSITION;
         float3 normalOS : NORMAL;
         float4 tangentOS : TANGENT;
         float4 uv0 : TEXCOORD0;
        #if UNITY_ANY_INSTANCING_ENABLED
         uint instanceID : INSTANCEID_SEMANTIC;
        #endif
    };
    struct Varyings
    {
         float4 positionCS : SV_POSITION;
         float4 texCoord0;
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
         float4 interp0 : INTERP0;
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

        PackedVaryings PackVaryings(Varyings input)
    {
        PackedVaryings output;
        ZERO_INITIALIZE(PackedVaryings, output);
        output.positionCS = input.positionCS;
        output.interp0.xyzw = input.texCoord0;
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

    Varyings UnpackVaryings(PackedVaryings input)
    {
        Varyings output;
        output.positionCS = input.positionCS;
        output.texCoord0 = input.interp0.xyzw;
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
float4 _MainTex_TexelSize;
float4 _Color;
float2 _Tilling;
float4 _Fill_Tex_TexelSize;
float _Speed_Modifier;
float4 _Border_Color;
float _Border_Thickness;
CBUFFER_END

// Object and Global properties
SAMPLER(SamplerState_Linear_Repeat);
TEXTURE2D(_MainTex);
SAMPLER(sampler_MainTex);
TEXTURE2D(_Fill_Tex);
SAMPLER(sampler_Fill_Tex);

// Graph Includes
// GraphIncludes: <None>

// -- Property used by ScenePickingPass
#ifdef SCENEPICKINGPASS
float4 _SelectionID;
#endif

// -- Properties used by SceneSelectionPass
#ifdef SCENESELECTIONPASS
int _ObjectId;
int _PassValue;
#endif

// Graph Functions
// GraphFunctions: <None>

/* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */

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

    #ifdef FEATURES_GRAPH_VERTEX
Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
{
return output;
}
#define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
#endif

// Graph Pixel
struct SurfaceDescription
{
    float Alpha;
};

SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
{
    SurfaceDescription surface = (SurfaceDescription)0;
    UnityTexture2D _Property_0c637757ac674c3c91062e332580745b_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
    float4 _SampleTexture2D_b51b4a310aa34fc49bdc3f5fa4f7cf8d_RGBA_0 = SAMPLE_TEXTURE2D(_Property_0c637757ac674c3c91062e332580745b_Out_0.tex, _Property_0c637757ac674c3c91062e332580745b_Out_0.samplerstate, _Property_0c637757ac674c3c91062e332580745b_Out_0.GetTransformedUV(IN.uv0.xy));
    float _SampleTexture2D_b51b4a310aa34fc49bdc3f5fa4f7cf8d_R_4 = _SampleTexture2D_b51b4a310aa34fc49bdc3f5fa4f7cf8d_RGBA_0.r;
    float _SampleTexture2D_b51b4a310aa34fc49bdc3f5fa4f7cf8d_G_5 = _SampleTexture2D_b51b4a310aa34fc49bdc3f5fa4f7cf8d_RGBA_0.g;
    float _SampleTexture2D_b51b4a310aa34fc49bdc3f5fa4f7cf8d_B_6 = _SampleTexture2D_b51b4a310aa34fc49bdc3f5fa4f7cf8d_RGBA_0.b;
    float _SampleTexture2D_b51b4a310aa34fc49bdc3f5fa4f7cf8d_A_7 = _SampleTexture2D_b51b4a310aa34fc49bdc3f5fa4f7cf8d_RGBA_0.a;
    surface.Alpha = _SampleTexture2D_b51b4a310aa34fc49bdc3f5fa4f7cf8d_A_7;
    return surface;
}

// --------------------------------------------------
// Build Graph Inputs

VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
{
    VertexDescriptionInputs output;
    ZERO_INITIALIZE(VertexDescriptionInputs, output);

    output.ObjectSpaceNormal = input.normalOS;
    output.ObjectSpaceTangent = input.tangentOS.xyz;
    output.ObjectSpacePosition = input.positionOS;

    return output;
}
    SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
{
    SurfaceDescriptionInputs output;
    ZERO_INITIALIZE(SurfaceDescriptionInputs, output);







    output.uv0 = input.texCoord0;
#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN                output.FaceSign =                                   IS_FRONT_VFACE(input.cullFace, true, false);
#else
#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
#endif
#undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

    return output;
}

    // --------------------------------------------------
    // Main

    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SelectionPickingPass.hlsl"

    ENDHLSL
}
Pass
{
    Name "ScenePickingPass"
    Tags
    {
        "LightMode" = "Picking"
    }

        // Render State
        Cull Back

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
        #define VARYINGS_NEED_TEXCOORD0
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_DEPTHONLY
    #define SCENEPICKINGPASS 1

        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

        // Includes
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreInclude' */

        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

        // --------------------------------------------------
        // Structs and Packing

        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */

        struct Attributes
    {
         float3 positionOS : POSITION;
         float3 normalOS : NORMAL;
         float4 tangentOS : TANGENT;
         float4 uv0 : TEXCOORD0;
        #if UNITY_ANY_INSTANCING_ENABLED
         uint instanceID : INSTANCEID_SEMANTIC;
        #endif
    };
    struct Varyings
    {
         float4 positionCS : SV_POSITION;
         float4 texCoord0;
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
         float4 interp0 : INTERP0;
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

        PackedVaryings PackVaryings(Varyings input)
    {
        PackedVaryings output;
        ZERO_INITIALIZE(PackedVaryings, output);
        output.positionCS = input.positionCS;
        output.interp0.xyzw = input.texCoord0;
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

    Varyings UnpackVaryings(PackedVaryings input)
    {
        Varyings output;
        output.positionCS = input.positionCS;
        output.texCoord0 = input.interp0.xyzw;
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
float4 _MainTex_TexelSize;
float4 _Color;
float2 _Tilling;
float4 _Fill_Tex_TexelSize;
float _Speed_Modifier;
float4 _Border_Color;
float _Border_Thickness;
CBUFFER_END

// Object and Global properties
SAMPLER(SamplerState_Linear_Repeat);
TEXTURE2D(_MainTex);
SAMPLER(sampler_MainTex);
TEXTURE2D(_Fill_Tex);
SAMPLER(sampler_Fill_Tex);

// Graph Includes
// GraphIncludes: <None>

// -- Property used by ScenePickingPass
#ifdef SCENEPICKINGPASS
float4 _SelectionID;
#endif

// -- Properties used by SceneSelectionPass
#ifdef SCENESELECTIONPASS
int _ObjectId;
int _PassValue;
#endif

// Graph Functions
// GraphFunctions: <None>

/* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */

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

    #ifdef FEATURES_GRAPH_VERTEX
Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
{
return output;
}
#define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
#endif

// Graph Pixel
struct SurfaceDescription
{
    float Alpha;
};

SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
{
    SurfaceDescription surface = (SurfaceDescription)0;
    UnityTexture2D _Property_0c637757ac674c3c91062e332580745b_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
    float4 _SampleTexture2D_b51b4a310aa34fc49bdc3f5fa4f7cf8d_RGBA_0 = SAMPLE_TEXTURE2D(_Property_0c637757ac674c3c91062e332580745b_Out_0.tex, _Property_0c637757ac674c3c91062e332580745b_Out_0.samplerstate, _Property_0c637757ac674c3c91062e332580745b_Out_0.GetTransformedUV(IN.uv0.xy));
    float _SampleTexture2D_b51b4a310aa34fc49bdc3f5fa4f7cf8d_R_4 = _SampleTexture2D_b51b4a310aa34fc49bdc3f5fa4f7cf8d_RGBA_0.r;
    float _SampleTexture2D_b51b4a310aa34fc49bdc3f5fa4f7cf8d_G_5 = _SampleTexture2D_b51b4a310aa34fc49bdc3f5fa4f7cf8d_RGBA_0.g;
    float _SampleTexture2D_b51b4a310aa34fc49bdc3f5fa4f7cf8d_B_6 = _SampleTexture2D_b51b4a310aa34fc49bdc3f5fa4f7cf8d_RGBA_0.b;
    float _SampleTexture2D_b51b4a310aa34fc49bdc3f5fa4f7cf8d_A_7 = _SampleTexture2D_b51b4a310aa34fc49bdc3f5fa4f7cf8d_RGBA_0.a;
    surface.Alpha = _SampleTexture2D_b51b4a310aa34fc49bdc3f5fa4f7cf8d_A_7;
    return surface;
}

// --------------------------------------------------
// Build Graph Inputs

VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
{
    VertexDescriptionInputs output;
    ZERO_INITIALIZE(VertexDescriptionInputs, output);

    output.ObjectSpaceNormal = input.normalOS;
    output.ObjectSpaceTangent = input.tangentOS.xyz;
    output.ObjectSpacePosition = input.positionOS;

    return output;
}
    SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
{
    SurfaceDescriptionInputs output;
    ZERO_INITIALIZE(SurfaceDescriptionInputs, output);







    output.uv0 = input.texCoord0;
#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN                output.FaceSign =                                   IS_FRONT_VFACE(input.cullFace, true, false);
#else
#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
#endif
#undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

    return output;
}

    // --------------------------------------------------
    // Main

    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
#include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/SelectionPickingPass.hlsl"

    ENDHLSL
}
Pass
{
    Name "Sprite Forward"
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
        #pragma multi_compile _ DEBUG_DISPLAY
        // GraphKeywords: <None>

        // Defines
        #define _SURFACE_TYPE_TRANSPARENT 1
        #define ATTRIBUTES_NEED_NORMAL
        #define ATTRIBUTES_NEED_TANGENT
        #define ATTRIBUTES_NEED_TEXCOORD0
        #define ATTRIBUTES_NEED_COLOR
        #define VARYINGS_NEED_POSITION_WS
        #define VARYINGS_NEED_TEXCOORD0
        #define VARYINGS_NEED_COLOR
        #define FEATURES_GRAPH_VERTEX
        /* WARNING: $splice Could not find named fragment 'PassInstancing' */
        #define SHADERPASS SHADERPASS_SPRITEFORWARD
        /* WARNING: $splice Could not find named fragment 'DotsInstancingVars' */

        // Includes
        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreInclude' */

        #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Texture.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
    #include "Packages/com.unity.render-pipelines.core/ShaderLibrary/TextureStack.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/ShaderPass.hlsl"

        // --------------------------------------------------
        // Structs and Packing

        /* WARNING: $splice Could not find named fragment 'CustomInterpolatorPrePacking' */

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
         float3 positionWS;
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
         float3 TangentSpaceNormal;
         float4 uv0;
         float3 TimeParameters;
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
         float3 interp0 : INTERP0;
         float4 interp1 : INTERP1;
         float4 interp2 : INTERP2;
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

        PackedVaryings PackVaryings(Varyings input)
    {
        PackedVaryings output;
        ZERO_INITIALIZE(PackedVaryings, output);
        output.positionCS = input.positionCS;
        output.interp0.xyz = input.positionWS;
        output.interp1.xyzw = input.texCoord0;
        output.interp2.xyzw = input.color;
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

    Varyings UnpackVaryings(PackedVaryings input)
    {
        Varyings output;
        output.positionCS = input.positionCS;
        output.positionWS = input.interp0.xyz;
        output.texCoord0 = input.interp1.xyzw;
        output.color = input.interp2.xyzw;
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
float4 _MainTex_TexelSize;
float4 _Color;
float2 _Tilling;
float4 _Fill_Tex_TexelSize;
float _Speed_Modifier;
float4 _Border_Color;
float _Border_Thickness;
CBUFFER_END

// Object and Global properties
SAMPLER(SamplerState_Linear_Repeat);
TEXTURE2D(_MainTex);
SAMPLER(sampler_MainTex);
TEXTURE2D(_Fill_Tex);
SAMPLER(sampler_Fill_Tex);

// Graph Includes
// GraphIncludes: <None>

// -- Property used by ScenePickingPass
#ifdef SCENEPICKINGPASS
float4 _SelectionID;
#endif

// -- Properties used by SceneSelectionPass
#ifdef SCENESELECTIONPASS
int _ObjectId;
int _PassValue;
#endif

// Graph Functions

void Unity_Divide_float(float A, float B, out float Out)
{
    Out = A / B;
}

void Unity_Sine_float(float In, out float Out)
{
    Out = sin(In);
}

void Unity_Twirl_float(float2 UV, float2 Center, float Strength, float2 Offset, out float2 Out)
{
    float2 delta = UV - Center;
    float angle = Strength * length(delta);
    float x = cos(angle) * delta.x - sin(angle) * delta.y;
    float y = sin(angle) * delta.x + cos(angle) * delta.y;
    Out = float2(x + Center.x + Offset.x, y + Center.y + Offset.y);
}

void Unity_TilingAndOffset_float(float2 UV, float2 Tiling, float2 Offset, out float2 Out)
{
    Out = UV * Tiling + Offset;
}

void Unity_Multiply_float4_float4(float4 A, float4 B, out float4 Out)
{
    Out = A * B;
}

void Unity_Add_float(float A, float B, out float Out)
{
    Out = A + B;
}

void Unity_Subtract_float(float A, float B, out float Out)
{
    Out = A - B;
}

void Unity_Add_float4(float4 A, float4 B, out float4 Out)
{
    Out = A + B;
}

/* WARNING: $splice Could not find named fragment 'CustomInterpolatorPreVertex' */

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

    #ifdef FEATURES_GRAPH_VERTEX
Varyings CustomInterpolatorPassThroughFunc(inout Varyings output, VertexDescription input)
{
return output;
}
#define CUSTOMINTERPOLATOR_VARYPASSTHROUGH_FUNC
#endif

// Graph Pixel
struct SurfaceDescription
{
    float3 BaseColor;
    float Alpha;
    float3 NormalTS;
};

SurfaceDescription SurfaceDescriptionFunction(SurfaceDescriptionInputs IN)
{
    SurfaceDescription surface = (SurfaceDescription)0;
    UnityTexture2D _Property_4cb4ba5a13354711b23cf3fc242c46e3_Out_0 = UnityBuildTexture2DStructNoScale(_Fill_Tex);
    float _Property_867ed292f7194dfb8aa78de18bf02e81_Out_0 = _Speed_Modifier;
    float _Divide_19e7a7061c074e148a747e3a74a3a76b_Out_2;
    Unity_Divide_float(IN.TimeParameters.x, _Property_867ed292f7194dfb8aa78de18bf02e81_Out_0, _Divide_19e7a7061c074e148a747e3a74a3a76b_Out_2);
    float _Sine_5e26a3e9462e4dcfb0031a8126ab4e54_Out_1;
    Unity_Sine_float(_Divide_19e7a7061c074e148a747e3a74a3a76b_Out_2, _Sine_5e26a3e9462e4dcfb0031a8126ab4e54_Out_1);
    float2 _Twirl_b19bbc06b45d4ffba9508a1171608bbd_Out_4;
    Unity_Twirl_float(IN.uv0.xy, float2 (0.5, 0.5), _Sine_5e26a3e9462e4dcfb0031a8126ab4e54_Out_1, float2 (0, 0), _Twirl_b19bbc06b45d4ffba9508a1171608bbd_Out_4);
    float2 _Property_6f13c625372048f2a99e715e703dd767_Out_0 = _Tilling;
    float2 _Vector2_e05e47cff0084416bc4ba6052c679d6c_Out_0 = float2(_Divide_19e7a7061c074e148a747e3a74a3a76b_Out_2, 0);
    float2 _TilingAndOffset_840bcddcf1684decb0ecbd40f34b28f6_Out_3;
    Unity_TilingAndOffset_float(_Twirl_b19bbc06b45d4ffba9508a1171608bbd_Out_4, _Property_6f13c625372048f2a99e715e703dd767_Out_0, _Vector2_e05e47cff0084416bc4ba6052c679d6c_Out_0, _TilingAndOffset_840bcddcf1684decb0ecbd40f34b28f6_Out_3);
    float4 _SampleTexture2D_799351af0a764d2e93a5e1cf1faf0f67_RGBA_0 = SAMPLE_TEXTURE2D(_Property_4cb4ba5a13354711b23cf3fc242c46e3_Out_0.tex, _Property_4cb4ba5a13354711b23cf3fc242c46e3_Out_0.samplerstate, _Property_4cb4ba5a13354711b23cf3fc242c46e3_Out_0.GetTransformedUV(_TilingAndOffset_840bcddcf1684decb0ecbd40f34b28f6_Out_3));
    float _SampleTexture2D_799351af0a764d2e93a5e1cf1faf0f67_R_4 = _SampleTexture2D_799351af0a764d2e93a5e1cf1faf0f67_RGBA_0.r;
    float _SampleTexture2D_799351af0a764d2e93a5e1cf1faf0f67_G_5 = _SampleTexture2D_799351af0a764d2e93a5e1cf1faf0f67_RGBA_0.g;
    float _SampleTexture2D_799351af0a764d2e93a5e1cf1faf0f67_B_6 = _SampleTexture2D_799351af0a764d2e93a5e1cf1faf0f67_RGBA_0.b;
    float _SampleTexture2D_799351af0a764d2e93a5e1cf1faf0f67_A_7 = _SampleTexture2D_799351af0a764d2e93a5e1cf1faf0f67_RGBA_0.a;
    float4 _Property_1e1a68af1d9f44b9839a9c84c505cbc2_Out_0 = IsGammaSpace() ? LinearToSRGB(_Color) : _Color;
    float4 _Multiply_a773e2c9580b42f4b3f73aa07fc9e86a_Out_2;
    Unity_Multiply_float4_float4(_SampleTexture2D_799351af0a764d2e93a5e1cf1faf0f67_RGBA_0, _Property_1e1a68af1d9f44b9839a9c84c505cbc2_Out_0, _Multiply_a773e2c9580b42f4b3f73aa07fc9e86a_Out_2);
    UnityTexture2D _Property_0c637757ac674c3c91062e332580745b_Out_0 = UnityBuildTexture2DStructNoScale(_MainTex);
    float4 _SampleTexture2D_b51b4a310aa34fc49bdc3f5fa4f7cf8d_RGBA_0 = SAMPLE_TEXTURE2D(_Property_0c637757ac674c3c91062e332580745b_Out_0.tex, _Property_0c637757ac674c3c91062e332580745b_Out_0.samplerstate, _Property_0c637757ac674c3c91062e332580745b_Out_0.GetTransformedUV(IN.uv0.xy));
    float _SampleTexture2D_b51b4a310aa34fc49bdc3f5fa4f7cf8d_R_4 = _SampleTexture2D_b51b4a310aa34fc49bdc3f5fa4f7cf8d_RGBA_0.r;
    float _SampleTexture2D_b51b4a310aa34fc49bdc3f5fa4f7cf8d_G_5 = _SampleTexture2D_b51b4a310aa34fc49bdc3f5fa4f7cf8d_RGBA_0.g;
    float _SampleTexture2D_b51b4a310aa34fc49bdc3f5fa4f7cf8d_B_6 = _SampleTexture2D_b51b4a310aa34fc49bdc3f5fa4f7cf8d_RGBA_0.b;
    float _SampleTexture2D_b51b4a310aa34fc49bdc3f5fa4f7cf8d_A_7 = _SampleTexture2D_b51b4a310aa34fc49bdc3f5fa4f7cf8d_RGBA_0.a;
    float _Property_b8027c0abfdc447caab82eef5443b3c4_Out_0 = _Border_Thickness;
    float _Add_3ad1be4b08824b09a7beb0333646c08e_Out_2;
    Unity_Add_float(_Property_b8027c0abfdc447caab82eef5443b3c4_Out_0, 1, _Add_3ad1be4b08824b09a7beb0333646c08e_Out_2);
    float _Divide_291340d611a74a1d972187c0a11b322e_Out_2;
    Unity_Divide_float(_Property_b8027c0abfdc447caab82eef5443b3c4_Out_0, -2, _Divide_291340d611a74a1d972187c0a11b322e_Out_2);
    float2 _TilingAndOffset_ff4716ad54d44b1bb269e66531e0c435_Out_3;
    Unity_TilingAndOffset_float(IN.uv0.xy, (_Add_3ad1be4b08824b09a7beb0333646c08e_Out_2.xx), (_Divide_291340d611a74a1d972187c0a11b322e_Out_2.xx), _TilingAndOffset_ff4716ad54d44b1bb269e66531e0c435_Out_3);
    float4 _SampleTexture2D_b1dd456a0aed4a3297c0fe18e5cfa833_RGBA_0 = SAMPLE_TEXTURE2D(_Property_0c637757ac674c3c91062e332580745b_Out_0.tex, _Property_0c637757ac674c3c91062e332580745b_Out_0.samplerstate, _Property_0c637757ac674c3c91062e332580745b_Out_0.GetTransformedUV(_TilingAndOffset_ff4716ad54d44b1bb269e66531e0c435_Out_3));
    float _SampleTexture2D_b1dd456a0aed4a3297c0fe18e5cfa833_R_4 = _SampleTexture2D_b1dd456a0aed4a3297c0fe18e5cfa833_RGBA_0.r;
    float _SampleTexture2D_b1dd456a0aed4a3297c0fe18e5cfa833_G_5 = _SampleTexture2D_b1dd456a0aed4a3297c0fe18e5cfa833_RGBA_0.g;
    float _SampleTexture2D_b1dd456a0aed4a3297c0fe18e5cfa833_B_6 = _SampleTexture2D_b1dd456a0aed4a3297c0fe18e5cfa833_RGBA_0.b;
    float _SampleTexture2D_b1dd456a0aed4a3297c0fe18e5cfa833_A_7 = _SampleTexture2D_b1dd456a0aed4a3297c0fe18e5cfa833_RGBA_0.a;
    float _Subtract_f1b0ba6b22fd423995cb67608087e950_Out_2;
    Unity_Subtract_float(_SampleTexture2D_b51b4a310aa34fc49bdc3f5fa4f7cf8d_A_7, _SampleTexture2D_b1dd456a0aed4a3297c0fe18e5cfa833_A_7, _Subtract_f1b0ba6b22fd423995cb67608087e950_Out_2);
    float4 _Property_1ea9bd2f0954453d8085632e5cc3073c_Out_0 = IsGammaSpace() ? LinearToSRGB(_Border_Color) : _Border_Color;
    float4 _Multiply_80378d73b5a44a899dfc137e1e758a2c_Out_2;
    Unity_Multiply_float4_float4((_Subtract_f1b0ba6b22fd423995cb67608087e950_Out_2.xxxx), _Property_1ea9bd2f0954453d8085632e5cc3073c_Out_0, _Multiply_80378d73b5a44a899dfc137e1e758a2c_Out_2);
    float4 _Add_3078c56f7ad446bd9c223ccad3653fe6_Out_2;
    Unity_Add_float4(_Multiply_a773e2c9580b42f4b3f73aa07fc9e86a_Out_2, _Multiply_80378d73b5a44a899dfc137e1e758a2c_Out_2, _Add_3078c56f7ad446bd9c223ccad3653fe6_Out_2);
    surface.BaseColor = (_Add_3078c56f7ad446bd9c223ccad3653fe6_Out_2.xyz);
    surface.Alpha = _SampleTexture2D_b51b4a310aa34fc49bdc3f5fa4f7cf8d_A_7;
    surface.NormalTS = IN.TangentSpaceNormal;
    return surface;
}

// --------------------------------------------------
// Build Graph Inputs

VertexDescriptionInputs BuildVertexDescriptionInputs(Attributes input)
{
    VertexDescriptionInputs output;
    ZERO_INITIALIZE(VertexDescriptionInputs, output);

    output.ObjectSpaceNormal = input.normalOS;
    output.ObjectSpaceTangent = input.tangentOS.xyz;
    output.ObjectSpacePosition = input.positionOS;

    return output;
}
    SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
{
    SurfaceDescriptionInputs output;
    ZERO_INITIALIZE(SurfaceDescriptionInputs, output);





    output.TangentSpaceNormal = float3(0.0f, 0.0f, 1.0f);


    output.uv0 = input.texCoord0;
    output.TimeParameters = _TimeParameters.xyz; // This is mainly for LW as HD overwrite this value
#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN                output.FaceSign =                                   IS_FRONT_VFACE(input.cullFace, true, false);
#else
#define BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN
#endif
#undef BUILD_SURFACE_DESCRIPTION_INPUTS_OUTPUT_FACESIGN

    return output;
}

    // --------------------------------------------------
    // Main

    #include "Packages/com.unity.render-pipelines.universal/Editor/ShaderGraph/Includes/Varyings.hlsl"
#include "Packages/com.unity.render-pipelines.universal/Editor/2D/ShaderGraph/Includes/SpriteForwardPass.hlsl"

    ENDHLSL
}
    }
        CustomEditor "UnityEditor.ShaderGraph.GenericShaderGraphMaterialGUI"
        FallBack "Hidden/Shader Graph/FallbackError"
}