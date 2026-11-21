//Author: SeleniumSoul
//Updated by Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using UnityEngine;

namespace DREditor.Cameras
{
    /// <summary>
    /// Script to represent whether a trial cameras radius should be locked.
    /// </summary>
    /// <remarks>
    /// The reason why this script hasn't been moved to be apart of another scripts data is because almost all Trial Camera Animations 
    /// already have a reference to this script, if it were to be deleted, many DREditor Trial Camera Animations would have to be redone.
    /// </remarks>
    public class RadiusLock : MonoBehaviour
    {
        /// <summary>
        /// Whether a Trial Camera's radius should be locked.
        /// </summary>
        [Header("Script to represent whether a trial cameras radius should be locked.")]
        [Tooltip("Whether a Trial Camera's radius should be locked.")]
        public bool Locked;
    }
}
