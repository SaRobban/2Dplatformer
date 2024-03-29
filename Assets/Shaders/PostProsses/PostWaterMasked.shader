Shader "DisplacePrePost/PostWaterMasked"
{
	//Unity Properties
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_MaskTex("MaskTexture", 2D) = "white" {}
		_Color("BlueChColor", Color) = (.34, .85, .92, 1)
		_Str("DisplaceStr", Range(0.0, 1.0)) = 0.5
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
					//Displacement red chanel
					fixed4 maskTex = tex2D(_MaskTex, i.uv);
					float maskDisplace = maskTex.r * 2;
					maskDisplace -= 1;
					maskDisplace *= _Str;

					float2 uv = i.uv;
					uv += maskDisplace * maskTex.a;

					//Color blue ch
					float spec = maskTex.b;
					fixed4 color = lerp(1 , _Color, maskTex.g);

					fixed4 col = (tex2D(_MainTex, uv)) * color + spec;
					return col;
				}
				ENDCG
			}
		}
}

