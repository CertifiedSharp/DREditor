//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.Events;
using DREditor.Dialogues.LineReading;

namespace DREditor.DialogueEventManaging
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <remarks></remarks>
    public class LineProcessDialogueEvent<T> : DialogueEventManager, ILineProcessor where T : IDialogueEvent
    {
        public virtual void LineProcess(Line currentLine) => TriggerAllDialogueEventsOfType<T>(currentLine);
    }
}