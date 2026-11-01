//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.DialogueEventHandling;
using DREditor.Dialogues.Events;
using DREditor.EventObjects;
using UnityEngine;

namespace DREditor.CustomEvents
{
    /// <summary>
    ///
    /// </summary>
    [CustomDialogueEvent("")]
    [ClassHeader(nameof(CustomDialogueEventHandler) +
        "\n\n")]
    public class ForceProtagViewCustomEvent : CustomDialogueEventHandler
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent protagUIVisible = null;


        /// <inheritdoc/>
        protected override void StartListeningToEvents() => DialogueEventSystem.StartListening("ForceProtagView", ForceProtagView);

        /// <inheritdoc/>
        protected override void StopListeningToEvents() => DialogueEventSystem.StopListening("ForceProtagView", ForceProtagView);

        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        protected virtual void ForceProtagView(object o = null)
        {
            if (o is BoolCapsulate boolData && boolData != null && boolData.value != protagUIVisible.Value)
                protagUIVisible.SetValue(!protagUIVisible.Value);
        }
    }
}