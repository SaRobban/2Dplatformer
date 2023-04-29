Shader "DisplacePrePost/Displace_WaterfallBOT"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
		_FlowSettings("XY animation, Z scale, w color", Vector) = (0, 1, 0.5, 1)
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
					float2 uv : TEXCOORD1;
					float2 orgUV : TEXCOORD0;
					float4 vertex : SV_POSITION;
					float2 worldPos : TEXCOORD3;
				};

				sampler2D _MainTex;
				float4 _MainTex_ST;
				float4 _FlowSettings;

				v2f vert(appdata v)
				{
					v2f o;
					o.vertex = UnityObjectToClipPos(v.vertex);
					o.worldPos = mul(unity_ObjectToWorld, v.vertex).xy;
					//o.worldPos = mul(unity_WorldToObject, o.worldPos).xy;

					o.uv = TRANSFORM_TEX(v.uv, _MainTex);
					o.orgUV = v.uv;

					return o;
				}

					fixed4 frag(v2f i) : SV_Target
					{
						//Frame Tex
						float frameX = (i.orgUV.x) * 2 - 1;
						frameX *= frameX;
						float frameY = frameX;
						frameY -= 0.8;
						frameY = clamp(frameY,0,1) * 5;

						//Tapper to worldmapping
						float2 uv2 = float2(
							i.worldPos.x * _MainTex_ST.x + _MainTex_ST.z,
							i.worldPos.y * _MainTex_ST.y + _MainTex_ST.z + _Time.y* _FlowSettings.x
						);

						//Sample Texture
						fixed4 col = tex2D(_MainTex, uv2);

						uv2.y += _Time.y* _FlowSettings.x;
						fixed4 col2 = col + tex2D(_MainTex, uv2 - 0.5);
						//Texture to Mask
						col2 -= 0.75;
						col2 = clamp(col2, 0, 1);
						col2 = col * frameY + col2 + frameY;
						col.a = 1;

						col.g += frameX;
						col.b = pow(col2.b, 4);

						col = lerp(col, 0.5, frameY * col2.r * col2.r);

						return col;

					}

					ENDCG
			}
		}
}
