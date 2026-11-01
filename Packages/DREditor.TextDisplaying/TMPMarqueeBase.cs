//OG Author: KHeartz. My Magic Man
//Edited Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections;
using TMPro;
using UnityEngine;

namespace DREditor.TextDisplaying
{
    /// <summary>
    ///
    /// </summary>
    [RequireComponent(typeof(RectTransform))]
    public abstract class TMPMarqueeBase : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected TextMeshProUGUI tmp = null;


        /// <summary>
        ///
        /// </summary>
        protected RectTransform parentMask;

        /// <summary>
        ///
        /// </summary>
        protected RectTransform marqueeTransform;

        /// <summary>
        ///
        /// </summary>
        protected Coroutine activeMarqueeRoutine;


        protected virtual void Awake()
        {
            marqueeTransform = GetComponent<RectTransform>();

            parentMask = transform.parent.GetComponent<RectTransform>();
        }

        protected virtual void OnDisable()
        {
            if (activeMarqueeRoutine != null)
                StopCoroutine(activeMarqueeRoutine);
        }

        protected virtual void Start() => StartCoroutine(InitialDelay());

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator InitialDelay()
        {
            yield return null;

            yield return null;

            yield return null;

            Initialize();

            StartMarqueeLoop();
        }

        /// <summary>
        ///
        /// </summary>
        protected abstract void Initialize();


        /// <summary>
        ///
        /// </summary>
        protected virtual void StartMarqueeLoop()
        {
            StopAlreadyActiveLoop();

            StartMarqueeRoutine();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void StopAlreadyActiveLoop()
        {
            if (activeMarqueeRoutine != null)
                StopCoroutine(activeMarqueeRoutine);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void StartMarqueeRoutine() => activeMarqueeRoutine = StartCoroutine(MarqueeRoutine());

        /// <summary>
        ///
        /// </summary>
        protected abstract IEnumerator MarqueeRoutine();
    }
}