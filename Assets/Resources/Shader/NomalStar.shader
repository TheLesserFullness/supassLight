Shader "StarShader/NomalStar"
{
    Properties
    {
        //_Color ("Color", Color) = (1,1,1,1)
        //_MainTex ("Albedo (RGB)", 2D) = "white" {}
        //_Glossiness("Smoothness", Range(0,1)) = 0.5
        //_Metallic("Metallic", Range(0,1)) = 0.0
        _CenterColor("CenterColor",Color) = (1,1,1,1)
        _CenterScale("_CenterScale",Range(-1,10)) = 0.1
        _OutterScale("OutterScale",Range(0,1)) = 0.1
        _OutLineColor("BorderColor",Color) = (1,1,1,1)
        _CenterStrength("CenterSize",Range(-1,10)) = 0.5
        _QiangDu("QiangDu",Range(0,100)) = 0.5
        _QiangDu1("QiangDu1",Range(0,100)) = 1

    }
    SubShader
    {
         Cull Front
         //Tags { "RenderType" = "Opaque" }
         //LOD 200

         Pass
         {
            //Blend SrcAlpha One
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
                float3 vertWorldPos : TEXCOORD1;

            };

            float4 _CenterColor;
            fixed _CenterScale;
            float _CenterStrength;
            float _QiangDu;
            //float4 center = mul(unity_ObjectToWorld, float4(0, 0, 0, 1));
            v2f vert(a2v i)
            {
                
                v2f o;
                i.pos.xyz += i.normal * _CenterScale;
                o.svpos = UnityObjectToClipPos(i.pos);
                o.normal = i.normal;
                o.vertWorldPos = mul(unity_ObjectToWorld, i.pos).xyz;
                return o;
            }

            float4 frag(v2f u) : COLOR
            {
                //法线
                u.normal = normalize(u.normal);

                float dots = dot(float3(0, 0, 1), u.normal);
                float strength = pow(dots, _CenterStrength);//+max(dots, 0.7) - 0.7;
                float4 color = _CenterColor;
                color.xyz *= _QiangDu * (0.4*strength);
                return color;
            }



            ENDCG
        }

        Cull Front
        Pass
         {
                //Blend SrcAlpha One
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
                    float3 vertWorldPos : TEXCOORD2;

                };

                float4 _OutLineColor;
                fixed _OutterScale;
                float _QiangDu1;
                //float4 center = mul(unity_ObjectToWorld, float4(0, 0, 0, 1));
                v2f vert(a2v i)
                {

                    v2f o;
                    i.pos.xyz += i.normal * _OutterScale;
                    o.svpos = UnityObjectToClipPos(i.pos);
                    o.normal = i.normal;
                    o.vertWorldPos = mul(unity_ObjectToWorld, i.pos).xyz;
                    return o;
                }

                float4 frag(v2f u) : COLOR
            {
                    //法线
                u.normal = normalize(u.normal);

                float dots = dot(float3(0, 0, 1), u.normal);
                float strength = pow(dots, _QiangDu1);
                float4 color = _OutLineColor;
                color.xyz *= strength - 2*pow(0.5 * strength,2);

                return color;
            }



            ENDCG
        }
            
    }
    FallBack "Diffuse"
}
