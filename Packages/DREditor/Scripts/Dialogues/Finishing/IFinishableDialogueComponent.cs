//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden

namespace DREditor.Dialogues.Finishing
{
    /// <summary>
    ///
    /// </summary>
    public interface IFinishableDialogueComponent : IDialogueComponent<IFinishableDialogueComponent>
    {
        /// <summary>
        ///
        /// </summary>
        void OnFinishDialogue();
    }
}