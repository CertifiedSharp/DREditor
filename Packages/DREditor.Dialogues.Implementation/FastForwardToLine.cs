//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using DREditor.Services;
using System.Collections;
using UnityEngine;

namespace DREditor.Dialogues.Implementation
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(ServiceHolder<FastForwardToLine>) + " for type: " + nameof(FastForwardToLine) +
        "\n\n")]
    public class FastForwardToLine : ServiceHolder<FastForwardToLine>
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inDialogue;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inFastForwardMode;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inInstant;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Tooltip("")]
        [SerializeField] protected int toLine = 0;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Dialogue dialogue;


        /// <summary>
        ///
        /// </summary>
        protected IDialogueManager dialogueManager;


        protected virtual void OnEnable() => inDialogue.AddListener(OnDialogueStart);

        protected virtual void OnDisable() => inDialogue.RemoveListener(OnDialogueStart);

        protected virtual void Start() => dialogueManager = ServiceLocator.GetService<IDialogueManager>();

        /// <summary>
        ///
        /// </summary>
        /// <param name="inDialogue"></param>
        protected virtual void OnDialogueStart(bool inDialogue)
        {
            if (inDialogue && !inInstant.Value)
            {
                inFastForwardMode.SetValue(true);

                StartCoroutine(WaitTillLine());
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator WaitTillLine()
        {
            yield return new WaitUntil(() => !inDialogue.Value || (AtTargetDialogue() && AtTargetLine()));

            inFastForwardMode.SetValue(false);

            if (AtTargetDialogue() && AtTargetLine())
                enabled = false;

            yield break;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool AtTargetDialogue() => dialogueManager.GetReader().GetCurrentDialogue() == dialogue;

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool AtTargetLine() => dialogueManager.GetReader().GetCurrentLineNum() == toLine;
    }
}