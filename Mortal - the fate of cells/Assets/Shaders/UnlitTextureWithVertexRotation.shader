Shader "Unlit/UnlitTextureWithVertexRotation"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _Color ("Color", Color) = (1.0, 1.0, 1.0, 1.0)
        _ColorOutline ("Color Outline", Color) = (1.0, 1.0, 1.0, 1.0)
		_RotationAxis("Rotation Axis", Vector) = (0.0, 1.0, 0.0, 0.0)
		_RotationSpeed("Rotation Speed", float) = 0
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

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            float4 _MainTex_ST, _Color, _RotationAxis, _ColorOutline;
			float _RotationSpeed;

			float3x3 AngleAxis3x3(float angle, float3 axis)
			{
				float c, s;
				sincos(angle, s, c);

				float t = 1 - c;
				float x = axis.x;
				float y = axis.y;
				float z = axis.z;

				return float3x3(
					t * x * x + c, t * x * y - s * z, t * x * z + s * y,
					t * x * y + s * z, t * y * y + c, t * y * z - s * x,
					t * x * z - s * y, t * y * z + s * x, t * z * z + c
					);
			}

            v2f vert (appdata v)
            {
                v2f o;
				float3 vertex = v.vertex.xyz;
				float3 rotAxisNorm = normalize(_RotationAxis.xyz);
				float3x3 rotation = AngleAxis3x3(sin(_Time.x * _RotationSpeed) * 360, rotAxisNorm);
				vertex = mul(rotation, vertex);
                o.vertex = UnityObjectToClipPos(float4(vertex.xyz, 1));
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
