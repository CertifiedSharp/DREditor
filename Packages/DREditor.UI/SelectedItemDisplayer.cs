//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

namespace DREditor.UI
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
    public abstract class SelectedItemDisplayer : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="index"></param>
        /// <remarks></remarks>
        public abstract void DisplaySelectedItem(int index);
    }
}