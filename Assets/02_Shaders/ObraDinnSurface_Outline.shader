Shader "Custom/ObraDinnSurface_Outline"
{
    Properties
    {
        _Color ("Color", Color) = (1,1,1,1)
        _Dither ("Dither", float) = 0.0
        _MainTex ("Albedo (RGB)", 2D) = "white" {}
        _Glossiness ("Smoothness", Range(0,1)) = 0.5
        _Metallic ("Metallic", Range(0,1)) = 0.0

        _Highlight("Highlight", Float) = 0       // 0 = off, 1 = on
        _OutlineColor("Outline Color", Color) = (1,1,0,1)
        _OutlineWidth("Outline Width", Float) = 0.05
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        // MAIN SURFACE SHADER
        CGPROGRAM
        #pragma surface surf Standard fullforwardshadows finalcolor:obradinn
        #pragma target 3.0

        sampler2D _MainTex;
        half _Glossiness;
        half _Metallic;
        fixed4 _Color;
        float _Dither;

        struct Input
        {
            float2 uv_MainTex;
            float4 color : COLOR;
        };

        void obradinn (Input IN, SurfaceOutputStandard o, inout fixed4 color)
        {
            fixed4 newCol;
            newCol.rg = IN.color.rg;
            newCol.b = dot(color.rgb, fixed3(0.299f, 0.587f, 0.114f));
            newCol.a = _Dither;
            color = newCol;
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex) * _Color;
            o.Albedo = c;
            o.Metallic = _Metallic;
            o.Smoothness = _Glossiness;
        }
        ENDCG

        // OUTLINE PASS
        Pass
        {
            Name "Outline"
            Tags { "LightMode"="Always" }

            Cull Front  // Render only backfaces to avoid covering the mesh
            ZWrite On
            ZTest LEqual

            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #include "UnityCG.cginc"

            float _OutlineWidth;
            float _Highlight;
            fixed4 _OutlineColor;

            struct appdata
            {
                float4 vertex : POSITION;
                float3 normal : NORMAL;
            };

            struct v2f
            {
                float4 vertex : SV_POSITION;
            };

            v2f vert(appdata v)
            {
                v2f o;
                float3 norm = normalize(v.normal);

                // Extrude only if highlighted
                float scale = _Highlight * _OutlineWidth;
                v.vertex.xyz += norm * scale;

                o.vertex = UnityObjectToClipPos(v.vertex);
                return o;
            }

            fixed4 frag(v2f i) : SV_Target
            {
                return _OutlineColor;
            }
            ENDCG
        }
    }
    FallBack "Diffuse"
}
