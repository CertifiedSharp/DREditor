//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if DOTWEEN
using DG.Tweening;
#endif
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace DREditor.Minigames.BS
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    ///
    /// </remarks>
    [ClassHeader(nameof(BlackenedNameScroller) +
        "\n\n")]
    public class ColoredNameScoller : BlackenedNameScroller
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected bool useDefaultProcess = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Image fillImage = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float xOffset = 50;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float xMoveDuration = 0.5f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float fillImageDuration = 0.5f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float textFadeDuration = 0.25f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<Color> colors = new();


        /// <inheritdoc/>
        public override void Animate()
        {
            if (useDefaultProcess)
                base.Animate();
            else
                StartCoroutine(AnimateTextRoutine());
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator AnimateTextRoutine()
        {
            yield return HideTextRoutine();

            DisplayCharacterName();

            yield return ShowTextRoutine();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator HideTextRoutine()
        {
#if DOTWEEN
            if (fillImage != null)
                fillImage.DOFillAmount(0, fillImageDuration);

            nameText.rectTransform.DOMoveX(defaultPosition.x - xOffset, xMoveDuration);

            nameText.DOFade(0, textFadeDuration);

            yield return new WaitForSeconds(textFadeDuration);

            nameText.rectTransform.DOMoveX(defaultPosition.x + xOffset, 0);
#endif
            yield return new WaitForEndOfFrame();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual IEnumerator ShowTextRoutine()
        {
#if DOTWEEN
            nameText.rectTransform.DOMoveX(defaultPosition.x, xMoveDuration);

            if (fillImage != null)
                fillImage.DOFillAmount(1, fillImageDuration);

            nameText.DOFade(1, textFadeDuration);
#endif
            yield break;
        }


        /// <inheritdoc/>
        protected override void DisplayCharacterName()
        {
            base.DisplayCharacterName();

            if (seatPosition < colors.Count)
                SetColorOfFirstCharacter(colors[seatPosition]);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="color"></param>
        protected virtual void SetColorOfFirstCharacter(Color color)
        {
            string hexString = ColorUtility.ToHtmlStringRGB(color);

            char firstLetter = nameText.text[0];

            string coloredLetter = "<color=#" + hexString + ">" + firstLetter.ToString() + "</color>";

            nameText.text = coloredLetter + nameText.text[1..];
        }
    }
}