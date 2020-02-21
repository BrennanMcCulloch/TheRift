Shader "Effect/Noir"
{
    Properties
    {
        _MainTex ("Texture", 2D) = "white" {}
        _LightTex ("Light Texture", 2D) = "white" {}
        _Darkness ("Darkness", float) = 1
        _WhitePoint ("White Point", float) = 2
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
            // make fog work
            #pragma multi_compile_fog

            #include "UnityCG.cginc"

            struct appdata
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct v2f
            {
                float2 uv : TEXCOORD0;
                UNITY_FOG_COORDS(1)
                float4 vertex : SV_POSITION;
            };

            sampler2D _MainTex;
            sampler2D _LightTex;
            float4 _MainTex_ST;
            float _Darkness;
            float _WhitePoint;

            v2f vert (appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = TRANSFORM_TEX(v.uv, _MainTex);
                //UNITY_TRANSFER_FOG(o,o.vertex);
                return o;
            }

            fixed4 frag (v2f i) : SV_Target
            {
                // sample the texture
                fixed4 col = tex2D(_MainTex, i.uv);
                //float intensity = length(col);
                float intensity = max(max(col.r, col.g), col.b);
                if (length(col) > _WhitePoint) {
                    return 1;
                } else {
                    col = col * tex2D(_LightTex, float2(intensity / (_Darkness), 0));
                    return col;
                }
            }
            ENDCG
        }
    }
}
