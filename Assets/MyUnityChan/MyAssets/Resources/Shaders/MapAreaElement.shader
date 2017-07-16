// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/MyUnityChan/MapAreaElement" {
	Properties {
		_Color ("Color", Color) = (1,1,1,0.5)
		_LineWidth ("Line width", float) = 0.05
	}
	SubShader {
		Tags { "Queue"="Transparent" "RenderType"="Opaque" }
		Cull Off
		ZWrite Off
		Blend SrcAlpha OneMinusSrcAlpha
		
		Pass {
			CGPROGRAM
			#pragma vertex vert
			#pragma fragment frag
			#pragma target 3.0

			#include "UnityCG.cginc"

			uniform float4 _Color;
			uniform float _LineWidth;

			struct v2f {
				float4 vertex : POSITION;
				float3 uv : TEXCOORD0;
			};


			v2f vert (appdata_base v) {
				v2f o;
				o.vertex = UnityObjectToClipPos(v.vertex);
				o.uv = v.texcoord;
				return o;
			}

			float4 frag(v2f i) : COLOR {
				float leftx = step(i.uv.x, _LineWidth);
				float upy = step(i.uv.y, _LineWidth);
				float rightx = step(1.0 - _LineWidth, i.uv.x);
				float downy = step(1.0 - _LineWidth, i.uv.y);
				
				return lerp(_Color, (0.8,0.8,0.8,1), leftx || upy || rightx || downy);
			}

			ENDCG
		}
	} 
	FallBack "Diffuse"
}
