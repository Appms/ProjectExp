Shader "Hidden/Desaturate"
{
	Properties
	{
		_MainTex ("Base (RGB)", 2D) = "white" {}
		_bwBlend ("Black & White blend", Range(0,1)) = 0
	}
	SubShader
	{
		// No culling or depth
		Cull Off ZWrite Off ZTest Always

		Pass
		{
			CGPROGRAM
			#pragma vertex vert_img
			#pragma fragment frag
			
			#include "UnityCG.cginc"
			
			uniform sampler2D _MainTex;
			uniform float _bwBlend;

			float aastep(float threshold, float value) {
				float afwidth = 0.7 * length(float2(ddx(value), ddy(value)));
				return smoothstep(threshold - afwidth, threshold + afwidth, value);
			}

			float4 frag (v2f_img i) : COLOR
			{

				float4 c = tex2D(_MainTex, i.uv);

				float lum = 0.3*c.r + 0.59*c.g + 0.11*c.b;
				float3 bw = float3(lum, lum, lum);

				float4 result = c;
				result.rgb = lerp(c.rgb, bw, _bwBlend);
				return result;
			}
			ENDCG
		}
	}
}
