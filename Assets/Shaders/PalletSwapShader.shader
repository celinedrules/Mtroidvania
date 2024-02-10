Shader "Custom/PaletteSwapShader"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _PaletteTex ("Palette 1", 2D) = "white" {}
        _PaletteTex2 ("Palette 2", 2D) = "white" {}
        _Blend ("Blend", Range(0, 1)) = 0.5
        _ColorBlendFactor ("Color Blend Factor", Range(0, 1)) = 0.5
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" }
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
            sampler2D _PaletteTex;
            sampler2D _PaletteTex2;
            float _Blend;
            float _ColorBlendFactor;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                fixed4 originalColor = tex2D(_MainTex, i.uv); // Original sprite color
                float gray = dot(originalColor.rgb, float3(0.3, 0.59, 0.11));

                if(originalColor.a > 0.1) 
                {
                    fixed4 palCol1 = tex2D(_PaletteTex, float2(gray, 0));
                    fixed4 palCol2 = tex2D(_PaletteTex2, float2(gray, 0));
                    fixed4 blendedPalColor = lerp(palCol1, palCol2, _Blend);

                    // Blend the original sprite color with the palette color
                    fixed4 finalColor = lerp(originalColor, blendedPalColor, _ColorBlendFactor);

                    finalColor.a = originalColor.a; // Preserve original alpha value
                    return finalColor;
                }
                else
                {
                    return fixed4(0, 0, 0, 0); // Fully transparent color
                }
            }
            ENDCG

            Blend SrcAlpha OneMinusSrcAlpha
            Cull Off
        }
    }
    FallBack "Diffuse"
}
