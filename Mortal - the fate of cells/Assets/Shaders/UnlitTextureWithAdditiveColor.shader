Shader "Unlit/UnlitTextureWithAdditiveColor"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_Color ("Additive Color", Color) = (1.0, 1.0, 1.0, 1.0)
		_Shade ("Shader Color", Color) = (0.0, 0.0, 0.0, 0.0)
		_AnimateUV ("Animate uv", Range(0, 1)) = 0
		_Animation ("Animation", Vector) = (1.0, 1.0, 1.0, 1.0)
    }
    SubShader
    {
		Tags { "Queue" = "Transparent" "RenderType" = "Transparent" "IgnoreProjector" = "True" }
        LOD 100

        Pass
        {
			ZWrite Off
			Blend SrcAlpha OneMinusSrcAlpha
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
            float4 _MainTex_ST, _Color, _Shade, _Animation;
			float _AnimateUV;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
				if (_AnimateUV)
				{
					o.uv.x += (_SinTime.x * _Animation.x) * _Animation.y;
					o.uv.y += (_CosTime.x * _Animation.z) * _Animation.w;
				}
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
				fixed4 col = tex2D(_MainTex, i.uv) *_Color;
				if (col.a < .5)
				{
					col = _Shade;
				}
                return col;
            }
            ENDCG
        }
    }
}
