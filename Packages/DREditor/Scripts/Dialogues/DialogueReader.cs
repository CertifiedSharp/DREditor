//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden

namespace DREditor.Dialogues
{
    /// <summary>
    ///
    /// </summary>
    public abstract class DialogueReader : DialogueComponent
    {
        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dialogues"></param>
        /// <param name="inLeave"></param>
        public abstract void PlayDialogues<T>(DialogueBase<T>[] dialogues, bool inLeave = false)
            where T : Line;

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dialogues"></param>
        public abstract void MidDialogueEnterance<T>(DialogueBase<T>[] dialogues)
            where T : Line;

        /// <summary>
        ///
        /// </summary>
        public abstract void CleanUp();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public abstract DialogueCore GetCurrentDialogue();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public abstract int GetCurrentLineNum();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public abstract Line GetCurrentLine();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public abstract bool IsOnLastLine();
    }
}