// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

// Upgrade NOTE: replaced '_Object2World' with 'unity_ObjectToWorld'

Shader "DisplacePrePost/HeatRise"
{
	Properties
	{
		_Color("Color", Color) = (.34, .85, .92, 1)
		_MainTex("Texture", 2D) = "white" {}
		_Speed("Speed", Range(0.0, -1.0)) = -0.5
		_Cfade("cornerFade", Range(0.0,64)) = 32
		_TextureScale("TexScale", Range(0.0,4)) = 0.25
	}
		SubShader
		{
			Tags { "RenderType" = "Opaque" "DisableBatching" = "True"  }
				LOD 100

			Pass
			{
				Cull Off
				Lighting Off
			//			ZWrite Off
						Blend One OneMinusSrcAlpha


						CGPROGRAM
			//#pragma surface surf Lambert vertex:vert nofog nolightmap nodynlightmap keepalpha noinstancing
			#pragma multi_compile_local _ PIXELSNAP_ON
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
				float4 vertex : SV_POSITION;
				float2 uv : TEXCOORD0;
				float3 worldPos : TEXCOORD1;
				float2 localPos : TEXCOORD2;
				float2 scale : TEXCOORD3;
			};

			fixed4 _Color;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			float _Speed;
			float _Cfade;
			float _TextureScale;

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				o.worldPos = mul(unity_ObjectToWorld, v.vertex);

				//THX. Random Output : https://forum.unity.com/threads/handle-uniform-non-uniform-scale-in-a-shader.115675/#post-2113659
				float4 modelX = float4(1.0, 0.0, 0.0, 0.0);
				float4 modelY = float4(0.0, 1.0, 0.0, 0.0);

				float4 modelXInWorld = mul(unity_ObjectToWorld, modelX);
				float4 modelYInWorld = mul(unity_ObjectToWorld, modelY);

				float scaleX = length(modelXInWorld) / 2;
				float scaleY = length(modelYInWorld) / 2;

				//float2 modelXInWorld = float2(length(modelXInWorld), length(modelYInWorld));
				o.scale = float2(scaleX, scaleY);
				o.localPos = v.vertex.xy * 2;// *float2(scaleX, scaleY);//  mul(unity_WorldToObject, mul(unity_ObjectToWorld, v.vertex)).xy;
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				/*
				//Corner

				float fadeStr = 10.5;
				float left = clamp(i.localPos.x * _Cfade, 0, 1);
				//float right = clamp(_Cfade + i.uv.x * -_Cfade, 0, 1);
				float bot = clamp(i.uv.y * _Cfade, 0, 1);

				*/
				// sample the texture

//				scale = 12
				float pxU = 1 / (i.scale.x * 32);
				float oneU = i.localPos.x;// *i.scale.x;

				float2 pos = abs(i.localPos) * -1 + 1;
				pos *= i.scale*2;
				pos = clamp(pos, 0, 1);
				float scaledFrame = pos.x * pos.y;
					
				float2 uv = i.worldPos * _TextureScale;
				uv.y += _Time.y * _Speed;
				fixed4 col =0.5- tex2D(_MainTex, uv);
				col.r*=(1 - i.uv.y);
				col.r *= scaledFrame;
				col.r += 0.5;
//				col.r = lerp(0.5, col.r, col.r);
				col.g = 0.0;
				col.b = 0.0;
				col.a = 1;
				/*
				float4 left;
					left.x = ceil((abs(oneU) + 4 * pxU) - 1);// floor((i.localPos.x - invScale * 4)* invScale);// i.scale.x);
					left.x = min( pos.x,pos.y);
					left.x = scaledFrame;
					left.y = left.x;
					left.z = left.x;
					left.a = 1;
					*/
					return col;
				}
				ENDCG
			}
		}
}