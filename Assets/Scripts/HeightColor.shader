Shader "Custom/HeightColor"
{
  Properties
  {
    _ColorLow ("Low Color", Color) = (0, 0, 1, 1)
    _ColorHigh ("High Color", Color) = (0, 1, 0, 1)

    _Test ("Test", Range(0, 1)) = 0.0
  }
  SubShader
  {
    Tags { "RenderType"="Opaque" }
    LOD 100

    CGPROGRAM
    #pragma surface surf Standard fullforwardshadows
    #pragma target 3.0

    sampler2D _MainTex;

    fixed4 _ColorLow;
    fixed4 _ColorHigh;
    float _Test;

    struct Input
    {
      float2 uv_MainTex;
      float3 worldNormal;
      float3 worldPos;
    };

    void surf (Input IN, inout SurfaceOutputStandard o)
    {
      float steepness = 1 - abs(IN.worldNormal);
      float weight = smoothstep(0.24 + _Test, 0.5 + _Test, steepness);
      o.Albedo = lerp(_ColorLow, _ColorHigh, weight);
    }
    ENDCG
  }
  FallBack "Diffuse"
}