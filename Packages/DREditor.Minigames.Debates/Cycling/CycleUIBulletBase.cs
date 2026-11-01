//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

namespace DREditor.Minigames.Debates.Cycling
{
    /// <summary>
    ///
    /// </summary>
    public abstract class CycleUIBulletBase : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="title"></param>
        public abstract void UpdateBulletText(string title);

        /// <summary>
        ///
        /// </summary>
        /// <param name="isActive"></param>
        public abstract void DisplayBulletActive(bool isActive);
    }
}