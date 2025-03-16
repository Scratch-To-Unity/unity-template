Shader "Unlit/The Line"
{
    Properties
    {
        //a_lineThicknessAndLength ("Line Thickness and Length", Vector) = (5, 100, 0, 0) //thickness on x, length on y
        //a_lineColor ("Line Color", Color) = (1, 0.5, 0.2, 1)
        //a_penPoints ("Pen Points", Vector) = (0, 0, 1, 1) //x, y, diffx, diffy
        u_stageSize ("Stage Dimensions", Vector) = (480, 360, 0, 0) // a uniform
    }
    SubShader
    {
        Tags { 
            "Queue"="Transparent" 
            "RenderType"="Transparent" 
            //"RenderPipeline" = "UniversalRenderPipeline"
        }

        Pass
        {
            // Blend One One // Additive

            // Blend DstColor Zero // Multiply

            Blend SrcAlpha OneMinusSrcAlpha


            CGPROGRAM
            #pragma vertex vert
            #pragma fragment frag
            #pragma multi_compile_instancing

            //#include "UnityCG.cginc"

            //Attributes input from C#
            //float2 a_lineThicknessAndLength; //thickness on x, length on y
            //float4 a_lineColor; //color
            //float4 a_penPoints; //p1 on xy, p2 is on zw
            float2 a_position;

            uniform float2 u_stageSize;

            float epsilon = 0.0001;

            StructuredBuffer<float2> a_lineThicknessAndLength_buffer;
            StructuredBuffer<float4> a_lineColor_buffer;
            StructuredBuffer<float4> a_penPoints_buffer;

            struct MeshData // per-vertex
            {
                float4 vertex : POSITION;
                float2 uv : TEXCOORD0;
            };

            struct Interpolators //varying
            {
                float2 uv : TEXCOORD0;
                float4 vertex : SV_POSITION;

                //Varying parameters sent from vertex shader
                float4 v_lineColor : TEXCOORD1;
                float v_lineThickness : TEXCOORD2;
                float v_lineLength : TEXCOORD3;
                float4 v_penPoints : TEXCOORD4;
            };

            float mix(float x, float y, float a) {
                return x * (1.0 - a) + y * a;
            }

            Interpolators vert (MeshData mesh, uint instance_id : SV_InstanceID)// vertex shader
            {
                Interpolators i;

                float2 a_lineThicknessAndLength = a_lineThicknessAndLength_buffer[instance_id];
                float4 a_lineColor = a_lineColor_buffer[instance_id];
                float4 a_penPoints = a_penPoints_buffer[instance_id];

                //Testing purpose
                //a_penPoints.x = -10 + instance_id / 1000;
                //a_penPoints.z = 0;
                //a_lineThicknessAndLength.x = 5;

                //a_lineThicknessAndLength = float2(30, 100);
                ////a_lineColor = float4(0.1, 1.0, 0.3, 1.0);
                //a_penPoints = float4(instance_id, instance_id, 100, 100);


                float2 v_texCoord;// = mesh.uv;
                mesh.vertex.x *= 480;
                mesh.vertex.y *= 360;
                a_position = (UnityObjectToClipPos(mesh.vertex) + float2(1, 1)) / float2(2, 2);
                
                //is it clipspace?? UnityObjectToClipPos()
                float2 position = a_position;
                float expandedRadius = (a_lineThicknessAndLength.x * 0.5) + 1.4142135623730951;

                v_texCoord.x = mix(0.0, a_lineThicknessAndLength.y + (expandedRadius * 2.0), a_position.x) - expandedRadius;
                v_texCoord.y = ((a_position.y - 0.5) * expandedRadius) + 0.5;

                position.x *= a_lineThicknessAndLength.y + (2.0 * expandedRadius);
                position.y *= 2.0 * expandedRadius;

                position -= expandedRadius;

                float2 pointDiff = a_penPoints.zw;

                pointDiff.x = (abs(pointDiff.x) < epsilon && abs(pointDiff.y) < epsilon) ? epsilon : pointDiff.x;

                float2 normalized = pointDiff / max(a_lineThicknessAndLength.y, epsilon);
                position = mul(float2x2(normalized.x, normalized.y, -normalized.y, normalized.x), position);

                position += a_penPoints.xy;

                position *= 2.0 / u_stageSize;
                i.vertex = float4(position, 0, 1);
                i.uv = v_texCoord;
                i.v_lineColor = a_lineColor;
                i.v_lineThickness = a_lineThicknessAndLength.x;
                i.v_lineLength = a_lineThicknessAndLength.y;
                i.v_penPoints = a_penPoints;
                
                return i;
            }

            half4 frag (Interpolators i) : SV_Target // fragment shader
            {
                float d = ((i.uv.x - clamp(i.uv.x, 0.0, i.v_lineLength)) * 0.5) + 0.5;

                float stroke = distance(float2(0.5, 0), float2(d, i.uv.y)) * 2.0;

                stroke -= ((i.v_lineThickness - 1.0) * 0.5);

                float4 fragColor = i.v_lineColor * clamp(1.0 - stroke, 0.0, 1.0);

                fragColor.a = floor(fragColor.a);

                return fragColor;
            }
            ENDCG
        }
    }
}
