// Upgrade NOTE: commented out 'float3 _WorldSpaceCameraPos', a built-in variable

Shader "Roberts/ScreenPos" {
	Properties{
	  _MainTex("Texture", 2D) = "white" {}
		_Color("Color", Color) = (.34, .85, .92, 1) // color
	  _Detail("Detail", 2D) = "gray" {}
	   _Offset("Offset", Range(0,1.5)) = 0.5
	}
		SubShader{
		  Lighting Off
		  Tags { "RenderType" = "Opaque" }
		  CGPROGRAM
		  #pragma surface surf Lambert noshadow 




		  struct Input {
			  float2 uv_MainTex;
			  float4 screenPos;
			  //float3 worldPos;
		  };
		  sampler2D _MainTex;
		  sampler2D _Detail;
		  float _Offset;

		  float4 _Color;
		  // float3 _WorldSpaceCameraPos;

		  void surf(Input IN, inout SurfaceOutput o) {
			  //o.Albedo = tex2D(_MainTex, IN.uv_MainTex).rgb;
			  
			  float2 screenUV = IN.screenPos.xy  / IN.screenPos.w;
			  screenUV.y *= -1;
			  screenUV.y += 1;
			  screenUV.y -= _Offset;

			  //o.Albedo *= tex2D(_Detail, screenUV).rgb * 2;

			  o.Albedo = tex2D(_MainTex, screenUV).rgb * _Color;
			  o.Emission = tex2D(_MainTex, screenUV).rgb * _Color;
		  }
		  ENDCG
	}
		Fallback "Diffuse"
}