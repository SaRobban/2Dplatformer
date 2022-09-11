//Shader is based on some sprite shader example i found online
Shader "Roberts/SpriteMul"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _Color ("Tint", Color) = (1,1,1,1)
        _HiLight("HiLight", Range(0,1)) = 1
        _Darkness("Dark", Range(0,1)) = 1
        [MaterialToggle] PixelSnap ("Pixel snap", Float) = 0
    }

    SubShader
    {
        Tags
        { 
            "Queue"="Transparent" 
            "IgnoreProjector"="True" 
            "RenderType"="TransparentCutout" 
            "PreviewType"="Plane"
            "CanUseSpriteAtlas"="True"
        }

        Cull Off
        Lighting Off
        ZWrite Off
        Fog { Mode Off }
        Blend One OneMinusSrcAlpha

        Pass
        {
        CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
//            #pragma multi_compile DUMMY PIXELSNAP_ON
         #pragma multi_compile_local _ PIXELSNAP_ON
            #include "UnityCG.cginc"

            struct appdata_t
            {
                float4 vertex   : POSITION;
                float4 color    : COLOR;
                float2 texcoord : TEXCOORD0;
            };

            struct v2f
            {
                float4 vertex   : SV_POSITION;
                fixed4 color    : COLOR;
                half2 texcoord  : TEXCOORD0;
            };

            fixed4 _Color;
            float _HiLight;
            float _Darkness;

            v2f vert(appdata_t IN)
            {
                v2f OUT;
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.texcoord = IN.texcoord;
                OUT.color = IN.color;// * _Color;
                #ifdef PIXELSNAP_ON
                OUT.vertex = UnityPixelSnap (OUT.vertex);
                #endif

                return OUT;
            }

            sampler2D _MainTex;

            fixed4 frag(v2f IN) : SV_Target
            {
               
                fixed4 c = tex2D(_MainTex, IN.texcoord) * IN.color * 2;
                fixed3 add = clamp(c,1,2);
                fixed3 sub = clamp(c,0,1);

                add = lerp(1, add, _HiLight);
                sub = lerp(1, sub, _Darkness);
                                                
                c.rgb = _Color.rgb * add * sub;
				c.rgb *= c.a *0.5f;
				c.a *= 0.5f;
                return c;
            }
        ENDCG
        }
    }
}

