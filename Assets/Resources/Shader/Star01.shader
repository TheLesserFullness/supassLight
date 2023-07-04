Shader "StarShader/TryOut01"
{
    Properties
    {
        //_Color ("Color", Color) = (1,1,1,1)
        //_MainTex ("Albedo (RGB)", 2D) = "white" {}
        //_Glossiness("Smoothness", Range(0,1)) = 0.5
        //_Metallic("Metallic", Range(0,1)) = 0.0
        _CenterColor("CenterColor",Color) = (1,1,1,1)
        _CenterScale("CenterScale",Range(-1,10)) = 0.1
        _Iterations("Iterations",Range(-10,10)) = 1
        _CenterBrightness("CenterBrightness",Range(0,100)) = 0.5
        _DecayFactor("DecayFactor",Range(0,1)) = 1
    }
        SubShader
    {
         Cull Front


         Pass
         {
                CGPROGRAM
#pragma         vertex vert
#pragma         fragment frag
#include        "UnityCG.cginc"

            struct a2v
            {
                float4 pos : POSITION;
                float3 normal:NORMAL;

            };

            struct v2f
            {
                float4 svpos : SV_POSITION;
                float3 normal : NORMAL;
                //float3 vertWorldPos : TEXCOORD1;

            };

            float4 _CenterColor;
            float _CenterScale;
            float _Iterations;
            float _CenterBrightness;
            fixed _DecayFactor;
            //float4 center = mul(unity_ObjectToWorld, float4(0, 0, 0, 1));
            v2f vert(a2v i)
            {

                v2f o;
                i.pos.xyz += i.normal * _CenterScale;
                o.svpos = UnityObjectToClipPos(i.pos);
                o.normal = i.normal;
                return o;
            }

            float4 frag(v2f u) : COLOR
            {
                //·¨Ïß
                u.normal = normalize(u.normal);

                float dots = 1 - dot(float3(0, 0, 1), u.normal);
                float strength = pow(dots, _Iterations);
                float4 color = _CenterColor;
                color.xyz *= _CenterBrightness * strength*_DecayFactor;
                return color;
            }



            ENDCG
         }

    }
    FallBack "Diffuse"
}
