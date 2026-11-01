//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

namespace DREditor.EventObjectServices
{
    /// <summary>
    ///
    /// </summary>
	[ClassHeader("")]
    public class IntWithEventIndexListener : IntWithEventListener
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int index = 0;

        /// <inheritdoc/>
        protected override void OnValueChanged(int updatedValue)
        {
            if (updatedValue == index)
                base.OnValueChanged(updatedValue);
        }
    }
}