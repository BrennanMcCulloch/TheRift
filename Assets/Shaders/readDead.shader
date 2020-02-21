// Upgrade NOTE: replaced 'mul(UNITY_MATRIX_MVP,*)' with 'UnityObjectToClipPos(*)'

Shader "Custom/readDead" {
    Properties {
        _Color ("Tint", Color) = (0, 0, 0, 1)
        _MainTex ("Texture", 2D) = "white" {}
        _Smoothness ("Smoothness", Range(0, 1)) = 0
        _Metallic ("Metalness", Range(0, 1)) = 0
        _NoiseTex ("Noise Texture", 2D) = "white" {}

        [IntRange] _StencilRef ("Stencil Reference Value", Range(0,255)) = 0
    }
    SubShader {
        Tags{ "RenderType"="Opaque" "Queue"="Geometry"}

        //stencil operation
        Stencil{
            Ref [_StencilRef]
            Comp Equal
        }

        CGPROGRAM

        #pragma surface surf Standard fullforwardshadows
        #pragma target 3.0

        sampler2D _MainTex;
        fixed4 _Color;
        half _Smoothness;
        half _Metallic;
        half3 _Emission;
        half _Curvature;

        struct Input {
            float2 uv_MainTex;
        };

        void surf (Input i, inout SurfaceOutputStandard o) {
            fixed4 col = tex2D(_MainTex, i.uv_MainTex);
            //col *= _Color;
            if (col.r < 0.7) {
                col = 0;
            } else if (col.r < 0.8) {
                col *= _Color;
            }
            o.Albedo += col.rgb;
            o.Metallic = _Metallic;
            o.Smoothness = _Smoothness;
            o.Emission = _Emission;
        }
        ENDCG
    }
    FallBack "Standard"
}