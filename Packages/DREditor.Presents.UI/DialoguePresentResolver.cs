//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.Events;
using DREditor.EventObjects;
using DREditor.Services;
using System.Collections;
using UnityEngine;

namespace DREditor.Presents.UI
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(PresentResolver) + 
        "\n\n")]
    public class DialoguePresentResolver : PresentResolver
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected PresentDatabase presentDatabase = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEvent OnPlayerDidntGivePresentChoice = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected string callEvaluateReaction = "EvaluateReaction";


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected SceneEvent hideFakePause = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float pauseMenuStartUpTime = 1.2f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int noSelectionIndex = 1;

        
        /// <inheritdoc/>
        public override IEnumerator ResolvePresentSelection(bool gavePresent, int selectedPresentIndex)
        {
            if (hideFakePause != null)
                hideFakePause.Raise();

            yield return new WaitForSecondsRealtime(pauseMenuStartUpTime);

            if (gavePresent)
                ResolveGivingPresent(selectedPresentIndex);
            else
                ResolveNotGivingPresent();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="selectedPresentIndex"></param>
        protected virtual void ResolveGivingPresent(int selectedPresentIndex)
        {
            Present present = presentDatabase.presents[selectedPresentIndex];

            UnityEngine.Debug.Log("REPORT: Removing present to give: " + present.DisplayName);

            ServiceLocator.GetService<IPresentManager>().Get<IAmountablePresent>(present.ID)?.Remove();

            DialogueEventSystem.TriggerEvent(callEvaluateReaction, presentDatabase.presents[selectedPresentIndex]);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ResolveNotGivingPresent()
        {
            if (ServiceLocator.HasService<IDialogueManager>())
                if (((Dialogue)ServiceLocator.GetService<IDialogueManager>().GetReader().GetCurrentDialogue())
                    .Choices[noSelectionIndex].NextDialogue != null)
                    ForceDidntGivePresentChoice();
                else
                    ServiceLocator.GetService<IDialogueManager>().GetEnder().StopDialogue();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ForceDidntGivePresentChoice()
        {
            DialogueReader dialogueReader = ServiceLocator.GetService<IDialogueManager>().GetReader();

            Dialogue[] originalDialogue = new Dialogue[1];

            originalDialogue[0] = ((Dialogue)dialogueReader.GetCurrentDialogue()).Choices[noSelectionIndex].NextDialogue;

            dialogueReader.PlayDialogues(originalDialogue);

            OnPlayerDidntGivePresentChoice.Raise();
        }
    }
}