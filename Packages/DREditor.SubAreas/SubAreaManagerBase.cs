//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.Finishing;
using DREditor.EventObjects;
using DREditor.LocalDialogues;
using DREditor.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DREditor.SubAreas
{
    /// <summary>
    ///
    /// </summary>
    public abstract class SubAreaManagerBase : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected StringWithEvent currentDisplayName = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inDialogue = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inTPFD = null;


        /// <summary>
        ///
        /// </summary>
        protected IDialogueManager dialogueManager;


        /// <summary>
        ///
        /// </summary>
        protected virtual void Awake()
        {
            if (ServiceLocator.HasService<IDialogueManager>())
                dialogueManager = ServiceLocator.GetService<IDialogueManager>();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="selectableObject"></param>
        /// <param name="dialogues"></param>
        /// <returns></returns>
        protected virtual bool TryPlayingDialogue(SelectableObject selectableObject, List<LocalDialogue> dialogues)
        {
            Dialogue dialogue = ServiceLocator.GetService<LocalDialogueHandlerBase>().EvaluateDialogue(dialogues);

            bool result = dialogue != null;

            if (result)
                dialogueManager.StartDialogue(selectableObject, dialogue);
            else
                UnityEngine.Debug.LogWarning($"ALERT: Sub Area {gameObject.name} failed to play valid dialogue!");

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="action"></param>
        protected virtual void SubscribeEndDialogueEvent(Action action)
        {
            var endEvents = FindObjectsByType<MonoBehaviour>().OfType<IEndDialogueEvent>();

            foreach(var endEvent in endEvents)
                endEvent.OnDialogueEnd += action;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="action"></param>
        protected virtual void UnsubscribeEndDialogueEvent(Action action)
        {
            var endEvents = FindObjectsByType<MonoBehaviour>().OfType<IEndDialogueEvent>();

            foreach (var endEvent in endEvents)
                endEvent.OnDialogueEnd -= action;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void TurnOffDisplayedObjectName()
        {
            if (currentDisplayName.Value == "")
                currentDisplayName.Invoke();
            else
                currentDisplayName.SetValue("");
        }
    }
}