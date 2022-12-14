Shader "Hidden/TestImageFX"
{
    Properties
    {
         _Color("Color", Color) = (.34, .85, .92, 1)
        _MainTex ("Texture", 2D) = "white" {}
    }
    SubShader
    {
        // No culling or depth
        Cull Off ZWrite Off ZTest Always

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

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            sampler2D _MainTex;
            fixed3 _Color;

            fixed4 frag(v2f i) : SV_Target
            {

                float2 uv = i.uv;
                fixed4 col = tex2D(_MainTex, uv);
                // just invert the colors

                    uv.x += sin(_Time.y + uv.y * 4) * 0.1;
                    //uv.y = 1 - uv.y;

                    col = tex2D(_MainTex, uv);
                    // col.rgb = 1 - col.rgb;
                    //col.b *= 4;
                return col;
            }
            ENDCG
        }
    }
}
