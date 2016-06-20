Shader "Hidden/DOF"
{
	Properties
	{
		_MainTex("Base (RGB)", 2D) = "white" {}
		_FocusDepth("FocusDepth", Float) = 0
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

			#define PI 3.141592653589793238462643383279502884197169399375105820974944592307816406286
			#define SIGMA 0.4

			uniform sampler2D _MainTex;
			uniform float4 _MainTex_TexelSize;
			uniform half4 _BlurOffsets;

			uniform float _FocusDepth;

			sampler2D_float _CameraDepthTexture;

			struct v2f {
				float4 pos : SV_POSITION;
				half2 uv : TEXCOORD0;
				half2 taps[4] : TEXCOORD1;
			};

			v2f vert(appdata_img i) {
				v2f o;
				o.pos = mul(UNITY_MATRIX_MVP, i.vertex);
				o.uv = i.texcoord - _BlurOffsets.xy * _MainTex_TexelSize.xy; // hack, see BlurEffect.cs for the reason for this. let's make a new blur effect soon
				o.taps[0] = o.uv + _MainTex_TexelSize * _BlurOffsets.xy;
				o.taps[1] = o.uv - _MainTex_TexelSize * _BlurOffsets.xy;
				o.taps[2] = o.uv + _MainTex_TexelSize * _BlurOffsets.xy * half2(1, -1);
				o.taps[3] = o.uv - _MainTex_TexelSize * _BlurOffsets.xy * half2(1, -1);
				return o;
			}

			float4 frag(v2f i) : COLOR
			{

				float2 uv0Offset = _MainTex_TexelSize * _BlurOffsets.xy * abs(Linear01Depth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, i.uv)) - sqrt(_FocusDepth));
				float2 uv1Offset = -_MainTex_TexelSize * _BlurOffsets.xy * abs(Linear01Depth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, i.uv)) - sqrt(_FocusDepth));
				float2 uv2Offset = _MainTex_TexelSize * _BlurOffsets.xy * half2(1, -1) * abs(Linear01Depth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, i.uv)) - sqrt(_FocusDepth));
				float2 uv3Offset = -_MainTex_TexelSize * _BlurOffsets.xy * half2(1, -1) * abs(Linear01Depth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, i.uv)) - sqrt(_FocusDepth));

				half4 color = tex2D(_MainTex, i.uv + uv0Offset * abs(Linear01Depth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, i.uv + uv0Offset)) - sqrt(_FocusDepth)));
				color += tex2D(_MainTex, i.uv + uv1Offset * abs(Linear01Depth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, i.uv + uv0Offset)) - sqrt(_FocusDepth)));
				color += tex2D(_MainTex, i.uv + uv2Offset * abs(Linear01Depth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, i.uv + uv0Offset)) - sqrt(_FocusDepth)));
				color += tex2D(_MainTex, i.uv + uv3Offset * abs(Linear01Depth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, i.uv + uv0Offset)) - sqrt(_FocusDepth)));

				//return abs(Linear01Depth(SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, i.uv)) - _FocusDepth);

				return color * 0.25;
			}

			/*
			v2f vert( appdata_img v ) 
			{
				v2f o;
				o.pos = mul (UNITY_MATRIX_MVP, v.vertex);
				o.uv1.xy = v.texcoord.xy;
				o.uv.xy = v.texcoord.xy;
		
				#if UNITY_UV_STARTS_AT_TOP
				if (_MainTex_TexelSize.y < 0)
					o.uv.y = 1-o.uv.y;
				#endif			
		
				return o;
			} 

			float4 fragCaptureCoc (v2f i) : SV_Target
			{
				float4 color = float4(0,0,0,0); //tex2D (_MainTex, i.uv1.xy);
				float d = SAMPLE_DEPTH_TEXTURE(_CameraDepthTexture, i.uv1.xy);
				d = Linear01Depth (d);
				color.a = _CurveParams.z * abs(d - _CurveParams.w) / (d + 1e-5f);
				color.a = clamp( max(0.0, color.a - _CurveParams.y), 0.0, _CurveParams.x);

				return color;
			}

			float4 frag4TapBlurForLRSpawn (v2f i) : SV_Target
			{
				float4 tap  =  tex2D(_MainTex, i.uv.xy);

				float4 tapA =  tex2D(_MainTex, i.uv.xy + 0.75 * _MainTex_TexelSize.xy);
				float4 tapB =  tex2D(_MainTex, i.uv.xy - 0.75 * _MainTex_TexelSize.xy);
				float4 tapC =  tex2D(_MainTex, i.uv.xy + 0.75 * _MainTex_TexelSize.xy * float2(1,-1));
				float4 tapD =  tex2D(_MainTex, i.uv.xy - 0.75 * _MainTex_TexelSize.xy * float2(1,-1));

				float4 weights = saturate(10.0 * float4(tapA.a, tapB.a, tapC.a, tapD.a));
				float sumWeights = dot(weights, 1);

				float4 color = (tapA*weights.x + tapB*weights.y + tapC*weights.z + tapD*weights.w);

				float4 outColor = tap;
				if(tap.a * sumWeights * 8.0 > 1e-5f) outColor.rgb = color.rgb/sumWeights;

				return outColor;
			}

			float4 fragBlurInsaneHQ (v2f i) : SV_Target
			{
				float4 centerTap = tex2D(_MainTex, i.uv1.xy);
				float4 sum = centerTap;
				float4 poissonScale = _MainTex_TexelSize.xyxy * centerTap.a * _Offsets.w;

				float sampleCount = max(centerTap.a * 0.25, _Offsets.z); // <- weighing with 0.25 looks nicer for small high freq spec
				sum *= sampleCount;

				float2 weights = 0;

				for(int l=0; l < NumDiscSamples; l++)
				{
					float4 sampleUV = i.uv1.xyxy + DiscKernel[l].xyxy * poissonScale.xyxy / float4(1.2,1.2,DiscKernel[l].zz);

					float4 sample0 = tex2D(_MainTex, sampleUV.xy);
					float4 sample1 = tex2D(_MainTex, sampleUV.zw);

					if( (sample0.a + sample1.a) > 0.0 )
					{
						weights = BokehWeightDisc2(sample0, sample1, float2(DiscKernel[l].z/1.2, 1.0), centerTap);
						sum += sample0 * weights.x + sample1 * weights.y;
						sampleCount += dot(weights, 1);
					}
				}

				float4 returnValue = sum / sampleCount;
				returnValue.a = centerTap.a;

				return returnValue;
			}

			float4 fragBlurUpsampleCombineHQ (v2f i) : SV_Target
			{
				float4 bigBlur = tex2D(_LowRez, i.uv1.xy);
				float4 centerTap = tex2D(_MainTex, i.uv1.xy);

				float4 smallBlur = centerTap;
				float4 poissonScale = _MainTex_TexelSize.xyxy * centerTap.a * _Offsets.w ;

				float sampleCount = max(centerTap.a * 0.25, 0.1f); // <- weighing with 0.25 looks nicer for small high freq spec
				smallBlur *= sampleCount;

				for(int l=0; l < NumDiscSamples; l++)
				{
					float2 sampleUV = i.uv1.xy + DiscKernel[l].xy * poissonScale.xy;

					float4 sample0 = tex2D(_MainTex, sampleUV);
					float weight0 = BokehWeightDisc(sample0, DiscKernel[l].z, centerTap);
					smallBlur += sample0 * weight0; sampleCount += weight0;
				}

				smallBlur /= (sampleCount+1e-5f);
				smallBlur = BlendLowWithHighHQ(centerTap.a, smallBlur, bigBlur);

				return centerTap.a < 1e-2f ? centerTap : float4(smallBlur.rgb,centerTap.a);
			}
			*/
			ENDCG
		}
	}
}
