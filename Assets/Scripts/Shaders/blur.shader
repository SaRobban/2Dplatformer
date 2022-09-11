Shader "custom/blur"
{
	Properties
	{
		_MainTex("Texture", 2D) = "white" {}
	}
		SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert_img
			//#pragma vertex vert
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

			v2f vert(appdata v)
			{
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.uv;
				return o;
			}

			sampler2D _MainTex;

			fixed4 frag(v2f i) : SV_Target
			{
				fixed4 col = tex2D(_MainTex, i.uv);
			float2 uvPlus = i.uv;
			int totalItt = 0;
			for (int x = -1; x < 2; x++) {
				for (int y = -1; y < 2; y++) {
					uvPlus = i.uv;
					uvPlus.x += x * 0.1;
					uvPlus.y += y * 0.1;
					col += tex2D(_MainTex, uvPlus);
						totalItt++;
				}
			}

			col /= totalItt;

				return col;
			}
			ENDCG
		}
	}
}
