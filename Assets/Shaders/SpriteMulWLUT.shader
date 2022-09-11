//Shader is based on some sprite shader example i found online
Shader "Roberts/SpriteMulWithLUT"
{
    Properties
    {
        [PerRendererData] _MainTex ("Sprite Texture", 2D) = "white" {}
        _LUTstr("LUT strength", Range(0,1)) = 1
        _LUTTex("LUT Texture", 2D) = "white" {}
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

          
            v2f vert(appdata_t IN)
            {
                v2f OUT;
                OUT.vertex = UnityObjectToClipPos(IN.vertex);
                OUT.texcoord = IN.texcoord;
                OUT.color = IN.color;
                #ifdef PIXELSNAP_ON
                OUT.vertex = UnityPixelSnap (OUT.vertex);
                #endif

                return OUT;
            }

            fixed4 _Color;
            float _HiLight;
            float _Darkness;

            float _LUTstr;
            sampler2D _MainTex;
            sampler2D _LUTTex;

            fixed4 frag(v2f IN) : SV_Target
            {
                //SpriteTexture
                fixed4 c = tex2D(_MainTex, IN.texcoord) * IN.color;
                
                //Max cord for LUT
                float  x = max(c.r, c.g);
                x = max(c.b, x);
                fixed4 lutC = lerp(1, tex2D(_LUTTex, x), _LUTstr);
                
                //Modyfy low/hi brightness
                c *= 2;
                fixed3 add = clamp(c,1,2);
                fixed3 sub = clamp(c,0,1);
                add = lerp(1, add, _HiLight);
                sub = lerp(1, sub, _Darkness);
                                          

                c.rgb = _Color.rgb * add * sub;
				c.rgb *= c.a *0.5f;
                c.rgb *= lutC * c.a;
				
                c.a *= 0.5f;
       
                return c;
            }
        ENDCG
        }
    }
}

