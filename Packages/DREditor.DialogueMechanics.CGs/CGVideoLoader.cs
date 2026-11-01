//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues.Events;

namespace DREditor.DialogueMechanics.CGs
{
    /// <summary>
    ///
    /// </summary>
	[ClassHeader("")]
    public class CGVideoLoader : CGStateLoader<VideoDisplay>
    {
        // Removed 4/19/2026 4:40pm CST:
        // When this code was active, saving and loading during a cg, 
        // the cg would clear the tracking of the Video loader, and since ClearAllOtherCGLoaders();
        // was called, it'd clear the cg loader, messing up the tracking of the active CG.
        // 
        // So if this code is truely needed, then the above scenario should also be tested to ensure a proper fix.
        // If post DREditor 2025-2026 rework the absence of this code hasn't created any issues, then this code 
        // can be deleted.
        //
        //protected override void ClearTracking()
        //{
        //    base.ClearTracking();

        //    ClearAllOtherCGLoaders();
        //}

        /// <inheritdoc/>
        protected override void OnPrepareLoadedLineComplete(VideoDisplay display)
        {
            SetLoadingCGState(ShouldBeTracked(display));

            SetLastEventNull();
        }

        /// <inheritdoc/>
        protected override bool ShouldBeTracked(VideoDisplay display) 
            => display.mainClip != null && !display.playOnly;
    }
}