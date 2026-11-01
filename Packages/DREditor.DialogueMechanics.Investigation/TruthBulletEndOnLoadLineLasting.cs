//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.Events;
using DREditor.Dialogues.Loading;
using DREditor.Investigations;
using UnityEngine;

namespace DREditor.DialogueMechanics.Investigation
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DialogueComponent) + ", " + nameof(ILoadToLineLastingEvent) +
        "\n\n")]
    public class TruthBulletEndOnLoadLineLasting : DialogueComponent, ILoadToLineLastingEvent
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string resolveEventName = "TruthBulletGetEnd";


        /// <inheritdoc/>
        public virtual void OnLoadToLineLasting(Line currentLine)
                => DialogueEventUtility.ContainsDialogueEvent<TruthBulletGet>(currentLine, BulletEndEventCheck);

        /// <summary>
        ///
        /// </summary>
        /// <param name="dialogueEvent"></param>
        protected virtual void BulletEndEventCheck(IDialogueEvent dialogueEvent) => DialogueEventSystem.TriggerEvent(resolveEventName);
    }
}