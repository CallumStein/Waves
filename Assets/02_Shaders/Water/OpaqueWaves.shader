Shader "Custom/Water/OpaqueWaves"
{
    Properties
    {
        _WaterColor ("Water Color", Color) = (0.0, 0.45, 0.65, 1)
        _FoamColor ("Foam Color", Color) = (1,1,1,1)

        _WaveHeight ("Wave Height", Float) = 0.25
        _WaveFrequency ("Wave Frequency", Float) = 1.5
        _WaveSpeed ("Wave Speed", Float) = 1.0

        _FoamDepth ("Foam Depth", Float) = 0.5
        _FoamIntensity ("Foam Intensity", Float) = 1.0
    }

    SubShader
    {
        Tags { "Queue"="Geometry" "RenderType"="Opaque" }
        LOD 300

        CGPROGRAM
        #pragma surface surf Standard vertex:vert addshadow
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
            float sceneDepth = LinearEyeDepth(
                tex2Dproj(_CameraDepthTexture, UNITY_PROJ_COORD(IN.screenPos)).r
            );

            float surfaceDepth = LinearEyeDepth(IN.screenPos.z / IN.screenPos.w);

            float foam = saturate((sceneDepth - surfaceDepth) / _FoamDepth);
            foam = 1 - foam;
            foam *= _FoamIntensity;

            fixed3 color = lerp(_WaterColor.rgb, _FoamColor.rgb, foam);

            o.Albedo = color;
            o.Smoothness = 0.9;
            o.Metallic = 0.0;
            o.Emission = foam * _FoamColor.rgb;
        }
        ENDCG
    }

    FallBack "Standard"
}
