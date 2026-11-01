//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.Ending;
using DREditor.EventObjects;
using DREditor.Progressing.Handling;
using DREditor.Saving;
using DREditor.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DREditor.EndEvents
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DialogueComponent) + ", " + nameof(IDialogueEndEvent) + ", " + 
        nameof(IClearDialogueSaveData) + ", " + nameof(IDRESaveable) + ", " + nameof(IDRELoadable) +
        "\n\n")]
    public class VariableEndEventHandler : DialogueComponent, IDialogueEndEvent, IClearDialogueSaveData, IDRESaveable, IDRELoadable
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent fastForwardModeWasEnabled = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string conditionalNumSaveKey = "Conditional";


        /// <summary>
        ///
        /// </summary>
        protected List<bool> saveConditions = new();


        /// <inheritdoc/>
        public virtual bool EvaluateEndEvent<T>(DialogueBase<T> currentDialogue, Action<DialogueCore[]> interruptDialogue)
            where T : Line
        {
            if (currentDialogue.Variable.Enabled)
                return EvaluateVarable(currentDialogue, interruptDialogue);
            else
                return false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="currentDialogue"></param>
        /// <param name="interruptDialogue"></param>
        /// <returns></returns>
        protected virtual bool EvaluateVarable<T>(DialogueBase<T> currentDialogue, Action<DialogueCore[]> interruptDialogue) 
            where T : Line 
        {
            DialogueCore[] dialogues = new DialogueCore[1];

            var variable = currentDialogue.Variable;

            bool boolVariableEvaluation = variable.BoolVariable != null && variable.BoolVariable.Resolve();

            bool flagShouldBeTriggered = ServiceLocator.GetService<FlagTriggerHandlerBase>().
                EvaluateVariable(variable.Chapter, variable.Objective, variable.Flags, variable.FalseFlags);

            if (boolVariableEvaluation || flagShouldBeTriggered)
            {
                dialogues[0] = currentDialogue.Variable.NextDialogueTrue;

                saveConditions.Add(true);
            }
            else if (variable.NextDialogueFalse != null)
            {
                dialogues[0] = currentDialogue.Variable.NextDialogueFalse;

                saveConditions.Add(false);
            }

            if (dialogues[0] != null)
            {
                fastForwardModeWasEnabled.SetValue(true);

                interruptDialogue(dialogues);

                return true;
            }

            return false;
        }

        
        /// <inheritdoc/>
        public virtual void Save(Dictionary<string, object> saveData)
        {
            if (!ServiceLocator.GetService<IDialogueManager>().GetComponentsInChildren<DialogueComponent>().Contains(this))
                return;

            saveData.Set(conditionalNumSaveKey, new List<bool>(saveConditions));
        }

        /// <inheritdoc/>
        public virtual void ClearSaveData() => saveConditions.Clear();

        /// <inheritdoc/>
        public virtual void Load(Dictionary<string, object> saveDatas) => saveConditions = new(saveDatas.Get<List<bool>>(conditionalNumSaveKey));
    }
}