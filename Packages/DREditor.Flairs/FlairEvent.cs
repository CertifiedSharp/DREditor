//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Kill/Cure: Beautiful Vice
using DREditor.Dialogues.Events;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.Flairs
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class FlairEvent : IDialogueEvent, IFlairEvent
    {
        /// <summary>
        ///
        /// </summary>
        [PropertyButton(nameof(TriggerDialogueEvent))]
        [Tooltip("")]
        [SerializeField] protected Flair flair;

        /// <summary>
        ///
        /// </summary>
        [SerializeReference, SelectType]
        [Tooltip("")]
        [SerializeField] protected List<IFlairSetting> settings = new();


        /// <inheritdoc/>
        public virtual void TriggerDialogueEvent() => DialogueEventSystem.TriggerEvent("SpawnFlair", this);

        /// <inheritdoc/>
        public virtual IFlair GetFlair() => flair;

        /// <inheritdoc/>
        public virtual List<IFlairSetting> GetSettings() => settings;

        /// <inheritdoc/>
        public virtual void ApplySettings(GameObject spawnedFlair)
        {
            foreach (var setting in settings)
                setting?.ApplySetting(spawnedFlair);
        }
    }
}