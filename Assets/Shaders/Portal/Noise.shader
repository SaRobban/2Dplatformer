//https://www.ronja-tutorials.com/post/007-sprite-shaders/
Shader "Custom/Noise"
{
	Properties{
		_Color("Tint", Color) = (0, 0, 0, 1)
		_MainTex("Texture", 2D) = "white" {}
		_NoiseTex("Noise", 2D) = "white" {}
	}

		SubShader{
			Tags{
				"RenderType" = "Transparent"
				"Queue" = "Transparent"
			}

			Blend SrcAlpha OneMinusSrcAlpha

			ZWrite off
			Cull off

			Pass{

				CGPROGRAM

				#include "UnityCG.cginc"

				#pragma vertex vert
				#pragma fragment frag

				sampler2D _MainTex;
				sampler2D _NoiseTex;
				float4 _MainTex_ST;

				fixed4 _Color;

				struct appdata {
					float4 vertex : POSITION;
					float2 uv : TEXCOORD0;
					fixed4 color : COLOR;
				};

				struct v2f {
					float4 position : SV_POSITION;
					float2 uv : TEXCOORD0;
					fixed4 color : COLOR;
				};

				v2f vert(appdata v) {
					v2f o;
					o.position = UnityObjectToClipPos(v.vertex);
					o.uv = TRANSFORM_TEX(v.uv, _MainTex);
					o.color = v.color;
					return o;
				}

				fixed4 frag(v2f i) : SV_TARGET{

					float2 uv = i.uv;
					float x = uv.x;
					x = ((x * x) - x) * 4 + 1;
					uv.x =x;
					uv.y += _Time.y - (i.uv.y * i.uv.y);
					float fade =1- i.uv.y;
					fade *=1- x;
					uv.x += i.uv.y*i.uv.y;

					fixed4 col = tex2D(_MainTex, i.uv); 
					float noise = tex2D(_NoiseTex, uv).r * fade;
					col *= noise;
					col += fade * fade;
					col *= _Color;
					col *= i.color;

					return col;
				}

				ENDCG
			}
	}
}