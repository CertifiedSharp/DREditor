//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.Events;
using DREditor.EventObjects;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace DREditor.DialogueEventHandling
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DialogueEventHandler) +
        "\n\n")]
    public class CharacterIntroAnim : DialogueEventHandler
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent isLoadingFile = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] Animator animator = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int animatorLayer = 0;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string endOfAnimationStateName = "Empty";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float startBufferTime = 0.2f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Canvas canvas = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected TMP_Text nameTMP = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected TMP_Text titleTMP = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected string showDialogueBoxEventString = "ShowBox";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string hideDialogueBoxEventString = "HideBox";


        [Header("")]
        [SerializeField] protected UnityEvent onIntroStarted;


        /// <summary>
        ///
        /// </summary>
        protected CharIntro introEvent = null;

        /// <summary>
        ///
        /// </summary>
        protected string stateName;


        /// <summary>
        ///
        /// </summary>
        protected virtual void Awake() => SetDefaultEventString("CharIntro");

        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        protected override void OnDialogueEventTriggered(object o = null)
        {
            introEvent = (CharIntro)o;

            UnityEngine.Debug.Log($"{ToString()}: Calling Intro Character Animation for: " + introEvent.charName);

            stateName = introEvent.charName;

            StartCoroutine(CharacterIntroRoutine());
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator CharacterIntroRoutine()
        {
            if (!isLoadingFile.Value)
            {
                PreIntro();

                if (startBufferTime > 0)
                    yield return new WaitForSeconds(startBufferTime);

                StartIntro();

                yield return animator.WaitForStateToComplete(animatorLayer);
            }

            ResolveIntro();

            PostResolveIntro();

            yield break;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void PreIntro()
        {
            if (hideDialogueBoxEventString != "")
                DialogueEventSystem.TriggerEvent(hideDialogueBoxEventString);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void StartIntro()
        {
            onIntroStarted?.Invoke();

            animator.Play(stateName);

            canvas.enabled = true;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ResolveIntro()
        {
            if (endOfAnimationStateName != "")
                animator.Play(endOfAnimationStateName);

            canvas.enabled = false;

            introEvent.SetFinished();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void PostResolveIntro()
        {
            if (showDialogueBoxEventString != "")
                DialogueEventSystem.TriggerEvent(showDialogueBoxEventString);
        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="name"></param>
        /// <remarks></remarks>
        public virtual void SetNameText(string name) => SetText(nameTMP, name);

        /// <summary>
        ///
        /// </summary>
        /// <param name="title"></param>
        /// <remarks></remarks>
        public virtual void SetTitleText(string title) => SetText(titleTMP, title);

        /// <summary>
        ///
        /// </summary>
        /// <param name="textGUI"></param>
        /// <param name="text"></param>
        protected virtual void SetText(TMP_Text textGUI, string text)
        {
            if (textGUI != null)
                textGUI.text = text;
        }
    }
}