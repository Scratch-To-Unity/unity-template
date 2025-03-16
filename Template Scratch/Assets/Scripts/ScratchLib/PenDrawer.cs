using System;
using UnityEngine;
using UnityEngine.UI;

namespace Scratch
{
    namespace PenTools
    {
        /// <summary>
        /// A class used by <seealso cref="ScratchSprite"/> to render pen strokes and stamping.
        /// </summary>
        public class PenDrawer : MonoBehaviour
        {
            [Header("Camera & Textures")]
            public Camera stampCamera;
            public RawImage canvas;
            public RenderTexture canvasTexture;
            public int textureWidth = 480;
            public int textureHeight = 360;

            [Header("Shader")]
            public Mesh quadMesh;
            public Material TheLineMaterial;

            private uint[] _args = { 0, 0, 0, 0, 0 };
            private int _count = 200000;

            [Header("Pen drawing")]
            public int currentLayer = 1;
            public uint currentLine;

            private ComputeBuffer a_lineThicknessAndLength_buffer, a_lineColor_buffer, a_penPoints_buffer;
            private ComputeBuffer _argsBuffer;
            [NonSerialized]
            public Vector4[] a_lineColor_array, a_penPoints_array;
            [NonSerialized]
            public Vector2[] a_lineThicknessAndLength_array;

            private void Awake()
            {
                canvasTexture = new RenderTexture(textureWidth, textureHeight, 0, RenderTextureFormat.ARGBFloat);
                canvasTexture.enableRandomWrite = false;
                canvasTexture.wrapMode = TextureWrapMode.Clamp;
                canvasTexture.filterMode = FilterMode.Bilinear;
                canvasTexture.Create();
                canvas.texture = canvasTexture;
                Camera.main.targetTexture = canvasTexture;


                a_lineColor_array = new Vector4[_count];
                a_lineThicknessAndLength_array = new Vector2[_count];
                a_penPoints_array = new Vector4[_count];

                ///Setting up argsBuffer
                _argsBuffer = new ComputeBuffer(1, _args.Length * sizeof(uint), ComputeBufferType.IndirectArguments);
            }

            private void LateUpdate()
            {
                RenderPen();
            }
            /// <summary>
            /// Reset pen arrays.
            /// </summary>
            public void ResetBuffers()
            {
                Array.Clear(a_lineColor_array, 0, _count);
                Array.Clear(a_lineThicknessAndLength_array, 0, _count);
                Array.Clear(a_penPoints_array, 0, _count);
                //if (a_lineColor_array[0].x == 0)
                //{
                //    return;
                //}
                //a_lineColor_array = new Vector4[_count];
                //a_lineThicknessAndLength_array = new Vector2[_count];
                //a_penPoints_array = new Vector4[_count];
            }

            /// <summary>
            /// Render all lines using shader instancing.
            /// </summary>
            public void RenderPen()
            {
                Graphics.SetRenderTarget(canvasTexture);

                //Release buffers
                a_lineColor_buffer?.Release();
                a_lineThicknessAndLength_buffer?.Release();
                a_penPoints_buffer?.Release();

                //Create buffer
                a_lineColor_buffer = new ComputeBuffer(_count, 16);
                a_lineThicknessAndLength_buffer = new ComputeBuffer(_count, 8);
                a_penPoints_buffer = new ComputeBuffer(_count, 16);

                //Set buffer
                a_lineColor_buffer.SetData(a_lineColor_array);
                a_lineThicknessAndLength_buffer.SetData(a_lineThicknessAndLength_array);
                a_penPoints_buffer.SetData(a_penPoints_array);

                //Send buffer to material
                TheLineMaterial.SetBuffer("a_lineColor_buffer", a_lineColor_buffer);
                TheLineMaterial.SetBuffer("a_lineThicknessAndLength_buffer", a_lineThicknessAndLength_buffer);
                TheLineMaterial.SetBuffer("a_penPoints_buffer", a_penPoints_buffer);

                //Setup rendering options
                _args[0] = quadMesh.GetIndexCount(0);
                _args[1] = (uint)currentLine;
                _args[2] = quadMesh.GetIndexStart(0);
                _args[3] = quadMesh.GetBaseVertex(0);
                _argsBuffer.SetData(_args);

                //Render lines
                if(currentLine > 0)
                {
                    Graphics.DrawMeshInstancedIndirect(quadMesh, 0, TheLineMaterial, new Bounds(Vector3.zero, new Vector3(480, 360, 50)), _argsBuffer, layer: 3);
                }

                //Render stamps using the GameObject system.
                //Note: update to shader instancing
                Timing.StartWatch();
                stampCamera.Render();
                Timing.StopWatch("StampCameraRendering");
                var stamps = GameObject.FindGameObjectsWithTag("Pen_stamp");
                foreach (GameObject stamp in stamps)
                {
                    DestroyImmediate(stamp);
                }

                //Reset line counters
                currentLine = 0;
                currentLayer = 0;
            }

            // Don't forget to release the texture when the script is destroyed.
            private void OnDestroy()
            {
                if (canvasTexture != null)
                {
                    canvasTexture.Release();
                    canvasTexture = null;
                }
            }

            private void OnDisable()
            {
                a_lineColor_buffer?.Release();
                a_lineThicknessAndLength_buffer?.Release();
                a_penPoints_buffer?.Release();
                _argsBuffer?.Release();
            }
        }
    }
}