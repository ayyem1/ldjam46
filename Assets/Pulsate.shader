﻿Shader "Jared/Pulsate"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
	_Color("Main Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_Intensity("Intensity", Range(0, 1)) = 0.01
		_Frequency("Frequency", Range(0, 100)) = 20
	}
		SubShader
	{
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

		Pass{
		Cull Off
		ZWrite On
		ZTest LEqual

		CGPROGRAM
#pragma vertex vert
#pragma fragment frag

		half4 frag(v2f i) : COLOR{
		fixed4 col = tex2D(_MainTex, i.uv);// * i.color;
	UNITY_APPLY_FOG(i.fogCoord, col);
	return col;
	}
		ENDCG
	}
	}
}