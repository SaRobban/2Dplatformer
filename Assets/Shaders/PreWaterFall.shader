Shader "Unlit/PreWaterFall"
{
	Properties
	{
		_Color("Color", Color) = (.34, .85, .92, 1)
		_MainTex("Texture", 2D) = "white" {}
		_FrameTex("Texture", 2D) = "white" {}
	_Speed("Speed", Range(0.0, 1.0)) = 0.5
	_Str("Stength", Range(0.0, 1.0)) = 0.5
		
		
	}
		SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 100

		Pass
		{
			Cull Off
			Lighting Off
			ZWrite Off
			Blend One OneMinusSrcAlpha

			CGPROGRAM
			//#pragma surface surf Lambert vertex:vert nofog nolightmap nodynlightmap keepalpha noinstancing
			//#pragma multi_compile_local _ PIXELSNAP_ON
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
				UNITY_FOG_COORDS(1)
				float4 vertex : SV_POSITION;
			};

			fixed4 _Color;
			sampler2D _MainTex;
			sampler2D _FrameTex;
			float4 _MainTex_ST;
			float _Speed;
			float _Str;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				//UNITY_TRANSFER_FOG(o,o.vertex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				//Corner
				float2 uv = i.uv;
				uv.y += _Time.y * _Speed;
				// sample the texture
				fixed4 col = 0.5;
				fixed4 tex = tex2D(_MainTex, uv);
				fixed4 frame = tex2D(_FrameTex, i.uv);
				col += tex*0.5 * _Str;
				col -=(1- tex)*0.5 * _Str;
				col += frame;
				return col;// *_Color;
			}
			ENDCG
		}
	}
}
