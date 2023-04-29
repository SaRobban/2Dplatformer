// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/DisplaceMap_WaterFall"
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
			Cull Off
			Lighting Off
			ZWrite Off
			Blend One OneMinusSrcAlpha

			CGPROGRAM
			//#pragma surface surf Lambert vertex:vert nofog nolightmap nodynlightmap keepalpha noinstancing
			//#pragma multi_compile_local _ PIXELSNAP_ON
			 #pragma target 3.0
			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			sampler2D _MainTex;
			float4 _FlowSettings;

			struct appdata
			{
				float2 uv : TEXCOORD0;
				float4 vertex : POSITION;
			};

			struct v2f
			{
				float2 uv : TEXCOORD0;
				float2 worldPos : TEXCOORD1;
				float4 vertex : SV_POSITION;
			};

		
			
			v2f vert(appdata v)
			{
				v2f o;
				o.uv = v.uv;
				o.worldPos = mul(unity_ObjectToWorld, v.vertex).xy;
				o.vertex = UnityObjectToClipPos(v.vertex);
				return o;
			}

			fixed4 frag(v2f i) : SV_Target
			{
				//Frame
				float frame = abs(i.uv.x-0.5);
				frame = max(frame, abs(i.uv.y-0.5));
				frame *= frame;


				// sample the texture
				fixed4 col = tex2D(_MainTex, i.worldPos.xy * _FlowSettings.z + _FlowSettings.xy * _Time.y);
				
				//Red ch is displacement
				col.r = lerp(col, 0.5, frame);
			
				//green ch is hilight
				col.g = pow((col.r - 0.5),2) * 4;
				col.g = pow(col.g + frame, 8)*8;


				//blue ch is Color;
				col.b = frame * (col.r * _FlowSettings.w);
			
				return col;
			}
			ENDCG
		}
	}
}
