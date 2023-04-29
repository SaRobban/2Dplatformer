Shader "Robbans/WaterSurface2"
{
    //Unity Properties
    Properties
    {
        _Color("Color", Color) = (.34, .85, .92, 1)
        _MainTex ("Texture", 2D) = "white" {}
        _AnimSpeed("AnimSpeed", Range(0,4)) = 1
        _WaveStr("WaveStrength", Range (0,0.1)) = 0.1
        _Waves("NumberOfWaves", Range (0,100)) = 40
    }
    SubShader
    {
        //Shader unity comands type
        Tags {"Queue" = "Transparent" "IgnoreProjector" = "True" "RenderType" = "Transparent"}
        LOD 100
        ZWrite Off
        Blend SrcAlpha OneMinusSrcAlpha //<-- Blend mode, change if you want transperecy to act diffrently 
        
        Pass
        {
            CGPROGRAM
            #pragma vertex vertexFunction
            #pragma fragment fragmentFunction
            #include "UnityCG.cginc"

            //Get our external data set in Properties
            sampler2D _MainTex;
            float4 _MainTex_ST;
            float4 _Color;
            float _AnimSpeed;
            float _WaveStr;
            float _Waves;
            
            //Vertex Function (Build the mesh)
            //What data does the shader need to build? (vertex position and UV)

            //Appdata (data we want/need IN to shader from object)
            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };
            
            //Vertex to fragment (build our object and send to fragmentFunction ("appdata" in, "o" out to fragmentFunction)
            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            v2f vertexFunction(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            //Fragment (pixel shader) (what should be rendered at the surface at this pixel)
            fixed4 fragmentFunction(v2f i) : SV_Target
            {
                //Flip UV Yaxis (since it is a water surface reflection)
                float2 flipUV = i.uv;
                flipUV.y = 1 - flipUV.y;

                //FadeCorners
                float leftPow = clamp(i.uv.x * 10, 0, 1);
                float rightPow = clamp(1 - i.uv.x * 10, 0, 1);


                //Create a wave in the Xaxis
                flipUV.x += sin(i.uv.y * i.uv.y * i.uv.y * _Waves + _Time.y  * _AnimSpeed) * flipUV.y * _WaveStr * leftPow * rightPow;

                //set pixel color by reading the texture data at uv, * color
                fixed4 col = tex2D(_MainTex, flipUV) + i.uv.y * _Color;


                //Alpha fade in x axis
                float alphaX = i.uv.x * 2 - 1;
                alphaX = abs(alphaX);
                alphaX = 1 - alphaX;

                //Alpha (transperency) fade (original alpha * alpha fade Xaxis * alpha fade Y axis)
                col.a = col.a * alphaX * i.uv.y;

                // apply fog
                return col;
            }
            ENDCG
        }
    }
}
