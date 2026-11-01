//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.Events;
using DREditor.Dialogues.Loading;

namespace DREditor.DialogueMechanics
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DialogueComponent) + ", " + nameof(ILoadToLineLastingEvent) +
        "\n\n")]
    public class LoadToLineLastingItemImage : DialogueComponent, ILoadToLineLastingEvent
    {
        /// <inheritdoc/>
        public virtual void OnLoadToLineLasting(Line currentLine)
            => DialogueEventUtility.ContainsDialogueEvent<ShowItem>(currentLine, ItemEventCheck);

        /// <summary>
        ///
        /// </summary>
        /// <param name="dialogueEvent"></param>
        protected virtual void ItemEventCheck(IDialogueEvent dialogueEvent)
        {
            ShowItem showItem = (ShowItem)dialogueEvent;

            if (showItem.Image == null)
                showItem.TriggerDialogueEvent();
        }
    }
}