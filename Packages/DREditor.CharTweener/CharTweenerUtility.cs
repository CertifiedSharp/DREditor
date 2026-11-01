//Author: Copyright (c) 2021 Matthew Dechatech under MIT License
#if DOTWEEN
using System.Collections.Generic;
using TMPro;

namespace CharTween
{
    public static class CharTweenerUtility
    {
        private static readonly Dictionary<TMP_Text, CharTweener> Tweeners = new Dictionary<TMP_Text, CharTweener>();

        /// <summary>
        ///
        /// </summary>
        public static CharTweener GetCharTweener(this TMP_Text text)
        {
            if (Tweeners.TryGetValue(text, out CharTweener tweener))
                return tweener;

            tweener = text.gameObject.AddComponent<CharTweener>();
            tweener.Initialize(text);
            Tweeners.Add(text, tweener);
            return tweener;
        }
    }
}
#endif