//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections;

namespace DREditor.Dialogues.Loading
{
    /// <summary>
    ///
    /// </summary>
    public class TrialDialogueLoader : DialogueAssetLoaderBase, IDialogueLoader
    {
        /// <inheritdoc/>
        public virtual IEnumerator LoadDialogueProcess(object dialogueData = null)
        {
            if (!inTrialMode.Value)
                yield break;

            SetDialogueData(dialogueData);

            yield break;
        }
    }
}