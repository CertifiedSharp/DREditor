//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections;

namespace DREditor.Dialogues.Starting
{
    /// <summary>
    ///
    /// </summary>
    public interface IPreparableDialogueComponent : IDialogueComponent<IPreparableDialogueComponent>
    {
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        bool UseRoutineToPrepare();

        /// <summary>
        ///
        /// </summary>
        void Prepare();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        IEnumerator PrepareRoutine();
    }
}