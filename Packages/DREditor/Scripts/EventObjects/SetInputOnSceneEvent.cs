//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.EventObjects
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(SceneEventListener) +
        "\n\n")]
    public class SetInputOnSceneEvent : SceneEventListener
    {
        [Tooltip("")]
        [SerializeField] protected bool whenInvokedSetInput = true;

        [Tooltip("")]
        [SerializeField] protected List<BoolWithEvent> allowedInputs = new();


        protected override void OnResponseInvoked()
        {
            base.OnResponseInvoked();

            SetInputStates(whenInvokedSetInput);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="enabled"></param>
        protected virtual void SetInputStates(bool enabled)
        {
            foreach (var input in allowedInputs)
                input.SetValue(enabled);
        }
    }
}