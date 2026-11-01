//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

namespace DREditor.AddOns
{
    /// <summary>
    ///
    /// </summary>
	[ClassHeader("")]
    public class TimeScaleOnStart : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float time = 1;

        /// <summary>
        ///
        /// </summary>
        protected virtual void Start() => Time.timeScale = time;
    }
}