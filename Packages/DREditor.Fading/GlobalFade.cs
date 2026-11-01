//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using DREditor.Services;
using System.Collections;
using UnityEngine;

namespace DREditor.Fading
{
    /// <summary>
    ///
    /// </summary>
    public abstract class GlobalFade : ServiceHolder<GlobalFade>
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent screenIsDark = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected bool isDark = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public bool isTransitioning = false;


        /// <summary>
        ///
        /// </summary>
        public bool IsDark
        {
            get
            {
                if (screenIsDark != null && screenIsDark.Value != isDark)
                    isDark = IsUIDark();
                
                return isDark;
            }

            set
            {
                isDark = value;

                if (screenIsDark != null)
                    screenIsDark.SetValue(isDark);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected abstract bool IsUIDark();

        /// <summary>
        ///
        /// </summary>
        /// <param name="time"></param>
        /// <param name="white"></param>
        /// <param name="overrule"></param>
        public abstract void FadeTo(float time, bool white = false, bool overrule = false);

        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        /// <param name="time"></param>
        /// <param name="white"></param>
        /// <param name="overrule"></param>
        public abstract IEnumerator FadeToRoutine(float time, bool white = false, bool overrule = false);

        /// <summary>
        ///
        /// </summary>
        /// <param name="time"></param>
        /// <param name="white"></param>
        /// <param name="overrule"></param>
        public abstract void FadeOut(float time, bool white = false, bool overrule = false);

        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        /// <param name="time"></param>
        /// <param name="white"></param>
        /// <param name="overrule"></param>
        public abstract IEnumerator FadeOutRoutine(float time, bool white = false, bool overrule = false);
    }
    /* Is Transitioning was to fix a bug where going from investigation to trial with
         * the last line being a CG or Video would fade an additional time, otherwise Fade works perfectly fine
         * so if the game has any fade issues after 10/20/22 then just remove it and find a new way to fix 
         * going from investigation to trial prep screen during CGs on the final line.
         * 
         */
}