//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues.Events;

namespace DREditor.DialogueMechanics.CGs
{
    /// <summary>
    ///
    /// </summary>
	[ClassHeader("")]
    public class CGLoader : CGStateLoader<CGDisplay>
    {
        /// <inheritdoc/>
        protected override void OnPrepareLoadedLineComplete(CGDisplay display)
        {
            if (ShouldBeTracked(display))
                ClearAllOtherCGLoaders();
        }

        /// <inheritdoc/>
        protected override bool ShouldBeTracked(CGDisplay display) 
            => display.CG != null || display.prefab != null;
    }
}