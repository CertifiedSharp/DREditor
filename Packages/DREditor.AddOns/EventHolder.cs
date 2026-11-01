//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

namespace DREditor.AddOns
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
	[ClassHeader("")]
    public class EventHolder : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] List<UnityEvent> events = new();


        /// <summary>
        ///
        /// </summary>
        /// <param name="index"></param>
        public virtual void InvokeEventAtIndex(int index) => events[index]?.Invoke();
    }
}