//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using System.Collections.Generic;

namespace DREditor.Dialogues.Events
{
    /// <summary>
    /// Interface for processes that contain a list of <see cref="IDialogueEvent"/>.
    /// </summary>
    public interface IDialogueEventCollection
    {
        /// <summary>
        /// The Collection of <see cref="IDialogueEvent"/>s held by the implementer.
        /// </summary>
        List<IDialogueEvent> DialogueEvents { get; }
    }
}