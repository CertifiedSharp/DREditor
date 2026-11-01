//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Kill/Cure: Beautiful Vice
using DREditor.Dialogues;
using DREditor.EventObjects;
using DREditor.Services;
using System;
using UnityEngine;

namespace DREditor.Flairs
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class SpawnFlairOnHead : SpawnFlairToParent
    {
        [TextArea]
        [SerializeField] protected string description = "Spawns the flair parented to the currently speaking character's head transform.";


        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        protected override Transform GetFlairParent() 
        {
            var line = ServiceLocator.GetService<IDialogueManager>().GetReader().GetCurrentLine();

            var actor = ServiceLocator.GetService<IObjectPooler<SelectableObject>>().GetObjectByName(line.Speaker.FirstName);

            if (actor == null)
                UnityEngine.Debug.LogWarning("ALERT: The Flair failed to spawn because the head of the current speaker could not be found!");

            return actor != null ? actor.head : null;
        }
    }
}