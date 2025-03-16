using System;
using UnityEngine;

namespace Scratch
{
    public partial class ScratchSprite
    {
        /// <summary>
        /// <para>Try to convert an <see cref="object"/> to <see cref="double"/>. Doesn't throw any errors.</para>
        /// </summary>
        public static double ToDouble(object inputValue)
        {
            try
            {
                return (double)inputValue;
            }
            catch (Exception)
            {
                string input = inputValue.ToString();

                //try with int.parse first to convert hexadecimals

                double convertedFloat = 0f;
                if (double.TryParse(input, out convertedFloat))
                {
                    return convertedFloat;
                }
                else
                {
                    return ToDouble(input);
                }
            }
        }
        public static double ToDouble(bool inputValue)
        {
            return inputValue ? 1 : 0;
        }

        /// <summary>
        /// <para>Try to convert a <see cref="string"/> to <see cref="double"/>. Doesn't throw any errors.</para>
        /// </summary>
        public static double ToDouble(string inputValue)
        {
            try
            {
                return Convert.ToInt32(inputValue, 16);
            }
            catch (Exception)
            {
                switch (inputValue)
                {
                    case "Infinity":
                        return Mathf.Infinity;
                    case "+Infinity":
                        return Mathf.Infinity;
                    case "-Infinity":
                        return Mathf.NegativeInfinity;
                    case "True":
                        return 1;
                    case "False":
                        return 0;
                    default:
                        break;
                }
                Debug.LogError("Couldn't cast to float.");
                return 0;
            }
        }
        /// <summary>
        /// <para>Returns the <paramref name="inputValue"/>.</para>
        /// </summary>
        public static double ToDouble(double inputValue)
        {
            return inputValue;
        }

        /// <summary>
        /// <para>Returns the <paramref name="inputValue"/> casted to <see cref="float"/>.</para>
        /// </summary>
        public static double ToDouble(int inputValue)
        {
            return inputValue;
        }
        /// <summary>
        /// <para>Try to convert an <see cref="object"/> to <see cref="float"/>. Doesn't throw any errors.</para>
        /// </summary>
        //[BurstCompile]
        public static float ToFloat(object inputValue)
        {
            try
            {
                return (float)inputValue;
            }
            catch (Exception)
            {
                string input = inputValue.ToString();

                //try with int.parse first to convert hexadecimals

                float convertedFloat = 0f;
                if (float.TryParse(input, out convertedFloat))
                {
                    return convertedFloat;
                }
                else
                {
                    return ToFloat(input);
                }
            }
        }
        public static float ToFloat(bool inputValue)
        {
            return inputValue ? 1 : 0;
        }

        /// <summary>
        /// <para>Try to convert a <paramref name="string"/> to <paramref name="float"/>. Doesn't throw any errors.</para>
        /// </summary>
        public static float ToFloat(string inputValue)
        {
            try
            {
                return Convert.ToInt32(inputValue, 16);
            }
            catch (Exception)
            {
                switch (inputValue)
                {
                    case "Infinity":
                        return Mathf.Infinity;
                    case "+Infinity":
                        return Mathf.Infinity;
                    case "-Infinity":
                        return -Mathf.Infinity;
                    case "True":
                        return 1f;
                    case "False":
                        return 0f;
                    default:
                        break;
                }
                Debug.LogError("Couldn't cast to float.");
                return 0;
            }
        }
        /// <summary>
        /// <para>Returns the <paramref name="inputValue"/>.</para>
        /// </summary>
        public static float ToFloat(float inputValue)
        {
            return inputValue;
        }
        public static float ToFloat(double inputValue)
        {
            return (float)inputValue;
        }

        /// <summary>
        /// <para>Returns the <paramref name="inputValue"/> casted to float.</para>
        /// </summary>
        public static float ToFloat(int inputValue)
        {
            return inputValue;
        }
        public static float antiNaN(float inputValue)
        {
            return float.IsNaN(inputValue) ? 0f : inputValue;
        }
        public static double antiNaN(double inputValue)
        {
            return double.IsNaN(inputValue) ? 0f : inputValue;
        }
        /// <summary>
        /// <para>Try to convert an <paramref name="object"/> to <paramref name="int"/>. Doesn't throw any errors.</para>
        /// </summary>
        public static int ToInt(object inputValue)
        {
            try
            {
                return (int)Math.Round((double)inputValue);
            }
            catch (Exception)
            {
                Debug.LogError("Couldn't cast to int.");
                return 0;
            }
        }
        /// <summary>
        /// <para>Try to convert a <paramref name="string"/> to <see cref="int"/>. Doesn't throw any errors.</para>
        /// </summary>
        public static int ToInt(string inputValue)
        {
            try
            {
                return Mathf.RoundToInt(Convert.ToInt64(inputValue));
            }
            catch (Exception)
            {
                Debug.LogError("Couldn't cast to int.");
                throw;
            }
        }
        /// <summary>
        /// <para>Returns the <paramref name="inputValue"/>.</para>
        /// </summary>
        public static int ToInt(int inputValue)
        {
            return inputValue;
        }
        /// <summary>
        /// <para>Round the <see cref="double"/> to <see cref="int"/>.</para>
        /// </summary>
        public static int ToInt(double inputValue)
        {
            return (int)Math.Round(inputValue);
        }
    }
}
