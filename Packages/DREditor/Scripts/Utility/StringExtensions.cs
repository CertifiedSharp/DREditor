//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using System.Collections.Generic;

namespace DREditor.Utility
{
    /// <summary>
    /// Utility extention class for strings.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Applies multiple replace functions using <paramref name="lookFors"/> and replacing each one found with <paramref name="replaceWith"/>.
        /// </summary>
        /// <param name="text">The text to replace.</param>
        /// <param name="lookFors">The List of different strings that should be replaced if found in <paramref name="text"/>.</param>
        /// <param name="replaceWith">The string to replace all <paramref name="lookFors"/> instances with.</param>
        /// <returns>The <paramref name="text"/> but it's replaced all instances of 
        /// <paramref name="lookFors"/> with <paramref name="replaceWith"/>.</returns>
        public static string ReplaceMany(this string text, List<string> lookFors, string replaceWith)
        {
            foreach (var item in lookFors)
                text = text.Replace(item, replaceWith);

            return text;
        }
    }
}