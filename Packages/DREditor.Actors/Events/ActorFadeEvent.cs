//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.DialogueEventHandling;
using UnityEngine;

namespace DREditor.Actors.Events
{
    /// <summary>
    ///
    /// </summary>
    public abstract class ActorFadeEvent : DialogueEventHandler
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected ActorSpriteFader spriteFader = null;
    }
}