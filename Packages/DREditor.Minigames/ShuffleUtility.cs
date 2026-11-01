//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections.Generic;

namespace DREditor.Minigames
{
    /// <summary>
    ///
    /// </summary>
    public static class ShuffleUtility
    {
        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        /// <remarks></remarks>
        public static void Shuffle<T>(this IList<T> list)
        {
            int number = list.Count;

            var rng = ThreadSafeSimpleRandom.Instance;

            while (number > 1)
            {
                number--;

                int k = rng.Next(number + 1);

                (list[k], list[number]) = (list[number], list[k]);
            }
        }
    }
}