Shader "Custom/OneForAllAura"
{
    Properties
    {
        _MainTex ("Base (RGB)", 2D) = "white" {}
        _GlowColor ("Glow Color", Color) = (0.5,0.7,1,1)
        _GlowIntensity ("Glow Intensity", Range(0, 5)) = 1.0
        _TimeScale ("Time Scale", Range(0.1, 2)) = 1.0
        _NoiseScale ("Noise Scale", Range(0.1, 2)) = 1.0
        _PanningSpeed ("Panning Speed", Vector) = (0.1, 0.1, 0, 0)
        _NoiseTex ("Noise Texture", 2D) = "white" {}
        _DisplacementStrength ("Displacement Strength", Range(0, 1)) = 0.1
    }
    SubShader
    {
        Tags { "RenderType"="Opaque" }
        LOD 200

        CGPROGRAM
        #pragma surface surf Lambert vertex:vert

        sampler2D _MainTex;
        sampler2D _NoiseTex;
        fixed4 _GlowColor;
        float _GlowIntensity;
        float _TimeScale;
        float _NoiseScale;
        float _DisplacementStrength;
        float4 _PanningSpeed;

        struct Input
        {
            float2 uv_MainTex;
            float2 uv_NoiseTex;
            float3 worldPos;
        };

        void vert(inout appdata_full v)
        {
            // Vertex displacement using noise texture
            float3 noise = tex2Dlod(_NoiseTex, float4(v.texcoord.xy * _NoiseScale, 0, 0)).rgb;
            v.vertex.xyz += noise * _DisplacementStrength;
        }

        void surf (Input IN, inout SurfaceOutput o)
        {
            // Base texture
            fixed4 c = tex2D (_MainTex, IN.uv_MainTex);
            
            // Panning noise texture for dynamic effect
            float2 panningUV = IN.uv_NoiseTex + (_Time.y * _PanningSpeed.xy);
            float noiseValue = tex2D(_NoiseTex, panningUV).r;
            
            // Add glow effect
            float glow = _GlowIntensity * (1 + sin(_Time.y * _TimeScale));
            
            // Combine the glow with noise and original color
            c.rgb += _GlowColor.rgb * glow * noiseValue;

            o.Albedo = c.rgb;
            o.Emission = c.rgb * _GlowIntensity;
        }
        ENDCG
    }
    FallBack "Diffuse"
}
