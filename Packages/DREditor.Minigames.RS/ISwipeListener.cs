//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

namespace DREditor.Minigames.RS
{
    /// <summary>
    ///
    /// </summary>
    public interface ISwipeListener
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="swipeStart"></param>
        /// <param name="swipeEnd"></param>
        void OnSwiped(Vector3 swipeStart, Vector3 swipeEnd);
    }
}