//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

namespace DREditor.AddOns
{
    /// <summary>
    ///
    /// </summary>
    public class EnableCanvasOnStart : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Canvas canvas = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool enableState = true;


        /// <summary>
        ///
        /// </summary>
        protected virtual void Start() => canvas.enabled = enableState;
    }
}