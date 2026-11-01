//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.Loading;
using DREditor.Services;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DREditor.DialogueEventHandling
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DialogueEventHandler)+ "\n\n")]
    public class DialogueMidEntranceDebugger : DialogueEventHandler, IDialogueHolder
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected List<Dialogue> dialogues = new();


        /// <summary>
        ///
        /// </summary>
        protected virtual void Start()
        {
            if (FindObjectsByType<DialogueMidEntranceDebugger>().Count() > 1)
                Destroy(gameObject);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        protected override void OnDialogueEventTriggered(object o = null)
        {
            Dialogue[] dialogues = new Dialogue[1] { Evaluation() };

            ServiceLocator.GetService<IDialogueManager>().GetReader().MidDialogueEnterance(dialogues);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual Dialogue Evaluation()
        {
            int index = UnityEngine.Random.Range(0, dialogues.Count - 1);

            UnityEngine.Debug.Log(nameof(DialogueMidEntranceDebugger) + ": Evaluation resulted in: " + index);

            return dialogues[index];
        }


        /// <inheritdoc/>
        public virtual bool CheckIfSaveDialogue()
            => ServiceLocator.GetService<IDialogueManager>()
            .GetManagerComponent<DialogueLoadIdentifier>()
            .CheckDialogues<Line>(GetBaseList());

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual List<DialogueBase<Line>> GetBaseList()
        {
            List<DialogueBase<Line>> bases = new();

            foreach (var dialogue in dialogues)
                bases.Add(dialogue);

            return bases;
        }

        /// <summary>
        /// 
        /// </summary>
        [ContextMenu("Set Default Event")]
        public void SetDefaultEvent() => SetDefaultEventString("FTE_Test");

        protected virtual void Reset() => SetDefaultEventString("FTE_Test");
    }
}