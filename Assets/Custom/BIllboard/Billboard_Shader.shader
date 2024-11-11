Shader "Custom/URP/BillboardShader"
{
    Properties
    {
       [MainTexture] _MainTex_Front ("Texture_Front", 2D) = "white" {}
       [MainTexture] _MainTex_Back ("Texture_Back", 2D) = "white" {}
       [MainTexture] _MainTex_Side ("Texture_Side", 2D) = "white" {}
    }
    SubShader
    {
        Tags { "RenderType"="Transparent" "Queue" = "Transparent" "RenderPipeline"="UniversalPipeline"  }
        Blend SrcAlpha OneMinusSrcAlpha
    
        Pass
        {
            HLSLPROGRAM
            #pragma vertex vert
            #pragma fragment frag

            #include "Packages/com.unity.render-pipelines.universal/ShaderLibrary/Core.hlsl"

            struct Attributes
            {
                float4 positionOS : POSITION;  //vertex =  positionOS 
                float2 uv : TEXCOORD0;
            };

            struct Varyings
            {
                float2 uv : TEXCOORD0;
                float4 positionHCS : SV_POSITION;  //vertex =  positionhc5 
                float angle : TEXCOORD1;
            };

            TEXTURE2D(_MainTex_Front);
            TEXTURE2D(_MainTex_Back);
            TEXTURE2D(_MainTex_Side);
            SAMPLER(sampler_MainTex_Front);
            SAMPLER(sampler_MainTex_Back);
            SAMPLER(sampler_MainTex_Side);

            CBUFFER_START(UnityPerMaterial)
            float4 _MainTex_Front_ST;
            CBUFFER_END
            

            Varyings vert (Attributes v)
            {
                Varyings o;
                
                float4 origin = float4(0,0,0,1);
                float4 world_origin = mul(unity_ObjectToWorld, origin);
                float4 view_origin = mul(UNITY_MATRIX_V, world_origin);
                float4 world_to_view_translation = view_origin - world_origin;

                float4 world_pos = mul(unity_ObjectToWorld, v.positionOS); // Object to World
                float4 view_pos = world_pos + world_to_view_translation;              // World to View
                float4 clip_pos = mul(UNITY_MATRIX_P, view_pos);    

                o.positionHCS = clip_pos;
                o.uv = TRANSFORM_TEX(v.uv, _MainTex_Front); 

                float3 frontVec = mul((float3x3)UNITY_MATRIX_V, (float3)unity_ObjectToWorld[2]);								//Set front vector
                float3 rotVec = mul(unity_ObjectToWorld, float4(0,0,0,1)).xyz
                    - _WorldSpaceCameraPos;							//Calculate current rotation vector
                    
                half grad = atan2(rotVec.x, rotVec.z);
                
                o.angle = grad;
                //o.angle = grad;

                /*float3 frontVec = mul((float3x3)UNITY_MATRIX_V, (float3)unity_ObjectToWorld[2]);
    
                // Calculate the view direction from the camera to the object in world space
                float3 view_dir = normalize(world_origin.xyz - _WorldSpaceCameraPos);

                // Calculate the angle using the dot product
                o.angle = dot(view_dir, frontVec);*/

                return o;
            }

            half4 frag (Varyings i) : SV_Target
            {
                // sample the texture
                half4 col = half4(0,0,0,0);
                
                
                if (abs(i.angle) < radians(45) || abs(i.angle - 2 * 3.14159) < radians(45)) 
                {
                    col = SAMPLE_TEXTURE2D(_MainTex_Back ,sampler_MainTex_Back , i.uv);
                }
                else if (abs(i.angle - 3.14159) < radians(45))  
                {
                    col = SAMPLE_TEXTURE2D(_MainTex_Front, sampler_MainTex_Front , i.uv);
                }
                else
                {
                    col = SAMPLE_TEXTURE2D(_MainTex_Side, sampler_MainTex_Side, i.uv);
                }

                //col = SAMPLE_TEXTURE2D(_MainTex_Front, sampler_MainTex_Front , i.uv);
                return col;
            }
            ENDHLSL
        }
    }
}