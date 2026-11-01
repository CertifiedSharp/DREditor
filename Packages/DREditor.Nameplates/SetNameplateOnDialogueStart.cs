//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.Starting;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DREditor.Nameplates
{
    /// <summary>
    ///
    /// </summary>
	[ClassHeader("")]
    public class SetNameplateOnDialogueStart : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected NameplateHandler nameplateHandler = null;


        /// <summary>
        ///
        /// </summary>
        protected List<IDialogueStartEvent> DialogueStartEvents = null;


        /// <summary>
        ///
        /// </summary>
        protected virtual void Start()
        {
            InitializeDialogueStartEvent();

            SubscribeToStartEvent();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeDialogueStartEvent() 
            => DialogueStartEvents = FindObjectsByType<MonoBehaviour>().OfType<IDialogueStartEvent>().ToList();

        /// <summary>
        ///
        /// </summary>
        protected virtual void SubscribeToStartEvent() 
            => DialogueStartEvents.ForEach(startEvent => startEvent.OnDialogueStarted += SetFirstSpeakersNameplate);


        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDisable() => UnsubscribeToStartEvent();

        /// <summary>
        ///
        /// </summary>
        protected virtual void UnsubscribeToStartEvent() 
            => DialogueStartEvents.ForEach(startEvent => startEvent.OnDialogueStarted -= SetFirstSpeakersNameplate);


        /// <summary>
        ///
        /// </summary>
        /// <param name="core"></param>
        protected virtual void SetFirstSpeakersNameplate(DialogueCore core)
        {
            if (core is Dialogue dialogue && dialogue.Lines.Count > 0 && dialogue.Lines[0].Speaker != null)
                nameplateHandler.SetNameplate(dialogue.Lines[0]);
        }
    }
}