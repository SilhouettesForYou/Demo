// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/Heart"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
		_HeightFactor("HeightFactor",Range(0, 0.5)) = 0
		_WidthFactor("WidthFactor",Range(0, 0.5)) = 0.2
    }

	SubShader 
    {
		Tags { "RenderType"="Opaque" }
		LOD 100
		Blend SrcAlpha OneMinusSrcAlpha

        pass
        {
            CGPROGRAM

            #pragma vertex vert
            #pragma fragment frag

            #include "UnityCG.cginc"

            struct v2f
            {
                float4 pos : POSITION;
                float2 uv : TEXCOORD0;
            };

            v2f vert(appdata_full v)
            {
                v2f o;
                o.pos = UnityObjectToClipPos(v.vertex);
                o.uv = 2 * v.texcoord.xy - fixed2(1, 1);
                return o;
            }

            float4 frag(v2f i) : COLOR
            {
                fixed3 background = fixed3(1, 0.8, 0.7 - 0.07 * i.uv.x) * (1 - 0.25 * length(i.uv));
                float tt = fmod(_Time.x, 1.0) / 1.0;
                float ss = pow(tt, 2) * .5 + .5;
                ss -= ss * 0.2 * sin(tt * 6.2831 * 3.0) * exp(-tt * 4.0);
                i.uv *= float2(0.5, 1.5) + float2(0.5, 1.5) / ss;

                float a = abs(atan2(i.uv.x, i.uv.y)) / 3.141593;
                float r = length(i.uv);
                float d = (13 * a - 22 * a * a + 10 * a * a * a) / (6 - 5 * a);
                fixed3 hcol = fixed3(1, 0, 0.3);
                fixed3 col = lerp(background, hcol, smoothstep(-0.01, 0.01, d - r));
                return fixed4(col, 1);
            }

            ENDCG
        }
    }
	Fallback "Diffuse"
}