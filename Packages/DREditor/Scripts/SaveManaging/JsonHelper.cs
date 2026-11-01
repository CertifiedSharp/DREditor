//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using UnityEngine;

namespace DREditor.SaveManaging
{
    /// <summary>
    ///
    /// </summary>
    public static class JsonHelper
    {
        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="json"></param>
        /// <returns></returns>
        public static T[] FromJson<T>(string json)
        {
            Wrapper<T> wrapper = JsonUtility.FromJson<Wrapper<T>>(json);

            return wrapper.Items;
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="array"></param>
        /// <param name="prettyPrint"></param>
        /// <returns></returns>
        public static string ToJson<T>(T[] array, bool prettyPrint = true)
        {
            Wrapper<T> wrapper = new(array);

            return JsonUtility.ToJson(wrapper, prettyPrint);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        [Serializable]
        sealed class Wrapper<T>
        {
            /// <summary>
            ///
            /// </summary>
            internal T[] Items;

            internal Wrapper(T[] values)
            {
                Items = values;
            }
        }
    }
}