//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.Finishing;
using DREditor.EventObjects;
using DREditor.Fading;
using DREditor.Services;
using System.Collections;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DREditor.UI.SavingLoading
{
    /// <summary>
    ///
    /// </summary>
	[ClassHeader("")]
    public class ReturnToMenuOnPauseMenu : ReturnToMenuHandler
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inInstant = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inDialogue = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inTrialMode = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected float fadeOutDuration = 0.3f;


        /// <summary>
        ///
        /// </summary>
        protected IDialogueFinisher DialogueFinisher => ServiceLocator.GetService<IDialogueManager>().GetFinisher();


        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        public virtual void ReturnToMenu()
        {
            EventSystem.current.SetSelectedGameObject(null);

            ServiceLocator.GetService<IDialogueManager>().GetManagerComponent<DialogueDataClearer>().ClearSaveData();

            StartCoroutine(FadeToMenu());
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public virtual IEnumerator FadeToMenu()
        {
            yield return ServiceLocator.GetService<GlobalFade>().FadeToRoutine(fadeOutDuration);

            if (inDialogue.Value && !inTrialMode.Value)
                SubscribeToMenuOnDialogueFinish();
            else
                base.ReturnToMenu();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void SubscribeToMenuOnDialogueFinish()
        {
            if (inInstant.Value && DialogueFinisher is IInstantDialogueFinisherSubscriber subscriber)
                subscriber.SubscribeToOnFinishOfInstantDialogue(ToMenuOnDialogueFinish);
            else if (!inInstant.Value)
                DialogueFinisher.SubscribeToOnFinishOfDialogue(ToMenuOnDialogueFinish);
            else
                UnityEngine.Debug.LogWarning("ALERT: Game is in Instant state and active dialogue finisher is not an " +
                    nameof(IInstantDialogueFinisherSubscriber) + ". Player may not return to menu, contact Sweden for support.");

            CallToTitleScreen();

            Time.timeScale = 1;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ToMenuOnDialogueFinish()
        {
            DialogueFinisher.UnsubscribeToOnFinishOfDialogue(ToMenuOnDialogueFinish);

            if (DialogueFinisher is IInstantDialogueFinisherSubscriber subscriber)
                subscriber.UnsubscribeToOnFinishOfInstantDialogue(ToMenuOnDialogueFinish);

            LoadToScene();
        }
    }
}