//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.DialogueEventHandling;
using DREditor.EventObjects;
using System.Collections;
using UnityEngine;

namespace DREditor.Actors.Events
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DialogueEventHandler) +
        "\n\n")]
    public class ActorFaintEventRenderQ : ActorFaintEvent
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected StringWithEvent renderQCamCode = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string faintingRenderQueueTag = "Fainting";


        /// <inheritdoc/>
        protected override void OnDialogueEventTriggered(object o)
        {
            renderQCamCode.SetValue(faintingRenderQueueTag);

            base.OnDialogueEventTriggered(o);
        }

        /// <inheritdoc/>
        protected override IEnumerator FaintAnim()
        {
            yield return base.FaintAnim();

            RemoveFromRenderQ();
        }

        /// <inheritdoc/>
        protected override IEnumerator UnFaintAnimation(float toYPosition)
        {
            yield return base.UnFaintAnimation(toYPosition);

            RemoveFromRenderQ();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void RemoveFromRenderQ()
        {
            if (renderQCamCode.Value == faintingRenderQueueTag)
                renderQCamCode.Invoke();
            else
                renderQCamCode.SetValue(faintingRenderQueueTag);
        }
    }
}