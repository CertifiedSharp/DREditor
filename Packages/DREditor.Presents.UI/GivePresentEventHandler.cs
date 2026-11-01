//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.DialogueEventHandling;
using DREditor.EventObjects;
using DREditor.UI;
using System.Collections;
using UnityEngine;

namespace DREditor.Presents.UI
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DialogueEventHandler) +
        "\n\n")]
    public class GivePresentEventHandler : DialogueEventHandler
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEvent showFakePause = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float pauseMenuStartUpTime = 1.2f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected MenuGroupBase presentUI = null;


        /// <inheritdoc/>
        protected override void OnDialogueEventTriggered(object o = null) => StartCoroutine(DelayDisplayPresentUI());

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator DelayDisplayPresentUI()
        {
            if (showFakePause != null)
                showFakePause.Raise();

            yield return new WaitForSecondsRealtime(pauseMenuStartUpTime);

            presentUI.Show();

            yield break;
        }


        /// <summary>
        ///
        /// </summary>
        protected virtual void Reset() => SetDefaultEventString("GivePresent");


        /// <summary>
        /// 
        /// </summary>
        [ContextMenu("Test Event")]
        public void TestEvent() => OnDialogueEventTriggered();
    }
}