//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Input;
using DREditor.Services;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DREditor.UI.Buttons
{
    /// <summary>
    ///
    /// </summary>
    public class DREButton : Button
    {
        /// <summary>
        ///
        /// </summary>
        protected bool inputRemoved = false;

        /// <summary>
        ///
        /// </summary>
        private int lastClickFrame = -1;


        /// <inheritdoc/>
        public override void OnSubmit(BaseEventData eventData)
        {
            if (PointerClickFails(eventData))
                return;

            lastClickFrame = Time.frameCount;

            StartCoroutine(BufferDisableInput());

            base.OnSubmit(eventData);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="eventData"></param>
        /// <returns></returns>
        protected virtual bool PointerClickFails(BaseEventData eventData)
            => ServiceLocator.GetService<MenuState>().Changing || Time.frameCount <= lastClickFrame;

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator BufferDisableInput()
        {
            ServiceLocator.GetService<IInputManager>().Disable();

            inputRemoved = true;

            yield return null;

            yield return null;

            ServiceLocator.GetService<IInputManager>().Enable();

            inputRemoved = false;
        }

        /// <remarks></remarks>
        protected override void OnDestroy()
        {
            base.OnDestroy();

            if (inputRemoved)
                ServiceLocator.GetService<IInputManager>().Enable();
        }
    }
}