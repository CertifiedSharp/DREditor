//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.LineReading;
using DREditor.Services;
using System;
using System.Collections;

namespace DREditor.Choices
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DialogueComponent) + ", " + nameof(ILineEndProcess) + ", " + nameof(ILineWaitForClick) +
        "\n\n")]
    public class DialogueChoiceHandler : DialogueComponent, ILineEndProcess, ILineWaitForClick
    {
        public virtual bool SkipOnFastForward => false;


        /// <inheritdoc/>
        public virtual void LineEndProcess(Line currentLine = null)
        {
            if (ServiceLocator.HasService<IChoiceManager>())
                ServiceLocator.GetService<IChoiceManager>()?.ClearChoiceText();
        }


        /// <summary>
        ///
        /// </summary>
        /// <inheritdoc/>
        public virtual IEnumerator LineWaitForClick<T>
            (DialogueBase<T> currentDialogue, Line currentLine, Action exitParentCallback = null)
            where T : Line
        {
            if (currentDialogue is not Dialogue dialogue)
                yield break;

            bool OnLastLine = dialogue.Lines.IndexOf(currentLine) == dialogue.Lines.Count - 1;

            bool hasChoices = OnLastLine && dialogue.Choices.Count != 0;

            if (hasChoices)
            {
                bool exitEarly = false;

                UnityEngine.Debug.Log(ToString() + ": Dialogue: " + dialogue.name + " has choices.");

                yield return ServiceLocator
                    .GetService<IChoiceManager>()
                    .ExecuteChoiceHandling(dialogue, () => { exitParentCallback?.Invoke(); exitEarly = true; });

                UnityEngine.Debug.Log(ToString() + ": Exit Early is: " + exitEarly);

                if (exitEarly)
                    yield break;
            }
        }
    }
}