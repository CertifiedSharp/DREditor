//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if DOTWEEN
using DG.Tweening;
using DREditor.Fading;
#endif
using DREditor.Dialogues;
using DREditor.EventObjects;
using DREditor.Services;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Video;

namespace DREditor.DialogueMechanics.CGs
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(IToTitleScreen) +
        "\n\n")]
    public class CGDisplayHandler : MonoBehaviour, IToTitleScreen, ISceneEventListener
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEvent onFadeOutComplete = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inCGStateMode = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inFastForwardMode = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent cgDialogueUIVisible = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected Image CG = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Image SubCG = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected float inCGTransitionTime = 1;


        /// <summary>
        ///
        /// </summary>
        protected GameObject activePrefab = null;

        /// <summary>
        ///
        /// </summary>
        protected Animator prefabAnimator = null;

        /// <summary>
        ///
        /// </summary>
        protected VideoPlayer prefabVideo = null;

        /// <summary>
        ///
        /// </summary>
        protected bool waitingToHide = false;

        /// <summary>
        /// 
        /// </summary>
        protected bool registered = false;


        protected virtual void OnEnable() => cgDialogueUIVisible.AddListener(OnCGUIVisibilityChanged);

        protected virtual void OnDisable() => cgDialogueUIVisible.RemoveListener(OnCGUIVisibilityChanged);

        /// <summary>
        ///
        /// </summary>
        /// <param name="visible"></param>
        protected virtual void OnCGUIVisibilityChanged(bool visible)
        {
            if (registered && visible)
                UnregisterCGImageFade();

            if (!visible && CG.color.a != 0)
                HideCGsWhenDark();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void HideCGsWhenDark()
        {
            if (waitingToHide)
                return;
            else
                waitingToHide = true;

            UnityEngine.Debug.Log($"REPORT: {nameof(CGDisplayHandler)} is waiting for the fade UI to go dark.");

            RegisterCGImageFade();
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void RegisterCGImageFade()
        {
            onFadeOutComplete.RegisterListener(this);

            registered = true;
        }

        /// <inheritdoc/>
        public virtual void OnEventRaised()
        {
            UnregisterCGImageFade();
#if DOTWEEN
            CG.DOFade(0, 0);

            SubCG.DOFade(0, 0);
#endif
            waitingToHide = false;

            UnityEngine.Debug.Log($"REPORT: {nameof(CGDisplayHandler)} Finished waiting and the CG's will not display.");
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void UnregisterCGImageFade()
        {
            onFadeOutComplete.UnregisterListener(this);

            registered = false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="prefab"></param>
        /// <remarks></remarks>
        public virtual IEnumerator StartCGRoutine(Sprite sprite, GameObject prefab)
        {
            SetStartCGSprite(sprite, prefab);

            yield break;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="prefab"></param>
        protected virtual void SetStartCGSprite(Sprite sprite, GameObject prefab)
        {
            inCGStateMode.SetValue(true);

            if (prefab != null)
                InstantiatePrefabCG(prefab);

            CG.sprite = sprite;
#if DOTWEEN
            if (prefab == null)
                CG.DOFade(1, 0);
#endif
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="prefab"></param>
        protected virtual void InstantiatePrefabCG(GameObject prefab)
        {
            activePrefab = Instantiate(prefab);

            DontDestroyOnLoad(activePrefab);

            prefabAnimator = activePrefab.GetComponent<Animator>();

            prefabVideo = activePrefab.GetComponentInChildren<VideoPlayer>();
        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="prefab"></param>
        /// <param name="transitionInstant"></param>
        /// <param name="checkFlashback"></param>
        public virtual IEnumerator InCGRoutine(Sprite sprite, GameObject prefab, bool transitionInstant, IEnumerator checkFlashback)
        {
            if (prefab != null) // CG prefab will be instantiated
                yield return InstantiatePrefabCGVideo(prefab);

            yield return checkFlashback;

            if (activePrefab == null)
                yield return TransitionCGRoutine(sprite, transitionInstant);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="prefab"></param>
        protected virtual IEnumerator InstantiatePrefabCGVideo(GameObject prefab)
        {
            InstantiatePrefabCG(prefab);

            if (prefabVideo != null)
                yield return new WaitUntil(() => prefabVideo.isPrepared);
#if DOTWEEN
            CG.DOFade(0, inCGTransitionTime); // because we're going from cg to video
#endif
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="sprite"></param>
        /// <param name="transitionInstant"></param>
        protected virtual IEnumerator TransitionCGRoutine(Sprite sprite, bool transitionInstant)
        {
#if DOTWEEN
            SubCG.DOFade(1, 0);
#endif
            SubCG.sprite = sprite;

            float transitionTime = transitionInstant ? 0 : inCGTransitionTime;
#if DOTWEEN
            CG.DOFade(0, transitionTime);
#endif
            if (inFastForwardMode == null || !inFastForwardMode.Value)
                yield return new WaitForSeconds(transitionTime);
#if DOTWEEN
            CG.DOKill();
#endif
            CG.sprite = sprite;
#if DOTWEEN
            if (CG.sprite != null)
                CG.DOFade(1, 0);

            yield return null;

            SubCG.DOFade(0, 0);
#endif
        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="playOnly"></param>
        public virtual IEnumerator WaitForAnimatorToFinish(bool playOnly)
        {
            if (prefabAnimator != null && playOnly)
                yield return prefabAnimator.WaitForStateToComplete(0);
        }


        /// <summary>
        ///
        /// </summary>
        public virtual void EndCGPrefab()
        {
            if (activePrefab != null)
            {
                Destroy(activePrefab);

                activePrefab = null;

                prefabAnimator = null;

                prefabVideo = null;
            }
        }

        /// <inheritdoc/>
        public virtual void ToTitleScreen() => EndCGPrefab();
    }
}