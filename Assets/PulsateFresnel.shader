﻿Shader "Jared/PulsateFresnel"
{
	Properties
	{
		Inner_Color("InnerColor", Color) = (0.01585142, 0, 0.5660378, 1)
		Outer_Color("OuterColor", Color) = (0.2703364, 0.5593228, 0.7075472, 0)
		_MainTex("Texture", 2D) = "white" {}
	_Color("Main Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_Intensity("Intensity", Range(0, 1)) = 0.01
		_Frequency("Frequency", Range(0, 100)) = 20
	}
		SubShader
	{
		Tags
	{
		"RenderPipeline" = "UniversalPipeline"
		"RenderType" = "Opaque"
		"Queue" = "Geometry+0"
	}

		CGINCLUDE
#include "UnityCG.cginc"

#pragma multi_compile_fog

		struct appdata
	{
		float4 vertex : POSITION;
		float3 normal : NORMAL;
		float2 uv : TEXCOORD0;
	};

	struct v2f
	{
		float2 uv : TEXCOORD0;
		float4 vertex : SV_POSITION;
		float3 normal : NORMAL;
		float4 color : COLOR;
		UNITY_FOG_COORDS(1)
	};

	sampler2D _MainTex;
	float4 _MainTex_ST;
	float _Intensity;
	float _Frequency;
	float4 _LightColor0;

	v2f vert(appdata v)
	{
		v2f o;
		float3 lightDirection = normalize(_WorldSpaceLightPos0.xyz);
		o.vertex = UnityObjectToClipPos(v.vertex);
		o.normal = normalize(mul(float4(v.normal, 0.0), unity_ObjectToWorld).xyz);
		o.uv = TRANSFORM_TEX(v.uv, _MainTex);

		float4 newVertex = mul(v.vertex, unity_ObjectToWorld) + _Intensity * ((float4(o.normal, 0.0) * sin(o.uv.x * _Frequency + _Time.w)) + (float4(o.normal, 0.0) * sin(o.uv.y * _Frequency + _Time.w)));
		newVertex = mul(newVertex, unity_WorldToObject);
		o.vertex = UnityObjectToClipPos(newVertex);
		o.normal = normalize(newVertex);
		float3 diffuse = _LightColor0.rgb * max(0.0, dot(o.normal, lightDirection));
		o.color = float4(diffuse, 1.0);
		UNITY_TRANSFER_FOG(o, o.vertex);

		return o;
	}
	ENDCG

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

		CGPROGRAM
#pragma vertex vert
#pragma fragment frag

		half4 frag(v2f i) : COLOR{
		fixed4 col = tex2D(_MainTex, i.uv);// * i.color;
	UNITY_APPLY_FOG(i.fogCoord, col);
	return col;
	}
		ENDCG

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
#pragma multi_compile_fog
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
#define VARYINGS_NEED_NORMAL_WS
#define VARYINGS_NEED_VIEWDIRECTION_WS
#define SHADERPASS_UNLIT

		// Includes
#include "Packages/com.unity.render-pipelines.core/ShaderLibrary/Color.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Lighting.hlsl"
#include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/ShaderGraphFunctions.hlsl"
#include "Packages/com.unity.shadergraph/ShaderGraphLibrary/ShaderVariablesFunctions.hlsl"

		// --------------------------------------------------
		// Graph

		// Graph Properties
		CBUFFER_START(UnityPerMaterial)
		float4 Inner_Color;
	float4 Outer_Color;
	CBUFFER_END

		// Graph Functions

		void Unity_FresnelEffect_float(float3 Normal, float3 ViewDir, float Power, out float Out)
	{
		Out = pow((1.0 - saturate(dot(normalize(Normal), normalize(ViewDir)))), Power);
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
	// GraphVertex: <None>

	// Graph Pixel
	struct SurfaceDescriptionInputs
	{
		float3 WorldSpaceNormal;
		float3 WorldSpaceViewDirection;
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
		float4 _Property_C88E5DBC_Out_0 = Inner_Color;
		float _FresnelEffect_7707184E_Out_3;
		Unity_FresnelEffect_float(IN.WorldSpaceNormal, IN.WorldSpaceViewDirection, 1.95, _FresnelEffect_7707184E_Out_3);
		float4 _Property_E3BFBBC2_Out_0 = Outer_Color;
		float4 _Multiply_3991E4CE_Out_2;
		Unity_Multiply_float((_FresnelEffect_7707184E_Out_3.xxxx), _Property_E3BFBBC2_Out_0, _Multiply_3991E4CE_Out_2);
		float4 _Add_D00F4D54_Out_2;
		Unity_Add_float4(_Property_C88E5DBC_Out_0, _Multiply_3991E4CE_Out_2, _Add_D00F4D54_Out_2);
		surface.Color = (_Add_D00F4D54_Out_2.xyz);
		surface.Alpha = 1;
		surface.AlphaClipThreshold = 0.26;
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
		float4 positionCS : SV_POSITION;
		float3 normalWS;
		float3 viewDirectionWS;
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

	// Generated Type: PackedVaryings
	struct PackedVaryings
	{
		float4 positionCS : SV_POSITION;
#if UNITY_ANY_INSTANCING_ENABLED
		uint instanceID : CUSTOM_INSTANCE_ID;
#endif
		float3 interp00 : TEXCOORD0;
		float3 interp01 : TEXCOORD1;
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
		PackedVaryings output = (PackedVaryings)0;
		output.positionCS = input.positionCS;
		output.interp00.xyz = input.normalWS;
		output.interp01.xyz = input.viewDirectionWS;
#if UNITY_ANY_INSTANCING_ENABLED
		output.instanceID = input.instanceID;
#endif
#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
		output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
#endif
#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
		output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
#endif
#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
		output.cullFace = input.cullFace;
#endif
		return output;
	}

	// Unpacked Type: Varyings
	Varyings UnpackVaryings(PackedVaryings input)
	{
		Varyings output = (Varyings)0;
		output.positionCS = input.positionCS;
		output.normalWS = input.interp00.xyz;
		output.viewDirectionWS = input.interp01.xyz;
#if UNITY_ANY_INSTANCING_ENABLED
		output.instanceID = input.instanceID;
#endif
#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
		output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
#endif
#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
		output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
#endif
#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
		output.cullFace = input.cullFace;
#endif
		return output;
	}

	// --------------------------------------------------
	// Build Graph Inputs

	SurfaceDescriptionInputs BuildSurfaceDescriptionInputs(Varyings input)
	{
		SurfaceDescriptionInputs output;
		ZERO_INITIALIZE(SurfaceDescriptionInputs, output);

		// must use interpolated tangent, bitangent and normal before they are normalized in the pixel shader.
		float3 unnormalizedNormalWS = input.normalWS;
		const float renormFactor = 1.0 / length(unnormalizedNormalWS);


		output.WorldSpaceNormal = renormFactor*input.normalWS.xyz;		// we want a unit length Normal Vector node in shader graph


		output.WorldSpaceViewDirection = input.viewDirectionWS; //TODO: by default normalized in HD, but not in universal
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
#include "Packages/com.unity.shadergraph/ShaderGraphLibrary/ShaderVariablesFunctions.hlsl"

		// --------------------------------------------------
		// Graph

		// Graph Properties
		CBUFFER_START(UnityPerMaterial)
		float4 Inner_Color;
	float4 Outer_Color;
	CBUFFER_END

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
		surface.AlphaClipThreshold = 0.26;
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
		float4 positionCS : SV_POSITION;
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

	// Generated Type: PackedVaryings
	struct PackedVaryings
	{
		float4 positionCS : SV_POSITION;
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
		PackedVaryings output = (PackedVaryings)0;
		output.positionCS = input.positionCS;
#if UNITY_ANY_INSTANCING_ENABLED
		output.instanceID = input.instanceID;
#endif
#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
		output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
#endif
#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
		output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
#endif
#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
		output.cullFace = input.cullFace;
#endif
		return output;
	}

	// Unpacked Type: Varyings
	Varyings UnpackVaryings(PackedVaryings input)
	{
		Varyings output = (Varyings)0;
		output.positionCS = input.positionCS;
#if UNITY_ANY_INSTANCING_ENABLED
		output.instanceID = input.instanceID;
#endif
#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
		output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
#endif
#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
		output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
#endif
#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
		output.cullFace = input.cullFace;
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
#include "Packages/com.unity.shadergraph/ShaderGraphLibrary/ShaderVariablesFunctions.hlsl"

		// --------------------------------------------------
		// Graph

		// Graph Properties
		CBUFFER_START(UnityPerMaterial)
		float4 Inner_Color;
	float4 Outer_Color;
	CBUFFER_END

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
		surface.AlphaClipThreshold = 0.26;
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
		float4 positionCS : SV_POSITION;
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

	// Generated Type: PackedVaryings
	struct PackedVaryings
	{
		float4 positionCS : SV_POSITION;
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
		PackedVaryings output = (PackedVaryings)0;
		output.positionCS = input.positionCS;
#if UNITY_ANY_INSTANCING_ENABLED
		output.instanceID = input.instanceID;
#endif
#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
		output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
#endif
#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
		output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
#endif
#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
		output.cullFace = input.cullFace;
#endif
		return output;
	}

	// Unpacked Type: Varyings
	Varyings UnpackVaryings(PackedVaryings input)
	{
		Varyings output = (Varyings)0;
		output.positionCS = input.positionCS;
#if UNITY_ANY_INSTANCING_ENABLED
		output.instanceID = input.instanceID;
#endif
#if (defined(UNITY_STEREO_INSTANCING_ENABLED))
		output.stereoTargetEyeIndexAsRTArrayIdx = input.stereoTargetEyeIndexAsRTArrayIdx;
#endif
#if (defined(UNITY_STEREO_MULTIVIEW_ENABLED)) || (defined(UNITY_STEREO_INSTANCING_ENABLED) && (defined(SHADER_API_GLES3) || defined(SHADER_API_GLCORE)))
		output.stereoTargetEyeIndexAsBlendIdx0 = input.stereoTargetEyeIndexAsBlendIdx0;
#endif
#if defined(SHADER_STAGE_FRAGMENT) && defined(VARYINGS_NEED_CULLFACE)
		output.cullFace = input.cullFace;
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
