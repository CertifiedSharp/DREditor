//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Kill/Cure

namespace DREditor.DialogueEventHandling.Flashing
{
    /// <summary>
    ///
    /// </summary>
    public abstract class FlashEventSimpleHandlerBase : FlashEventHandlerBase
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        protected override void OnDialogueEventTriggered(object o = null)
        {
            KillActiveProcess(ResetFlash);

            if (FailFlash())
                return;

            DisplayFlashEvent();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected abstract bool FailFlash();

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayFlashEvent() => FlashAnimation(FlashUpdate, ResetFlash);
    }
}