//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.Loading;
using DREditor.EventObjects;
using System.Collections;
using UnityEngine;

namespace DREditor.DialogueMechanics
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DialogueComponent) + ", " + nameof(IOnPrepareLoadedLine) +
        "\n\n")]
    public class PrepareFastForwardLoadedLine : DialogueComponent, IOnPrepareLoadedLine
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inFastForwardMode = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent dialogueLoaded = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent isLoadingFile = null;


        /// <inheritdoc/>
        public virtual void OnPrepareLoadedLine(Line currentLine) => inFastForwardMode.SetValue(true);

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnEnable() => dialogueLoaded.AddListener(OnDialogueLoadedIs);

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDisable() => dialogueLoaded.RemoveListener(OnDialogueLoadedIs);

        /// <summary>
        ///
        /// </summary>
        /// <param name="dialogueIsLoaded"></param>
        protected virtual void OnDialogueLoadedIs(bool dialogueIsLoaded)
        {
            if (dialogueIsLoaded)
                StartCoroutine(StopFFOnLoadingFileComplete());
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator StopFFOnLoadingFileComplete()
        {
            yield return new WaitUntil(() => !isLoadingFile.Value);

            inFastForwardMode.SetValue(false);

            yield break;
        }
    }
}