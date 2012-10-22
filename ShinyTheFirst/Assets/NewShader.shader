Shader "Custom/NewShader" {
Properties {
	_Color ("Particle Texture", Color) = (0.5,0.5,0.5,0.5)
}

Category {
	Tags { "Queue"="Transparent" "IgnoreProjector"="True" "RenderType"="Transparent" }
	Blend SrcAlpha OneMinusSrcAlpha
	AlphaTest Greater 0
	ColorMask RGB
	Cull Off Lighting Off ZWrite Off Fog { Mode Off }
	BindChannels {
		Bind "Color", color
		Bind "Vertex", vertex
		Bind "TexCoord", texcoord
	}
	
	// ---- Fragment program cards
	SubShader {
		Pass {
		
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma fragmentoption ARB_precision_hint_fastest
			#pragma multi_compile_particles

			#include "UnityCG.cginc"

			float4 _Color;
			
			struct appdata_t {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
			};

			struct v2f {
				float4 vertex : POSITION;
				float2 texcoord : TEXCOORD0;
			};
			
			v2f vert (appdata_t v)
			{
				v2f o;
				o.vertex = mul(UNITY_MATRIX_MVP, v.vertex);

				o.texcoord = float4( v.texcoord.xy, 0, 0 );
				return o;
			}

			sampler2D _CameraDepthTexture;
			float _InvFade;
			
			fixed4 frag (v2f i) : COLOR
			{
				
				half2 center = half2(.5, .5);
				half d = distance(center, i.texcoord);
				
				float min = 0.05;
				float max = 0.49;
				
				if (d > max){
					_Color.a = 0;
				} else if (d > min) {
					float dd = (d - min) / (max - min);
					dd = sqrt (dd);
					_Color.a = 1 - dd;
				}

				return _Color;
				
				//return 2.0f * i.color * _TintColor * _Color;
			}
			ENDCG 
		}
	} 	
	
}
}

//	Properties {
//		_Color ("Main coor", Color) = (1, 0, 1, 1)
//	}
//SubShader {
//    Pass {
//        Fog { Mode Off }
//        
//CGPROGRAM
//#pragma vertex vert
//#pragma fragment frag
//#pragma fragmentoption ARB_precision_hint_fastest
//#pragma multi_compile_particles
//
//float4 _Color;
//
//// vertex input: position, UV
//struct appdata {
//    float4 vertex : POSITION;
//    float4 texcoord : TEXCOORD0;
//};
//
//struct v2f {
//    float4 pos : POSITION;
//    float4 uv : TEXCOORD0; 
//};
//v2f vert (appdata v) {
//    v2f o;
//    o.pos = mul( UNITY_MATRIX_MVP, v.vertex );
//    //o.pos = v.vertex;
//    o.uv = float4( v.texcoord.xy, 0, 0 );
//    return o;
//}
//half4 frag( v2f i ) : COLOR {
//	half2 center = half2(.5, .5);
//	half d = distance(center, i.uv);
//	
//	if (d > 0.0001){
//		return _Color / d;
//	}
//	else {
//		return _Color;
//	}
//    
//    half4 c = frac( i.uv );
//    if (any(saturate(i.uv) - i.uv))
//        c.b = 0.5;
//    return c;
//}
//ENDCG
//    }
//}
//}