//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using System.Collections;
using System.Linq;
using UnityEngine;

namespace DREditor.Minigames.Debates.Firing
{
    /// <summary>
    ///
    /// </summary>
    public abstract class FiredTruthHandler : MonoBehaviour, IFireBulletUI
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEvent onPhraseShot = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected FireTruthAnimationBase fireAnimation = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected float fallbackRemainingDuration = 1;


        /// <summary>
        ///
        /// </summary>
        protected IReloadBulletUI reloadBulletHandler = null;


        protected virtual void Awake() => InitializeComponents();


        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual Component[] InitializeComponents()
        {
            Component[] allComponents = transform.root.GetComponentsInChildren<Component>();

            reloadBulletHandler ??= allComponents.OfType<IReloadBulletUI>().FirstOrDefault();

            return allComponents;
        }


        /// <inheritdoc/>
        public virtual void FireUITruthBullet() => StartCoroutine(EvaluateBulletAnimation());

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator EvaluateBulletAnimation()
        {
            yield return null;

            if (fireAnimation != null)
                yield return fireAnimation.FireAnimation();

            // Past here should be the moment the first letter hits the text or just as we're about to slash.

            yield return EvaluateBulletHit();
        }

        /// <summary>
        ///
        /// </summary>
        protected abstract IEnumerator EvaluateBulletHit();

        /// <summary>
        ///
        /// </summary>
        protected virtual void InvokePhraseShot()
        {
            if (onPhraseShot != null)
                onPhraseShot.Raise();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator WaitForFireAnimation()
        {
            float remainingDuration = fireAnimation != null ? fireAnimation.CalculateRemainingDuration() : fallbackRemainingDuration;

            yield return new WaitForSecondsRealtime(remainingDuration);

            reloadBulletHandler?.Reload();
        }
    }
}