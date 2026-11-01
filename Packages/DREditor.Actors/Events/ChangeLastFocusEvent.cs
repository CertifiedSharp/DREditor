//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Characters;
using DREditor.DialogueEventHandling;
using DREditor.Dialogues.Events;
using DREditor.EventObjects;
using DREditor.Services;
using DREditor.Talkables;
using UnityEngine;

namespace DREditor.Actors.Events
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DialogueEventHandler) + 
        "\n\n")]
    public class ChangeLastFocusEvent : DialogueEventHandler
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected CharacterDatabase characterDatabase = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent isLoadingFile = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected FocusHandler focusHandler = null;


        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        protected override void OnDialogueEventTriggered(object o = null)
        {
            LastActorSprite laEvent = (LastActorSprite)o;

            Character character = characterDatabase.Characters[laEvent.charNum];

            Actor actor = null;

            if (character != null)
                actor = ServiceLocator.GetService<IObjectPooler<Talkable>>().GetObjectByName(character.FirstName) as Actor;

            if (isLoadingFile.Value && actor != null)
                focusHandler.ChangeFocus(actor, null);
        }

        /// <summary>
        /// 
        /// </summary>
        [ContextMenu("Set Default Event")]
        public void SetDefaultEvent() => SetDefaultEventString("ChangeLast");
    }
}