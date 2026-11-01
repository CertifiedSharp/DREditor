//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Services;
using System.Collections;

namespace DREditor.Dialogues.Loading
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DialogueAssetLoaderBase) + ", " + nameof(DialogueLoader) + ", " + 
        nameof(IToTitleScreen) + ", " + nameof(IDialogueLoader) +
        "\n\n")]
    public class DialogueAssetLoader : DialogueAssetLoaderBase, IDialogueLoader
    {
        /// <inheritdoc/>
        public virtual IEnumerator LoadDialogueProcess(object dialogueData = null)
        {
            if (inTrialMode.Value)
                yield break;

            SetDialogueData(dialogueData);

            ServiceLocator.GetService<IDialogueManager>()?.StartDialogue(null, (Dialogue)DialogueLoadIdentifier.GetLoadedDialogue());

            yield break;
        }
    }
}