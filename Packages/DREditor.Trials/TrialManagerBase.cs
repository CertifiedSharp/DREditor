//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

namespace DREditor.Trials
{
    /// <summary>
    ///
    /// </summary>
    public abstract class TrialManagerBase : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        protected abstract void LoadTrialAtDialogueIndex();

        /// <summary>
        ///
        /// </summary>
        /// <param name="startIndex"></param>
        public abstract void StartTrialAtIndex(int startIndex);

        /// <summary>
        ///
        /// </summary>
        public abstract void StartTrialAtChapter();
    }
}