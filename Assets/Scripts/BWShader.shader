﻿Shader "Camera/ScreenWipe"
{
	Properties
	{
		_MainTex ("Texture", 2D) = "white" {}
		_Alpha ("Alpha", Float) = 0.5
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
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);
				o.uv = v.uv;
				return o;
			}
			
			sampler2D _MainTex;
			fixed _Alpha;

			fixed4 frag (v2f i) : SV_Target
			{
				return tex2D(_MainTex, i.uv) * _Alpha;
			}
			ENDCG
		}
	}
}
