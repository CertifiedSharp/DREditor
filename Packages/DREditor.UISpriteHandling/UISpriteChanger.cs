//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if DOTWEEN
using DG.Tweening;
#endif
using DREditor.DialogueEventHandling;
using DREditor.Dialogues;
using DREditor.Dialogues.Events;
using DREditor.EventObjects;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DREditor.UISpriteHandling
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DialogueEventHandler) +
        "\n\n")]
    public class UISpriteChanger : DialogueEventHandler
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string loadToLineEndEvent = "ProtagLoadToLineEndSpriteEvent";


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected RawImage charSpriteOne = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected RawImage charSpriteTwo = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inFastForwardMode = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected float uISpriteChangeTime = 0.185f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float changeSpriteBufferTime = 0;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float resetSpriteBufferTime = 0;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Vector3 uiSpritePosition = new(-72, -298, 0); // for center panel, y = -513
#if DOTWEEN
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Ease topSpriteFadeOutCurve = Ease.Linear;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Ease bottomSpriteFadeInCurve = Ease.Linear;
#endif

        /// <summary>
        ///
        /// </summary>
        protected Material currentExpression = null;


        /// <inheritdoc/>
        protected override void OnEnable()
        {
            base.OnEnable();

            DialogueEventSystem.StartListening(loadToLineEndEvent, OnLoadToLineEndEventTriggered);
        }

        /// <inheritdoc/>
        protected override void OnDisable()
        {
            base.OnDisable();

            DialogueEventSystem.StopListening(loadToLineEndEvent, OnLoadToLineEndEventTriggered);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        protected override void OnDialogueEventTriggered(object o = null) => EvaluateSpriteChange((Line)o);

        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        protected virtual void OnLoadToLineEndEventTriggered(object o = null) => EvaluateSpriteChange((Line)o);


        /// <summary>
        ///
        /// </summary>
        /// <param name="currentLine"></param>
        protected virtual void EvaluateSpriteChange(Line currentLine)
        {
            bool expressionIsValid = currentLine.Expression != null && currentLine.Expression.Sprite != null;

            if (expressionIsValid)
                EvaluateSpriteChangeProcess(currentLine);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="currentLine"></param>
        protected virtual void EvaluateSpriteChangeProcess(Line currentLine)
        {
            currentExpression = currentLine.Expression.Sprite;

            if (inFastForwardMode.Value)
                FastForwardChangeUISpriteProcess();
            else if (charSpriteOne != null)
                StartCoroutine(ChangeUISpriteProcess());
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void FastForwardChangeUISpriteProcess()
        {
            SetImagePositionInUI(charSpriteOne);

            ApplyTextureAndSizeToRawImage(charSpriteTwo, currentExpression.mainTexture);

            ApplyTextureAndSizeToRawImage(charSpriteOne, currentExpression.mainTexture);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator ChangeUISpriteProcess()
        {
            yield return new WaitForSeconds(changeSpriteBufferTime);

            SetImagePositionInUI(charSpriteOne);

            ApplyTextureAndSizeToRawImage(charSpriteTwo, currentExpression.mainTexture);

            AnimateSpriteChange(currentExpression.mainTexture);

            yield return new WaitForSeconds(uISpriteChangeTime + resetSpriteBufferTime);

            ResetImageTextures();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="rawImage"></param>
        /// <param name="texture"></param>
        protected virtual void ApplyTextureAndSizeToRawImage(RawImage rawImage, Texture texture)
        {
            rawImage.texture = texture;

            rawImage.rectTransform.SizeDeltaToTextureSize(texture);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="rawImage"></param>
        protected virtual void SetImagePositionInUI(RawImage rawImage) 
            => rawImage.rectTransform.localPosition = GetUISpritePosition();

        /// <summary>
        ///
        /// </summary>
        /// <param name="texture"></param>
        protected virtual void AnimateSpriteChange(Texture texture)
        {
#if DOTWEEN
            charSpriteTwo.DOFade(1, uISpriteChangeTime)
                    .SetEase(bottomSpriteFadeInCurve);

            charSpriteOne.DOFade(0, uISpriteChangeTime)
                .SetEase(topSpriteFadeOutCurve);
#endif
            charSpriteOne.rectTransform.SizeDeltaToTextureSize(texture);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ResetImageTextures()
        {
            charSpriteOne.texture = currentExpression.mainTexture;
#if DOTWEEN
            charSpriteOne.DOFade(1, 0);

            charSpriteTwo.DOFade(0, 0);
#endif
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual Vector3 GetUISpritePosition() => uiSpritePosition;

        /// <summary>
        ///
        /// </summary>
        /// <param name="pos"></param>
        public virtual void SetUISpritePosition(Vector2 pos) => uiSpritePosition = pos;


        /// <summary>
        /// 
        /// </summary>
        [ContextMenu("Set Default Event")]
        public void SetDefaultEvent() => SetDefaultEventString("ProtagSpriteLineEvent");
    }
}