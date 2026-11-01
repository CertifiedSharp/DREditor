//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.UIHandling;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace DREditor.Minigames
{
    /// <summary>
    ///
    /// </summary>
	[ClassHeader("")]
    public class QuestionToggle : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected InputActionReference toggleQuestion = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected UIVisibilityComponent fullUIVisibility = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected UIVisibilityComponent questionVisibility = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected ToggleTextVisibility controlsToggle = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected TextMeshProUGUI questionText = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float toggleWaitTime = 1;


        /// <summary>
        ///
        /// </summary>
        protected bool showing = false;


        /// <summary>
        ///
        /// </summary>
        /// <param name="questionText"></param>
        public virtual void Initialize(string questionText)
        {
            SetQuestion(questionText);

            Activate();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="text"></param>
        public virtual void SetQuestion(string text) => questionText.text = text;

        /// <summary>
        ///
        /// </summary>
        public virtual void Activate()
        {
            fullUIVisibility.Show();

            toggleQuestion.action.started += ToggleQuestion;
        }

        /// <summary>
        ///
        /// </summary>
        public virtual void Deactivate()
        {
            toggleQuestion.action.started -= ToggleQuestion;

            ResolveVisuals();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ResolveVisuals()
        {
            if (showing)
            {
                questionVisibility.Hide();

                controlsToggle.Toggle();

                showing = !showing;
            }

            fullUIVisibility.Hide();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="ctx"></param>
        protected virtual void ToggleQuestion(CallbackContext ctx)
        {
            toggleQuestion.action.started -= ToggleQuestion;

            controlsToggle.Toggle();

            if (showing)
                questionVisibility.Hide();
            else
                questionVisibility.Show();

            showing = !showing;

            StartCoroutine(Wait());
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator Wait()
        {
            yield return new WaitForSecondsRealtime(toggleWaitTime);

            yield return questionVisibility.WaitForChangeToComplete();

            toggleQuestion.action.started += ToggleQuestion;

            yield break;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDestroy()
            => toggleQuestion.action.started -= ToggleQuestion;
    }
}