//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden

namespace DREditor.Dialogues.Finishing
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
    public interface IComplexFinishableDialogueComponent : IFinishableDialogueComponent, IFinishableDialogueComponentRoutine
    {
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        bool UseCoroutine();
    }
}