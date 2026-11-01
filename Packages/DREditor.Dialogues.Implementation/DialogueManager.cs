//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues.Ending;
using DREditor.Dialogues.Finishing;
using DREditor.Dialogues.Starting;
using DREditor.EventObjects;
using UnityEngine;

namespace DREditor.Dialogues.Implementation
{
    /// <summary>
    ///
    /// </summary>
    [DefaultExecutionOrder(-1)]
    public abstract class DialogueManager : MonoBehaviour, IDialogueManager
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected DialogueStarterDebugger DialogueStarterDebugger = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected DialogueStarter DialogueStarter = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected DialogueReader DialogueReader = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected DialogueEnder DialogueEnder = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected DialogueFinisher DialogueFinisher = null;


        /// <inheritdoc/>
        public virtual void DebugNoCharacterDialogueCall(SelectableObject actor, DialogueCore DebugNoDialogue = null) 
            => DialogueStarterDebugger.DebugNoCharacterDialogueCall(actor, DebugNoDialogue);

        /// <inheritdoc/>
        public virtual void StartDialogue<T>(SelectableObject actor, DialogueBase<T>[] dialogues, bool inleaving = false) where T : Line 
            => DialogueStarter.StartDialogue(actor, dialogues, inleaving);

        /// <inheritdoc/>
        public virtual void StartDialogue<T>(SelectableObject actor, DialogueBase<T> dialogue, bool inleaving = false) where T : Line 
            => DialogueStarter.StartDialogue(actor, dialogue, inleaving);

        /// <inheritdoc/>
        public virtual GameObject GetManagerObject() => gameObject;

        /// <inheritdoc/>
        public virtual DialogueReader GetReader() => DialogueReader;

        /// <inheritdoc/>
        public virtual IDialogueEnder GetEnder() => DialogueEnder;

        /// <inheritdoc/>
        public virtual IDialogueFinisher GetFinisher() => DialogueFinisher;

        /// <inheritdoc/>
        public virtual T GetManagerComponent<T>() where T : MonoBehaviour => GetComponentInChildren<T>();


        /// <inheritdoc/>
        public virtual void Invoke<T>(DialogueCore dialogue) where T : Line
        {
            if (dialogue is DialogueBase<T> dia)
                StartDialogue(null, dia);
        }
    }
}