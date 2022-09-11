Shader "Unlit/PostWaterMasked"
{
	//Unity Properties
	Properties
	{
		_Color("Color", Color) = (.34, .85, .92, 1)
		_MainTex("Texture", 2D) = "white" {}
		_MaskTex("MaskTexture", 2D) = "white" {}
		_Str("str", Range(0.0, 1.0)) = 0.5
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
				sampler2D _MaskTex;
				float4 _MainTex_ST;
				float _Str;
				fixed4 _Color;

				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.uv = TRANSFORM_TEX(v.uv, _MainTex);
					return o;
				}

				fixed4 frag(v2f i) : SV_Target
				{
					//Displace UV
					fixed4 mask = tex2D(_MaskTex, i.uv);
					float2 uv = i.uv;
					//0 = -0.5, 128 = 0, 255 = 0.5 * alpha 
					uv += (mask.g -0.5)* _Str * mask.a;

					float hi = (mask.b - 0.75) * 8;
					hi = clamp(hi, 0, 10);
					fixed4 color = lerp(1, _Color, mask.b);
					fixed4 col = (tex2D(_MainTex, uv) + hi) * color;
					return col;
				}
				ENDCG
			}
		}
}

