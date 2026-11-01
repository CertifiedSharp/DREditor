//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.LineReading;
using System;
using System.Collections;
using UnityEngine;

namespace DREditor.DialogueMechanics.AutomaticLine
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DialogueComponent) + ", " + nameof(ILineWaitForClick) +
        "\n\n")]
    public class AutoMechanicLineHandler : DialogueComponent, ILineWaitForClick
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected AutoMechanicBase AutoMechanic = null;


        public virtual bool SkipOnFastForward => true;

        /// <inheritdoc/>
        public virtual IEnumerator LineWaitForClick<T>(DialogueBase<T> currentDialogue, Line currentLine, 
            Action exitParentCallback = null) 
            where T : Line
        {
            if (AutoMechanic.InAutoMode())
            {
                yield return AutoMechanic.WaitForAutoMechanicToResolve();

                if (AutoMechanic.InAutoMode())
                    exitParentCallback?.Invoke();
            }
        }
    }
}