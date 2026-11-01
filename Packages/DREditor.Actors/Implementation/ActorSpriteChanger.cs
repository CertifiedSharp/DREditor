//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.Events;
using DREditor.Dialogues.LineReading;
using DREditor.Dialogues.Loading;
using DREditor.EventObjects;
using DREditor.Talkables;
using System.Collections;
using UnityEngine;

namespace DREditor.Actors.Implementation
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    ///
    /// </remarks>
    [ClassHeader(nameof(DialogueComponent) + ", " + nameof(ILineProcessorRoutine) + ", " + nameof(ILoadToLineable) +
        "\n\n")]
    public class ActorSpriteChanger : DialogueComponent, ILineProcessorRoutine, ILoadToLineable
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected WorldSpriteChanger changer = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected SelectableObjectWithEvent currentActor = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inFastForwardMode = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent isProtagSpeaking = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent isLoadingFile = null;


        /// <inheritdoc/>
        public virtual bool ShouldProcess(Line currentLine)
        {
            bool lastActorPresent = currentLine.ContainsEventType<LastActorSprite>();

            bool expressionValid = currentLine.Expression != null && currentLine.Expression.Sprite != null;

            bool spriteShouldChange = expressionValid && !isProtagSpeaking.Value;

            return lastActorPresent || spriteShouldChange;
        }

        /// <inheritdoc/>
        public virtual IEnumerator LineProcessRoutine(Line currentLine) => ChangeLineSprites(currentLine);

        /// <summary>
        ///
        /// </summary>
        /// <param name="currentLine"></param>
        protected virtual IEnumerator ChangeLineSprites(Line currentLine)
        {
            Talkable currentTalkable = (Talkable)currentActor.Value;

            yield return WaitForSpriteChangeEventFinished<LastActorSprite>(currentLine, isProtagSpeaking.Value);

            yield return ChangeWorldSprite(currentLine, currentTalkable);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="currentLine"></param>
        /// <param name="additionalCondition"></param>
        protected virtual IEnumerator WaitForSpriteChangeEventFinished<T>(Line currentLine, bool additionalCondition = true)
        {
            if (currentLine.ContainsEventType<T>() && !isLoadingFile.Value && additionalCondition)
            {
                var changers = FindObjectsByType<WorldSpriteChanger>();

                foreach (var changer in changers)
                    if (changer.IsActivelyChanging())
                        yield return changer.WaitForSpriteToFinishChanging();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="currentLine"></param>
        /// <param name="currentActor"></param>
        /// <param name="waitToFinish"></param>
        protected virtual IEnumerator ChangeWorldSprite(Line currentLine, Talkable currentActor, bool waitToFinish = true)
        {
            bool expressionIsValid = currentLine.Expression != null && currentLine.Expression.Sprite != null;

            if (!isProtagSpeaking.Value && expressionIsValid)
            {
                Material expressionMaterial = currentLine.Expression.Sprite;

                Sprite expressionSprite = currentLine.Speaker.Sprites[currentLine.ExpressionNumber - 1].Sprite;

                changer.WorldSpriteChange((Actor)currentActor, expressionMaterial, expressionSprite);

                if (waitToFinish)
                    yield return changer.WaitForSpriteToFinishChanging();
            }
        }

        /// <inheritdoc/>
        public virtual void OnLoadToLine(Line currentLine) 
            => StartCoroutine(ChangeWorldSprite(currentLine, (Talkable)currentActor.Value, false));
    }
}