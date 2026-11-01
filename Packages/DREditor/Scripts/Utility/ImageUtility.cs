//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using UnityEngine;
using UnityEngine.UI;

namespace DREditor.Utility
{
    public static class ImageUtility
    {
        public static void SetFade(this Graphic i, float alpha)
        {
            i.color = new Color(i.color.r, i.color.g, i.color.b, alpha);
        }

        public static void SetImageColorIgnoreAlpha(this Image i, Color c)
        {
            i.color = new Color(c.r, c.g, c.b, i.color.a);
        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="images"></param>
        public static void SetActiveBySprite(params Image[] images)
        {
            foreach (var image in images)
                image.SetActiveBySprite();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="image"></param>
        public static void SetActiveBySprite(this Image image)
        {
            if (image != null)
                image.gameObject.SetActive(image.sprite != null);
        }
    }
}