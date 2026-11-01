//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections;

namespace DREditor.Dialogues.Ending
{
    /// <summary>
    ///
    /// </summary>
    public interface IResolvableDialogueComponent : IDialogueComponent<IResolvableDialogueComponent>
    {
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        bool UseRoutineToResolve();

        /// <summary>
        ///
        /// </summary>
        void Resolve();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        IEnumerator ResolveRoutine();
    }
}