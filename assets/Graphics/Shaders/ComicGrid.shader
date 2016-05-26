Shader "Custom/ComicGrid"
{
	Properties
	{
		_H("Hue",Range(-0.5,0.5)) = 0
		_S("Saturation",Range(-1,1)) = 0
		_B("Brightness",Range(-1,1)) = 0
		_MainTex("Texture", 2D) = "white" {}
		//_NormalTex("Normal Map", 2D) = "bump" {}
		_ShadowColor("Shadows Color", Color) = (1.0,1.0,1.0,1.0)
		[MaterialToggle]_SoftShadows("Soft Shadows", Float) = 1.0
		_Levels("Levels", Range(2, 100)) = 3
		_Reflectivity("Reflectivity", Range(0,1)) = 0
		_Frequency("Frequency", Range(0,1)) = 0
		_Angle("Angle", Range(0,90)) = 0
		_Size("Size", Range(1,10)) = 1
	}
	SubShader
	{
		Tags { "RenderType"="Opaque" }
		LOD 100

		Pass
		{
			Tags{
				"LightMode" = "ForwardBase"
			}

			CGPROGRAM

			#pragma vertex vert
			#pragma fragment frag
			#include "UnityCG.cginc"

			#pragma multi_compile_fwdbase
			#include "AutoLight.cginc"

			#include "UtilityCG.cginc"

			float _H;
			float _S;
			float _B;
			sampler2D _MainTex;
			float4 _MainTex_ST;
			sampler2D _NormalTex;
			float4 _NormalTex_ST;
			float4 _ShadowColor;
			float _SoftShadows;
			float _Levels;
			float _Reflectivity;
			float _Frequency;
			float _Angle;
			float _Size;
			
			v2f vert(a2v i)
			{
				v2f o;

				o.pos = mul(UNITY_MATRIX_MVP, i.pos);
				o.normal = i.normal;
				o.color = i.color;
				o.uv = TRANSFORM_TEX(i.uv, _MainTex);
				o.normaluv = TRANSFORM_TEX(i.uv, _NormalTex);
				o.position = mul(_Object2World, i.pos).xyz;

				TRANSFER_VERTEX_TO_FRAGMENT(o);

				return o;
			}
			
			float4 frag(v2f i) : SV_Target
			{
				// Calculate Normal, Eye and Light Vector
				//TODO Figure why unpacking normals doesn't work with this
				//float3 N = normalize(UnityObjectToWorldNormal(UnpackNormal(tex2D(_NormalTex, i.normaluv))));
				float3 N = normalize(UnityObjectToWorldNormal(i.normal));
				float3 E = -normalize(UnityWorldSpaceViewDir(i.position));
				float3 L = normalize(UnityWorldSpaceLightDir(i.position));

				// Get shadows from shadow map
				float attenuation = LIGHT_ATTENUATION(i);

				// Calculate CubeMap contribution
				float3 reflectedDir = reflect(E, N);
				float4 c = UNITY_SAMPLE_TEXCUBE(unity_SpecCube0, reflectedDir);
				float4 cubemapColor = float4(DecodeHDR(c, unity_SpecCube0_HDR), 1.0);

				//col = lerp(col, finalColor, (1 + dot(N, E)) * _Reflectivity);

				// Sample the texture and modify the color
				HSBColor baseColor = RGB2HSB(tex2D(_MainTex, i.uv));
				baseColor.h += _H;
				baseColor.s += _S;
				baseColor.b += _B;

				HSBColor shadowColor = RGB2HSB(_ShadowColor);

				float4 col = HSB2RGB(baseColor);

				// Calculate shadows
				float scaleFactor = 1.0 / _Levels;
				float shadow = floor(max(dot(L, N), 0) * _Levels) * scaleFactor * attenuation;

				// Distance to nearest point in a grid of
				// (frequency x frequency) points over the unit square
				float2x2 rotMatrix = { cos(radians(_Angle)), -sin(radians(_Angle)), sin(radians(_Angle)), cos(radians(_Angle)) };
				float2 st2 = mul(mul(lerp(1, _ScreenParams.x, _Frequency / 3), rotMatrix), i.pos.xy / _ScreenParams.x);
				float2 nearest = 2.0*frac(st2) - 1.0;
				float dist = length(nearest);

				float radius = sqrt(1.0 - shadow.r);

				// Calculate half of the size of the square's sides
				float halfSizeLength = pow(radius, _Size) / sqrt(2);

				// Check if the fragment is inside the square
				float X = aastep(halfSizeLength, abs(nearest.x));
				float Y = aastep(halfSizeLength, abs(nearest.y));

				// Add reflections from cube map and add the shadow grid pattern
				float4 reflectionColor = lerp(HSB2RGB(baseColor), cubemapColor, (1 + dot(N, E)) * _Reflectivity);
				float4 finalColor = lerp(_ShadowColor, reflectionColor,  lerp(aastep(0.5, X + Y), 1, _SoftShadows * shadow.r));

				return finalColor;
			}
			ENDCG
		}
	}
	Fallback "VertexLit"
}
