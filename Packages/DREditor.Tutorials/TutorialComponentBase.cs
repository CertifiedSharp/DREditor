//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

namespace DREditor.Tutorials
{
    /// <summary>
    ///
    /// </summary>
    public abstract class TutorialComponentBase : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="asset"></param>
        public abstract void Initialize(TutorialAsset asset);


        /// <summary>
        ///
        /// </summary>
        public abstract void Resolve();
    }
}