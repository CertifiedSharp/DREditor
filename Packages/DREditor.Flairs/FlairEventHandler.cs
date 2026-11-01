//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Kill/Cure: Beautiful Vice
using DREditor.DialogueEventHandling;
using UnityEngine;

namespace DREditor.Flairs
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DialogueEventHandler) +
        "\n\n")]
    public class FlairEventHandler : DialogueEventHandler
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected FlairSpawnerBase spawner;


        /// <inheritdoc/>
        protected override void OnDialogueEventTriggered(object o = null)
        {
            if (spawner != null && o is IFlairEvent eventInstance)
                spawner.Spawn(eventInstance);
            else
                UnityEngine.Debug.LogWarning($"ALERT: Either there was no spawner assigned or the event sent was not a {nameof(IFlairEvent)}!");
        }


        [ContextMenu("Set Default Event")]
        public void SetDefaultEvent() => SetDefaultEventString("SpawnFlair");
    }
}