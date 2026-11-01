//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues.Events;
using UnityEngine;

namespace DREditor.TrialDialogueHandling
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    ///
    /// </remarks>
	[ClassHeader("")]
    public class TrialUIOnTutorialHandler : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected string dialogueEventString = "TrialTutorial";


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected Animator uiAnimator = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string textBoxOnlyParameter = "MoveTDUI";


        /// <summary>
        ///
        /// </summary>
        protected virtual void OnEnable() => DialogueEventSystem.StartListening(dialogueEventString, TutorialEvaluation);

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDisable() => DialogueEventSystem.StopListening(dialogueEventString, TutorialEvaluation);

        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        protected virtual void TutorialEvaluation(object o = null)
        {
            if (HasParameter(textBoxOnlyParameter, uiAnimator))
                uiAnimator.SetBool(textBoxOnlyParameter, o != null);
            else
                UnityEngine.Debug.LogWarning("COULDN'T FIND PARAMETER FOR TUT EVAL");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="paramName"></param>
        /// <param name="animator"></param>
        /// <returns></returns>
        static bool HasParameter(string paramName, Animator animator)
        {
            foreach (AnimatorControllerParameter param in animator.parameters)
                if (param.name == paramName)
                    return true;

            return false;
        }
    }
}