Shader "Custom/Water/WavesWithFoam"
{
    Properties
    {
        _WaterColor ("Water Color", Color) = (0.0, 0.5, 0.7, 0.6)
        _FoamColor ("Foam Color", Color) = (1,1,1,1)

        _WaveHeight ("Wave Height", Float) = 0.3
        _WaveFrequency ("Wave Frequency", Float) = 1.5
        _WaveSpeed ("Wave Speed", Float) = 1.0

        _FoamDepth ("Foam Depth", Float) = 0.5
        _FoamIntensity ("Foam Intensity", Float) = 1.0
    }

    SubShader
    {
        Tags { "Queue"="Transparent" "RenderType"="Transparent" }
        LOD 300

        GrabPass {}

        CGPROGRAM
        #pragma surface surf Standard alpha:fade vertex:vert
        #pragma target 3.0

        sampler2D _CameraDepthTexture;

        fixed4 _WaterColor;
        fixed4 _FoamColor;

        float _WaveHeight;
        float _WaveFrequency;
        float _WaveSpeed;

        float _FoamDepth;
        float _FoamIntensity;

        struct Input
        {
            float2 uv_MainTex;
            float4 screenPos;
        };

        void vert (inout appdata_full v)
        {
            float wave =
                sin(v.vertex.x * _WaveFrequency + _Time.y * _WaveSpeed) +
                cos(v.vertex.z * _WaveFrequency + _Time.y * _WaveSpeed);

            v.vertex.y += wave * _WaveHeight;
        }

        void surf (Input IN, inout SurfaceOutputStandard o)
        {
            float depth = LinearEyeDepth(
                tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(IN.screenPos)).r
            );

            float surfaceDepth = IN.screenPos.w;
            float foam = saturate((depth - surfaceDepth) / _FoamDepth);
            foam = 1 - foam;
            foam *= _FoamIntensity;

            fixed4 col = lerp(_WaterColor, _FoamColor, foam);

            o.Albedo = col.rgb;
            o.Alpha = col.a;
            o.Smoothness = 0.85;
            o.Metallic = 0.0;
            o.Emission = foam * _FoamColor.rgb;
        }
        ENDCG
    }

    FallBack "Transparent/Diffuse"
}
