//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.Events;
using DREditor.Dialogues.LineReading;
using DREditor.Dialogues.Loading;
using DREditor.EventObjects;
using UnityEngine;

namespace DREditor.UISpriteHandling
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DialogueComponent) + ", " + nameof(ILineProcessor) + ", " + nameof(ILoadToLineEnd) +
        "\n\n")]
    public class ProtagUISpriteHandler : DialogueComponent, ILineProcessor, ILoadToLineEnd
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent isProtagSpeaking = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string spriteLineEvent = "ProtagSpriteLineEvent";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string loadToLineEndEvent = "ProtagLoadToLineEndSpriteEvent";


        /// <inheritdoc/>
        public virtual void LineProcess(Line currentLine)
        {
            bool expressionIsValid = currentLine.Expression != null && currentLine.Expression.Sprite != null;

            if (isProtagSpeaking.Value && expressionIsValid)
                DialogueEventSystem.TriggerEvent(spriteLineEvent, currentLine);
        }

        /// <inheritdoc/>
        public virtual void OnLoadToLineEnd(Line currentLine)
        {
            if (isProtagSpeaking.Value)
                DialogueEventSystem.TriggerEvent(loadToLineEndEvent, currentLine);
        }
    }
}