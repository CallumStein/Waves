Shader "Custom/OutlineBlend"
{
    Properties
    {
        _MainTex("Base Texture", 2D) = "white" {}
        _OutlineColor("Outline Color", Color) = (1,1,0,1)
        _OutlineWidth("Outline Width", Float) = 0.05
        _BlendAmount("Blend Amount", Range(0,1)) = 0.5
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

            sampler2D _MainTex;
            float4 _OutlineColor;
            float _BlendAmount;

            struct appdata { float4 vertex : POSITION; float2 uv : TEXCOORD0; };
            struct v2f { float2 uv : TEXCOORD0; float4 vertex : SV_POSITION; };

            v2f vert(appdata v)
            {
                v2f o;
                o.vertex = UnityObjectToClipPos(v.vertex);
                o.uv = v.uv;
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                fixed4 baseCol = tex2D(_MainTex, i.uv);
                // Blend the outline color with the base color
                return lerp(baseCol, _OutlineColor, _BlendAmount);
            }
            ENDCG
        }
    }
}