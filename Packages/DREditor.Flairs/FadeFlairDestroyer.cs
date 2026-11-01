//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Kill/Cure: Beautiful Vice
#if DOTWEEN
using DG.Tweening;
using DREditor.Utility;
using UnityEngine.UI;
#endif
using System.Collections;
using UnityEngine;


namespace DREditor.Flairs
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(FlairDestroyer) + ", " + nameof(IFlairDestroyable) +
        "\n\n")]
    public class FadeFlairDestroyer : FlairDestroyer
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float fadeOutFlairTime = 0.5f;


        /// <inheritdoc/>
        public override void DestroyFlair() => StartCoroutine(FadeOutFlairsOnDestroy());

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator FadeOutFlairsOnDestroy()
        {
#if DOTWEEN
            FadeComponents<SpriteRenderer>((sprite) => sprite.DOColor(sprite.color.Invisible(), fadeOutFlairTime));

            FadeComponents<Image>((image) => image.DOColor(image.color.Invisible(), fadeOutFlairTime));
#endif
            yield return new WaitForSeconds(fadeOutFlairTime);

            base.DestroyFlair();
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="fade"></param>
        protected virtual void FadeComponents<T>(System.Action<T> fade) where T : Component
        {
            T[] components = gameObject.GetComponentsInChildren<T>();

            foreach (var component in components)
                if (component != null)
                    fade(component);
        }
    }
}