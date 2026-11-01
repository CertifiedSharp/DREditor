//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections.Generic;

namespace DREditor.UI
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
    public static class CarouselListExtentions
    {
        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="shiftBy"></param>
        /// <returns></returns>
        public static List<T> CarouselLeft<T>(this List<T> list, int shiftBy)
        {
            if (list.Count <= shiftBy)
                return list;

            var result = list.GetRange(shiftBy, list.Count - shiftBy);

            result.AddRange(list.GetRange(0, shiftBy));

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <param name="shiftBy"></param>
        /// <returns></returns>
        public static List<T> CarouselRight<T>(this List<T> list, int shiftBy)
        {
            if (list.Count <= shiftBy)
                return list;

            var result = list.GetRange(list.Count - shiftBy, shiftBy);

            result.AddRange(list.GetRange(0, list.Count - shiftBy));

            return result;
        }
    }
}