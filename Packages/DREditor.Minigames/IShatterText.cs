// Completely Reworked Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
// Original Author: LeoTheDev 
using UnityEngine;

namespace DREditor.Minigames
{
    /// <summary>
    ///
    /// </summary>
    public interface IShatterText
    {
        /// <summary>
        ///
        /// </summary>
        void ResetMesh();

        /// <summary>
        ///
        /// </summary>
        void SetMesh();

        /// <summary>
        ///
        /// </summary>
        /// <param name="hitPoint"></param>
        void Shatter(Vector3 hitPoint);

        /// <summary>
        ///
        /// </summary>
        /// <param name="shatterProcess"></param>
        void OverrideProcess(ScriptableObject shatterProcess);
    }
}