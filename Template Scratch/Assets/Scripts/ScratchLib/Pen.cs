using Scratch.PenTools;
using UnityEngine;

namespace Scratch
{
    public partial class ScratchSprite
    {
        [Header("Pen")]
        public float penWidth = 5;
        public Color penColor = Color.blue;
        private PenDrawer pen;
        private bool isPenDown = false;

        private float length;
        private float diffX;
        private float diffY;
        private Vector2 displacement = new Vector2(0, 0.0001f);

        /// <summary>
        /// Add a line to the line stack, using data-oriented arrays. All lines will be rendered with an instanced shader in late update.
        /// </summary>
        public void DrawLineShader(Vector2 startPosition, Vector2 endPosition, Color color, float width)
        {
            if (pen.currentLine > 199999)
            {
                Debug.LogError("Too much lines");
                return;
            }
            endPosition += displacement;
            length = Vector2.Distance(endPosition, startPosition);
            diffX = endPosition.x - startPosition.x;
            diffY = endPosition.y - startPosition.y;

            pen.a_lineColor_array[pen.currentLine] = color;
            pen.a_lineThicknessAndLength_array[pen.currentLine] = new Vector2(width + 1, length);
            pen.a_penPoints_array[pen.currentLine] = new Vector4(startPosition.x, -startPosition.y, diffX, diffY);

            pen.currentLine++;
        }

        /// <summary>
        /// Clear the pen canvas.
        /// </summary>
        public void Clear()
        {
            RenderTexture renderTexture = pen.canvasTexture;
            Graphics.SetRenderTarget(renderTexture);
            GL.Clear(true, true, Color.clear);
            pen.ResetBuffers();
        }
        /// <summary>
        /// The sprite won't draw when moving
        /// </summary>
        public void PenUp()
        {
            isPenDown = false;
        }
        /// <summary>
        /// When moving, the sprite will draw.
        /// </summary>
        public void PenDown()
        {
            isPenDown = true;
            OnMove(transform.position);
        }

        /// <summary>
        /// Stamp the sprite onto the pen canvas.
        /// </summary>
        public void Stamp()
        {
            GameObject stamp = new GameObject("stamp");
            stamp.transform.position = transform.position;
            stamp.transform.rotation = transform.rotation;
            stamp.transform.localScale = transform.localScale;
            SpriteRenderer renderer = stamp.AddComponent<SpriteRenderer>();
            renderer.sprite = spriteRenderer.sprite;
            stamp.layer = 6;
            //stamp.AddComponent<DestroyImmediate>();
            stamp.tag = "Pen_stamp";
            renderer.sortingOrder = pen.currentLayer;
            pen.currentLayer++;
        }
        /// <summary>
        /// Change a color parameter by <paramref name="value"/>.
        /// <para>
        /// The <paramref name="type"/> can be "color", "saturation", "brightness" or "transparency". They correspond to HSVA.
        /// </para>
        /// </summary>
        public void ChangePenColor(ColorParam type, double value)
        {
            value /= 100;
            float H;
            float S;
            float V;
            float A = penColor.a;
            Color.RGBToHSV(penColor, out H, out S, out V);
            switch (type)
            {
                case ColorParam.color:
                    H += ToFloat(value);
                    break;
                case ColorParam.saturation:
                    S += ToFloat(value);
                    break;
                case ColorParam.brightness:
                    V += ToFloat(value);
                    break;
                case ColorParam.transparency:
                    A += ToFloat(value);
                    break;
                default:
                    UnityEngine.Debug.LogError("Unknown ColorChange Command");
                    break;
            }
            penColor = Color.HSVToRGB(H, S, V);
            penColor.a = A;
        }
        /// <summary>
        /// Set a color parameter by <paramref name="value"/>.
        /// <para>
        /// The <paramref name="type"/> can be "color", "saturation", "brightness" or "transparency". They correspond to HSVA.
        /// </para>
        /// </summary>
        public void SetPenColor(ColorParam type, float value)
        {
            value /= 100;
            float H;
            float S;
            float V;
            float A = penColor.a;
            Color.RGBToHSV(penColor, out H, out S, out V);
            switch (type)
            {
                case ColorParam.color:
                    H = value;
                    break;
                case ColorParam.saturation:
                    S = value;
                    break;
                case ColorParam.brightness:
                    V = value;
                    break;
                case ColorParam.transparency:
                    A = value;
                    break;
                default:
                    UnityEngine.Debug.LogError("Unknown ColorChange Command");
                    break;
            }
            penColor = Color.HSVToRGB(H, S, V);
            penColor.a = A;
        }
        /// <summary>
        /// Converts a hex code to <seealso cref="Color"/>.
        /// </summary>
        public Color HexToColor(string hex)
        {
            Color color;
            if (ColorUtility.TryParseHtmlString(hex, out color))
            {
                return color;
            }
            else
            {
                //if (hex.Contains("+"))
                //{
                //    return Color.white;
                //}
                int hexInt = 0;
                float hexFloat = 0;
                if (float.TryParse(hex, out hexFloat))
                {
                    hexInt = (int)hexFloat;
                    return new Color32(
                    (byte)((hexInt >> 16) & 255),
                    (byte)((hexInt >> 8) & 255),
                    (byte)(hexInt & 255),
                    255);
                }
                else
                {
                    // Returns white if parsing fails
                    UnityEngine.Debug.LogWarning("Failed to parse hex code: " + hex);
                    return Color.black;
                }
            }
        }
        /// <summary>
        /// Scratch color parameter for set and add. Used in <seealso cref="ChangePenColor(float, ColorParam)"/>, and <seealso cref="SetPenColor(ColorParam, float)"/>.
        /// </summary>
        public enum ColorParam
        {
            color,
            saturation,
            brightness,
            transparency
        }
    }
}

