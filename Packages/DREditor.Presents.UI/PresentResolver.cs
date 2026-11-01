//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections;
using UnityEngine;

namespace DREditor.Presents.UI
{
    /// <summary>
    ///
    /// </summary>
    public abstract class PresentResolver : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="gavePresent"></param>
        /// <param name="selectedPresentIndex"></param>
        /// <returns></returns>
        public abstract IEnumerator ResolvePresentSelection(bool gavePresent, int selectedPresentIndex);
    }
}