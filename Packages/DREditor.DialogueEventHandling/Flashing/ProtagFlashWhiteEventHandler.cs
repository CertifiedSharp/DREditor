//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Kill/Cure
using DREditor.Dialogues;
using DREditor.EventObjects;
using DREditor.Services;
using DREditor.Utility;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DREditor.DialogueEventHandling.Flashing
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DialogueEventHandler) + ", " + nameof(FlashEventHandlerBase) + ", " + nameof(FlashEventSimpleHandlerBase) +
        "\n\n")]
    public class ProtagFlashWhiteEventHandler : FlashEventSimpleHandlerBase
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Graphic protagImage = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent isLoadingFile = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string shaderStringTextureRef = "_MainTex";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Color flashColor = Color.white;


        /// <summary>
        ///
        /// </summary>
        protected Transform graphicsParent = null;

        /// <summary>
        ///
        /// </summary>
        protected RawImage rawImageMask;

        /// <summary>
        ///
        /// </summary>
        protected Mask mask;

        /// <summary>
        ///
        /// </summary>
        protected Image flashImage;


        /// <inheritdoc/>
        protected override bool FailFlash()
        {
            bool result = isLoadingFile.Value || protagImage == null;

            if (protagImage == null)
                UnityEngine.Debug.LogWarning($"ALERT: Event {dialogueEventString} does not have " +
                    $"it's {nameof(protagImage)} Graphic assigned!");

            return result;
        }


        /// <inheritdoc/>
        protected override void DisplayFlashEvent() => StartCoroutine(BufferThenDisplay());

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator BufferThenDisplay()
        {
            yield return null;

            CreateMask();

            CreateFlashImage();

            SetImagesParentToMask();

            SetMaskToCurrentLineSprite();

            base.DisplayFlashEvent();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void CreateMask()
        {
            graphicsParent = protagImage.transform.parent;

            GameObject maskObject = new();

            maskObject.transform.SetParent(graphicsParent);

            rawImageMask = maskObject.AddComponent<RawImage>();

            rawImageMask.rectTransform.position = protagImage.rectTransform.position;

            rawImageMask.rectTransform.localScale = protagImage.rectTransform.localScale;

            rawImageMask.rectTransform.rotation = protagImage.rectTransform.rotation;

            mask = maskObject.AddComponent<Mask>();

            mask.showMaskGraphic = false;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void CreateFlashImage()
        {
            GameObject overlay = new();

            flashImage = overlay.AddComponent<Image>();

            if (endValue < 0)
                flashImage.color = Color.black;
            else
                flashImage.color = flashColor;

            flashImage.SetFade(0);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void SetImagesParentToMask()
        {
            protagImage.transform.SetParent(rawImageMask.gameObject.transform);

            flashImage.transform.SetParent(rawImageMask.transform);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void SetMaskToCurrentLineSprite()
        {
            var currentLine = ServiceLocator.GetService<IDialogueManager>().GetReader().GetCurrentLine();

            if (currentLine != null && currentLine.Expression != null && currentLine.Expression.Sprite != null)
                rawImageMask.texture = currentLine.Expression.Sprite.GetTexture(shaderStringTextureRef);
            else
                rawImageMask.texture = protagImage.mainTexture;

            rawImageMask.SetNativeSize();

            flashImage.rectTransform.sizeDelta = rawImageMask.rectTransform.sizeDelta;

            flashImage.rectTransform.localPosition = Vector3.zero;
        }

        /// <inheritdoc/>
        protected override void FlashUpdate(float value)
        {
            if (value < 0)
                value *= -1;

            flashImage.SetFade(value);
        }

        /// <inheritdoc/>
        protected override void OnDisable()
        {
            base.OnDisable();

            ResetFlash();
        }

        /// <inheritdoc/>
        protected override void ResetFlash()
        {
            DestroyMask();

            ClearData();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DestroyMask()
        {
            if (rawImageMask != null)
            {
                graphicsParent = rawImageMask.transform.parent;

                protagImage.transform.SetParent(rawImageMask.transform.parent);

                Destroy(rawImageMask.gameObject);
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ClearData()
        {
            rawImageMask = null;

            mask = null;

            flashImage = null;
        }
    }
}