Shader "Custom/WaveShaderWithScrollingUVAndNormalMapAndFoam"
{
    Properties
    {
        _MainTex ("Main Texture", 2D) = "white" {}
        _NormalMap ("Normal Map", 2D) = "bump" {}
        _FoamTex ("Foam Texture", 2D) = "white" {}
        _Tint ("Tint", Color) = (1, 1, 1, 1)
        _Frequency ("Frequency", Float) = 2.0
        _Speed ("Speed", Float) = 2.0
        _Amplitude ("Amplitude", Float) = 0.2
        _UVSpeed ("UV Speed", Float) = 0.1 // Controls the speed of scrolling
        _FoamThreshold ("Foam Threshold", Float) = 0.5 // Controls when foam appears (based on wave height)
    }

    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Lambert vertex:vert

        sampler2D _MainTex;
        sampler2D _NormalMap;  // The normal map (bump map) for finer details
        sampler2D _FoamTex;    // Foam texture for simulating foam on waves
        float4 _Tint;
        float _Frequency;
        float _Speed;
        float _Amplitude;
        float _UVSpeed;  // Speed for scrolling UV
        float _FoamThreshold; // Threshold to control where foam appears

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_NormalMap;  // UV coordinates for normal map
            float4 color : COLOR;  // To retain the vertex color
            float waveHeight; // To store the height of the wave for foam control
        };

        void vert(inout appdata_full v)
        {
            // Apply wave effect using the x-axis and time for vertical displacement
            float wave = sin(v.vertex.x * _Frequency + _Time.y * _Speed) * _Amplitude;
            v.vertex.y += wave; // Displace the vertex vertically

            // Store the wave height in a custom variable (not the vertex color)
            v.color.rgb = float3(1.0, 1.0, 1.0); // We can reset this to white for proper color handling
            v.color.a = wave;  // Store the wave height in the alpha channel (not red)

            // Scroll the UVs for the texture
            v.texcoord.xy += _UVSpeed * _Time.y; // Scroll the texture over time

            // Adjust normal direction for the wave
            v.normal = float3(0, 1, 0);
        }

        void surf(Input IN, inout SurfaceOutput o)
        {
            // Sample the main texture with the updated (scrolled) UVs
            float4 mainTex = tex2D(_MainTex, IN.uv_MainTex);
            
            // Sample the normal map and convert from tangent space to world space
            float3 normalMap = tex2D(_NormalMap, IN.uv_NormalMap).rgb;
            normalMap = normalize(normalMap * 2.0 - 1.0);  // Convert from [0,1] to [-1,1] range

            // Calculate the final normal by blending the normal map with the base normal
            o.Normal = normalize(mul(float4(normalMap, 0), (float3x3) UNITY_MATRIX_IT_MV).xyz);
            
            // Apply foam where wave height is above the threshold
            float foamFactor = smoothstep(_FoamThreshold, _FoamThreshold + 0.1, IN.color.a); // Use the alpha for wave height

            // Sample the foam texture and blend with the main texture
            float4 foamTex = tex2D(_FoamTex, IN.uv_MainTex);
            o.Albedo = lerp(mainTex.rgb, foamTex.rgb, foamFactor) * _Tint.rgb * IN.color.rgb;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
