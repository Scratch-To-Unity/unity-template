using System.Collections.Generic;
using UnityEngine;
using System;

namespace Scratch
{
    public partial class ScratchSprite : MonoBehaviour
    {
        /// <summary>
        /// <para>Returns the asked <paramref name="letter"/> without throwing any errors.</para>
        /// </summary>
        public string LetterOf(object letter, object input)
        {
            int idx = ToInt(letter) - 1;
            string value = input.ToString();
            if (idx < 0 || idx > value.Length - 1)
            {
                idx = 0;
            }
            return value[idx].ToString();
        }
        /// <summary>
        /// <para>Returns the asked <paramref name="letter"/> without throwing any errors.</para>
        /// </summary>
        public string LetterOf(double letter, string input)
        {
            int idx = ToInt(letter) - 1;
            string value = input.ToString();
            if (idx < 0 || idx > value.Length - 1)
            {
                idx = 0;
            }
            return value[idx].ToString();
        }

        public void RemoveAt<T>(List<T> list, double index)
        {
            int intIndex = ToInt(index) - 1;
            if (intIndex < 0 || intIndex > list.Count - 1)
            {
                return;
            }
            list.RemoveAt(intIndex);
        }
        /// <summary>
        /// Insert a <paramref name="value"/> at <paramref name="index"/> in a scratch <paramref name="list"/>.
        /// </summary>
        public void Insert<T>(List<T> list, T value, double index)
        {
            list.Insert(Math.Clamp(ToInt(index) - 1, 0, Math.Max(list.Count - 1, 0)), value);
        }

        /// <summary>
        /// <para>Replace the asked element of a <paramref name="list"/> by another <paramref name="value"/>. Doesn't throw any errors.</para>
        /// </summary>
        public void ReplaceItem<T>(List<T> list, double index, T value)
        {
            int idx = (int)Math.Round(index) - 1;
            if (idx < 0 || idx > list.Count - 1)
            {
                print("Index out of range");
                return;
            }
            list[idx] = value;
        }

        /// <summary>
        /// <para>Returns the asked element of a <paramref name="list"/>. Doesn't throw any errors.</para>
        /// </summary>
        public T ElementOf<T>(List<T> list, double index)
        {
            int idx = (int)Math.Round(index) - 1;
            if (idx < 0 || idx > list.Count - 1)
            {
                print("Index out of range");
                if (list.Count > 0)
                {
                    return default;
                }
                else
                {
                    return default;
                }
            }
            return list[idx];
        }
    }
}
