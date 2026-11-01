//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues.Starting;
using DREditor.EventObjects;
using System.Collections;

namespace DREditor.Dialogues.Implementation
{
    using UnityEngine;

    /// <summary>
    ///
    /// </summary>
    public class DialogueAssetWaiter : DialogueWaiter
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]

        [Tooltip("")]
        [SerializeField] protected BoolWithEvent hasStartedDialogue = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inDialogue = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent isWaitingForDialogueToFinish = null;


        /// <summary>
        ///
        /// </summary>
        /// <param name="dialogue"></param>
        /// <returns></returns>
        public override IEnumerator CheckIfInDialogueToPlay(DialogueCore dialogue)
        {
            if (hasStartedDialogue.Value)
                yield return WaitToPlayDialogue(dialogue.name);
            else
                hasStartedDialogue.SetValue(true);

            yield break;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="name"></param>
        /// <returns></returns>
        protected virtual IEnumerator WaitToPlayDialogue(string name)
        {
            Debug.LogWarning($"{ToString()}: ALERT: Dialogue: " + name + " has attempted to start while still inside of dialogue. In " +
                "order to not flip off the InDialogue Value, the game will wait for the current dialogue to finish.");

            SetToWaiting();

            while (IsWaiting())
            {
                if (!inDialogue.Value)
                    break;

                yield return null;
            }

            SetToNoLongerWaiting();

            Debug.Log($"{ToString()}: REPORT: The dialogue: " + name + " has finished waiting and will now play.");

            yield break;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void SetToWaiting() => isWaitingForDialogueToFinish.SetValue(true);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool IsWaiting() => isWaitingForDialogueToFinish.Value;

        /// <summary>
        ///
        /// </summary>
        protected virtual void SetToNoLongerWaiting() => isWaitingForDialogueToFinish.SetValue(false);
    }
}