//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

namespace DREditor.DialogueEventHandling
{
    /// <summary>
    ///
    /// </summary>
    public abstract class CustomDialogueEventHandler : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        protected virtual void OnEnable() => StartListeningToEvents();

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDisable() => StopListeningToEvents();


        /// <summary>
        ///
        /// </summary>
        protected abstract void StartListeningToEvents();

        /// <summary>
        ///
        /// </summary>
        protected abstract void StopListeningToEvents();
    }
}