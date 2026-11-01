//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections.Generic;
using TMPro;
using UnityEngine;

namespace DREditor.Minigames.Debates.PhraseHandling
{
    /// <summary>
    ///
    /// </summary>
    [RequireComponent(typeof(TMP_Text))]
    [ClassHeader(nameof(IHitBoxOverride) +
        "\n\n")]
    public class PartlyHitHandler : MonoBehaviour, IHitBoxOverride
    {
        /// <summary>
        ///
        /// </summary>
        protected TMP_Text tmpText;

        /// <summary>
        ///
        /// </summary>
        protected TMP_TextInfo textInfo;

        /// <summary>
        ///
        /// </summary>
        protected TMP_LineInfo[] lineInfo;

        /// <summary>
        ///
        /// </summary>
        protected TMP_WordInfo[] wordInfos;

        /// <summary>
        ///
        /// </summary>
        protected int wordInPhrase;

        /// <summary>
        ///
        /// </summary>
        protected bool foundHitStart;

        /// <summary>
        ///
        /// </summary>
        protected bool foundHitEnd;

        /// <summary>
        ///
        /// </summary>
        protected int lineCount;

        /// <summary>
        ///
        /// </summary>
        protected bool hasOddLineBreaks;

        /// <summary>
        ///
        /// </summary>
        protected bool isMultiLine;

        /// <summary>
        ///
        /// </summary>
        protected bool foundCalled;

        /// <summary>
        ///
        /// </summary>
        protected bool carryOver;

        /// <summary>
        ///
        /// </summary>
        protected TMP_LineInfo line;

        /// <summary>
        ///
        /// </summary>
        protected BoxCollider boxCollider;

        /// <summary>
        ///
        /// </summary>
        protected float bottomLeft;

        /// <summary>
        ///
        /// </summary>
        protected float bottomRight;


        protected virtual void Awake() => tmpText = GetComponent<TMP_Text>();


        /// <inheritdoc/>
        public virtual Vector3 SetHitBox(PhraseBase phrase, List<BoxCollider> addedBoxes)
        {
            if (!phrase.Has<PartlyHitModifier>())
                return Vector3.zero;

            PartlyHitModifier hitModifier = phrase.Get<PartlyHitModifier>();

            ResetVariables();

            for (int i = 0; i < lineInfo.Length; i++) // For each line
            {
                if (!textInfo.GetValidLineIndexes().Contains(i))
                {
                    wordInPhrase += lineInfo[i].wordCount;

                    continue;
                }

                ResetLineVariables(i);

                UpdateAddedBoxes(addedBoxes);

                EvaluateTextLine(hitModifier);

                UpdateColliderEnd(i);
            }

            return tmpText.bounds.size;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ResetVariables()
        {
            textInfo = tmpText.textInfo;

            lineInfo = textInfo.lineInfo;

            wordInfos = textInfo.wordInfo;

            wordInPhrase = 0;

            foundHitStart = false;

            foundHitEnd = false;

            lineCount = textInfo.GetRealLineCount();

            hasOddLineBreaks = lineCount % 2 != 0;

            isMultiLine = lineCount > 1;

            foundCalled = false;

            carryOver = false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="i"></param>
        protected virtual void ResetLineVariables(int i)
        {
            line = lineInfo[i];

            bottomLeft = 0;

            bottomRight = 0;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="addedBoxes"></param>
        protected virtual void UpdateAddedBoxes(List<BoxCollider> addedBoxes)
        {
            boxCollider = GetUpdatedCollider();

            if (foundHitStart && !foundHitEnd) // if more than one hitable word past line break
            {
                carryOver = true;

                addedBoxes.Add(boxCollider);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual BoxCollider GetUpdatedCollider()
        {
            if (foundHitStart && !foundHitEnd)
                return gameObject.AddComponent<BoxCollider>();

            var colliders = GetComponentsInChildren<BoxCollider>();

            foreach (var collider in colliders)
                if (!collider.isTrigger)
                    return collider;

            UnityEngine.Debug.LogError("ALERT: The game failed to obtain the next collider! Contact the developer for support!");

            return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="hitModifier"></param>
        /// <remarks>
        ///
        /// </remarks>
        protected virtual void EvaluateTextLine(PartlyHitModifier hitModifier)
        {
            for (int j = 0; j < line.wordCount; j++) // For each word in the line
            {
                TMP_WordInfo word = wordInfos[wordInPhrase];

                // Indicates we are on a hitable word
                if (wordInPhrase >= hitModifier.WordStart &&
                    wordInPhrase < (hitModifier.WordStart + hitModifier.WordCount))
                {
                    if (j == 0 &&
                        foundHitStart &&
                        !foundHitEnd || wordInPhrase == hitModifier.WordStart)
                    {
                        bottomLeft = textInfo.characterInfo[word.firstCharacterIndex].bottomLeft.x;
                    }

                    // Recognizes this word as the first word in the partly hitable
                    if (wordInPhrase == hitModifier.WordStart)
                        foundHitStart = true;

                    if (j == line.wordCount - 1 && foundHitStart && !foundHitEnd ||
                        wordInPhrase + 1 == (hitModifier.WordStart + hitModifier.WordCount))
                    {
                        bottomRight = textInfo.characterInfo[word.lastCharacterIndex].bottomRight.x;
                    }
                }

                wordInPhrase++;

                // Recognizes this word as the last word in the partly hitable
                if (wordInPhrase == (hitModifier.WordStart + hitModifier.WordCount))
                    foundHitEnd = true;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="i"></param>
        /// <remarks>
        ///
        /// </remarks>
        protected virtual void UpdateColliderEnd(int i)
        {
            if ((!foundCalled || carryOver) && foundHitStart)
            {
                int yCenterRatio = 0;

                if (isMultiLine)
                {
                    if (hasOddLineBreaks)
                        yCenterRatio = (lineCount / 2) - i;
                    else
                    {
                        yCenterRatio = (lineCount / 2) - 1 - i;

                        if (yCenterRatio >= 0)
                            yCenterRatio += 1;
                    }
                }

                foundCalled = true;

                float colliderWidth = bottomRight - bottomLeft;

                float xCenter = bottomRight - bottomLeft;

                xCenter /= 2;

                xCenter += bottomLeft;

                float evenDist = yCenterRatio < 0 ?
                    (line.lineHeight * yCenterRatio) + (line.lineHeight / 2)
                    : (line.lineHeight * yCenterRatio) - (line.lineHeight / 2);

                if (!hasOddLineBreaks)
                    boxCollider.center = new(xCenter, evenDist, -0.09f); // even
                else
                    boxCollider.center = new(xCenter, line.lineHeight * yCenterRatio, -0.09f);

                boxCollider.size = new(colliderWidth, line.lineHeight, 0.2f);
            }
        }
    }
}