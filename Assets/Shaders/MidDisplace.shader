Shader "Unlit/MidDisplace"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_str("str", Range(0.0, 1.0)) = 0.5
		_color("color", Color) = (.25, .5, .5, 1)
	}
		SubShader
	{
		Tags { "RenderType" = "Opaque" }
		LOD 100

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

			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _str;
			fixed4 _color;
			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				// sample the texture
				fixed4 col = tex2D(_MainTex, i.uv);
			
				float lum = col.r + col.g + col.b;
				lum*=0.333;
				lum *= -1;
				lum += 1;
				lum *= lum *lum;
				lum *= -1;
				lum += 1;
			
				float4 bw = float4(lum, lum, lum, 1);
			
				fixed4 shade = _color - bw;
				col += shade *_str;

				return col;
			}
			ENDCG
		}
	}
}
