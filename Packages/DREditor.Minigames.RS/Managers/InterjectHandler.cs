//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using DREditor.Processables;
using DREditor.UIHandling;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace DREditor.Minigames.RS.Managers
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(InterjectHandlerBase) +
        "\n\n")]
    public class InterjectHandler : InterjectHandlerBase
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected InitializableBase multiPanelManager = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Graphic shatterGraphic = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent isLoadingFile = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float postShatterBufferTime = 0.75f;


        /// <summary>
        ///
        /// </summary>
        protected bool pastPostShatter = false;


        /// <inheritdoc/>
        public override IEnumerator InterjectionProcess(GameObject interjectionObject, bool skipEarly = false)
        {
            if (skipEarly || isLoadingFile.Value)
            {
                Destroy(shatterGraphic.transform.parent.gameObject);

                yield break;
            }

            yield return SpawnInterjectAnimationObject(interjectionObject);

            StartCoroutine(ShatterInterject());

            yield return WaitForPostShatterCompletion();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="interjectionObject"></param>
        protected virtual IEnumerator SpawnInterjectAnimationObject(GameObject interjectionObject)
        {
            if (interjectionObject == null)
                yield break;

            GameObject gameObject = Instantiate(interjectionObject);

            if (gameObject != null && gameObject.GetComponent<Animator>() is Animator animator)
                yield return new WaitForSeconds(animator.GetCurrentAnimatorClipInfo(0)[0].clip.length);
            else if (gameObject != null && gameObject.GetComponent<Animation>() is Animation animation)
                yield return new WaitForSeconds(animation.clip.length);
            else if (gameObject != null && gameObject.GetComponent<UIVisibilityComponent>() is UIVisibilityComponent handler)
                yield return handler.ShowTillComplete();
            else
                UnityEngine.Debug.LogWarning($"ALERT: Spawned Interject Failed: " +
                    $"{gameObject != null}  {gameObject.GetComponent<Animator>() != null}");

            if (gameObject != null)
                Destroy(gameObject);

            yield break;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator ShatterInterject()
        {
            IShatterGraphic shatter = (IShatterGraphic)shatterGraphic;

            if (multiPanelManager != null)
                shatter?.OnPostShatter.AddListener(multiPanelManager.Initialize);

            shatter?.OnPostShatter.AddListener(OnPostShatter);

            yield return shatter?.ShatterTheGraphic();

            if (multiPanelManager != null)
                shatter?.OnPostShatter.RemoveListener(multiPanelManager.Initialize);

            shatter?.OnPostShatter.RemoveListener(OnPostShatter);

            Destroy(shatterGraphic);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnPostShatter() => pastPostShatter = true;

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator WaitForPostShatterCompletion()
        {
            if (shatterGraphic is IShatterGraphic shatter && shatter != null)
                while (!pastPostShatter)
                    yield return null;

            yield return new WaitForSeconds(postShatterBufferTime);

            pastPostShatter = false;
        }

        protected virtual void OnDestroy()
        {
            if (multiPanelManager != null && shatterGraphic is IShatterGraphic shatter)
                shatter?.OnPostShatter.RemoveListener(multiPanelManager.Initialize);
        }
    }
}