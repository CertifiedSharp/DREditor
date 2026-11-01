//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.Loading;
using DREditor.EventObjects;
using UnityEngine;

namespace DREditor.DialogueMechanics
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DialogueComponent) + ", " + nameof(ILoadToLineEnd) + ", " + nameof(ILoadToLineResolver) +
        "\n\n")]
    public class ProtagUILoadEndResolver : DialogueComponent, ILoadToLineEnd, ILoadToLineResolver
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string toggleProtagUIDialogueCode = "^";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent protagUIVisible = null;


        /// <summary>
        ///
        /// </summary>
        /// <param name="currentLine"></param>
        /// <remarks></remarks>
        public virtual void OnLoadToLineEnd(Line currentLine)
        {
            if (currentLine.Text.Contains(toggleProtagUIDialogueCode))
                protagUIVisible.Value = !protagUIVisible.Value;
        }

        /// <inheritdoc/>
        public virtual void OnLoadedLineResolve() => protagUIVisible.Invoke();
    }
}