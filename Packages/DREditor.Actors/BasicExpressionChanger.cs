//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if DOTWEEN
using DG.Tweening;
#endif
using DREditor.EventObjects;
using System.Collections;
using UnityEngine;

namespace DREditor.Actors
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    ///
    /// </remarks>
    [ClassHeader(nameof(ExpressionChanger) +
        "\n\n")]
    public class BasicExpressionChanger : ExpressionChanger
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
        [SerializeField] protected BoolWithEvent spriteIsFading = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected MeshRenderer character = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected MeshRenderer characterb = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SpriteRenderer sprite = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SpriteRenderer spriteb = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected float spriteChangeTime = 0.185f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float finishedWorldSpriteChangeBufferTime = 0;
#if DOTWEEN
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Ease bottomSpriteFadeInCurve = Ease.Linear;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Ease topSpriteFadeOutCurve = Ease.Linear;
#endif

        /// <inheritdoc/>
        public override void FastForwardExpressionChange(Material expressionMaterial, Sprite expressionSprite)
        {
            if (spriteIsFading.Value)
            {
#if DOTWEEN
                sprite.DOKill();
#endif
                sprite.color = new Color(255, 255, 255, 255);
                /* The above line causes a visual kinda bug where pressing fast forward, a sprite changes, then 
                     * going out of fast forward, will incorrectly fade the sprite transition. But this line seems 
                     * needed for fixing the bug of CharLeave overpowering sprite change.
                     */
            }

            SetActorMaterials(expressionMaterial);

            sprite.sprite = expressionSprite;

            spriteb.sprite = expressionSprite;

            if (!isLoadingFile.Value)
                MoveSpriteToFront();
        }

        /// <inheritdoc/>
        public override IEnumerator ChangeExpressionProcess(Material expressionMaterial, Sprite expressionSprite)
        {
            SetActorMaterials(expressionMaterial);

            spriteb.sprite = expressionSprite;
#if DOTWEEN
            spriteb.DOFade(1, spriteChangeTime).SetEase(bottomSpriteFadeInCurve);

            sprite.DOFade(0, spriteChangeTime).SetEase(topSpriteFadeOutCurve);
#endif
            yield return new WaitForSeconds(spriteChangeTime);

            sprite.sprite = expressionSprite;

            MoveSpriteToFront();

            if (finishedWorldSpriteChangeBufferTime > 0)
                yield return new WaitForSeconds(finishedWorldSpriteChangeBufferTime);
        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="material"></param>
        protected virtual void SetActorMaterials(Material material)
        {
            character.material = material;

            characterb.material.mainTexture = character.material.mainTexture;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void MoveSpriteToFront()
        {
#if DOTWEEN
            sprite.DOFade(1, 0);

            spriteb.DOFade(0, 0);
#endif
        }
    }
}