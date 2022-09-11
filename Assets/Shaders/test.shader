Shader "Custom/test"{
	Properties{
		_Color("Tint", Color) = (0, 0, 0, 1)
		_MainTex("Texture", 2D) = "white" {}
		
		_HiLight("HiLight", Range(0,1)) = 1
		_Darkness("Dark", Range(0,1)) = 1
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
				float4 _MainTex_ST;

				fixed4 _Color;
				float _HiLight;
				float _Darkness;

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
					
					fixed4 c = tex2D(_MainTex, i.uv) * _Color;
					c = lerp(c, _Color, _HiLight);

				/*
				fixed4 hi = clamp(c, 1, 2) -1;
				fixed4 low = clamp(c, 0, 1);

				hi *= _HiLight;
				low *= _Darkness;
				*/
				c.rgb = c;

				c.a *= c.a;

					return c;
				}

				ENDCG
			}
	}
}