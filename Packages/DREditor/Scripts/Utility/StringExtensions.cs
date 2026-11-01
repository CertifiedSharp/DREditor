//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using System.Collections.Generic;

namespace DREditor.Utility
{
    /// <summary>
    ///
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="text"></param>
        /// <param name="lookFors"></param>
        /// <param name="replaceWith"></param>
        /// <returns></returns>
        public static string ReplaceMany(this string text, List<string> lookFors, string replaceWith)
        {
            foreach (var item in lookFors)
                text = text.Replace(item, replaceWith);

            return text;
        }
    }
}