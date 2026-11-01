//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

namespace DREditor.EventObjects
{
    /// <summary>
    ///
    /// </summary>
    public class ToggleBoolWithEvent : MonoBehaviour
    {
        [Header("")]

        [Tooltip("")]
        [SerializeField] protected BoolWithEvent boolWithEvent = null;


        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        public virtual void ToggleEvent() => boolWithEvent.SetValue(!boolWithEvent.Value);
    }
}