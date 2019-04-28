// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Unlit/InnerOutline"
{
    Properties
    {
        _MainTex ("OutlineMask", 2D) = "white" {}
        _Color ("Color", Color) = (0.0, 0.0, 0.0, 1.0)
        _ColorOutline ("Color Outline", Color) = (0.0, 0.0, 0.0, 1.0)
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 100

        Pass
        {
            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
			#pragma multi_compile_instancing
            #include "UnityCG.cginc"

            struct appdata
            {
			UNITY_VERTEX_INPUT_INSTANCE_ID
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST;
			fixed4 _Color, _ColorOutline;

			UNITY_INSTANCING_BUFFER_START(Props)
				// Define per instance attributes here
			UNITY_INSTANCING_BUFFER_END(Props)

            v2f vert (appdata v)
            {
                v2f o;
				UNITY_INITIALIZE_OUTPUT(v2f, o);
				UNITY_SETUP_INSTANCE_ID(v);
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
				if (col.a > 0)
				{
					col = _ColorOutline;
				}
				else
				{
					col.rgb = lerp(_ColorOutline.rgb, _Color.rgb, _Color.a);
				}
                return col;
            }
            ENDCG
        }
    }
}
