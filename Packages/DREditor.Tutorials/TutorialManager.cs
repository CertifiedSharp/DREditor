//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Services;
using DREditor.UI;
using DREditor.UIHandling;
using DREditor.Utility;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DREditor.Tutorials
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(IToTitleScreen) + ", " + nameof(IComponentsCache) +
        "\n\n")]
    public class TutorialManager : MonoBehaviour, IToTitleScreen, IComponentsCache
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected IndexSelector selector = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool freezeOnScreenStart = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected UIVisibilityComponent visibilityComponent = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<TutorialComponentBase> tutorialComponents = new();


        /// <summary>
        ///
        /// </summary>
        protected TutorialAsset asset = null;

        /// <summary>
        ///
        /// </summary>
        private bool active = false;

        /// <summary>
        ///
        /// </summary>
        protected readonly List<Coroutine> changeRoutines = new();


        /// <summary>
        ///
        /// </summary>
        public bool Active { get => active; }

        public MonoBehaviour[] CachedComponents => tutorialComponents.ToArray();


        /// <summary>
        ///
        /// </summary>
        /// <param name="active"></param>
        protected virtual void SetActive(bool active) => this.active = active;

        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        public virtual void TurnOn(object o = null)
        {
            if (FailedInitializing(o))
                return;

            StartCoroutine(DisplayUI());

            UpdateFreezeStateTo();

            InitializeSelector();

            SetActive(true);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        /// <returns></returns>
        protected virtual bool FailedInitializing(object o = null)
        {
            bool sentAssetValid = o != null;

            if (sentAssetValid)
                Initialize((TutorialAsset)o);
            else if (active)
                TurnOff();

            return !sentAssetValid;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="asset"></param>
        protected virtual void Initialize(TutorialAsset asset)
        {
            this.asset = asset;

            StartCoroutine(BufferDeselect());

            tutorialComponents.InvokeComponents((component) => component.Initialize(asset));
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator BufferDeselect()
        {
            yield return new WaitForEndOfFrame();

            EventSystem.current.SetSelectedGameObject(null);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator DisplayUI()
        {
            if (visibilityComponent == null)
                yield break;

            if (!active)
                visibilityComponent.Show();
            else
                yield return visibilityComponent.HideTillComplete();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void UpdateFreezeStateTo()
        {
            if (freezeOnScreenStart)
                Time.timeScale = !active ? 0 : 1;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeSelector()
        {
            if (selector == null)
                return;

            selector.ActivateControls();

            selector.OnNavigated.AddListener(ChangePage);

            selector.SelectionMaximum = asset.GetPageCount() - 1;
        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="ctx"></param>
        protected virtual void ChangePage(int ctx)
        {
            if (SkipChangingPage())
                return;

            StartCoroutine(ChangePageRoutine(ctx));
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool SkipChangingPage() =>
            (!freezeOnScreenStart && Time.timeScale == 0) || asset.OnlySinglePage();

        /// <summary>
        ///
        /// </summary>
        /// <param name="newIndex"></param>
        protected virtual IEnumerator ChangePageRoutine(int newIndex)
        {
            if (selector != null)
                selector.DeactivateControls();

            changeRoutines.Clear();

            this.InvokeComponents<TutorialComponent>((component) =>
            changeRoutines.Add(StartCoroutine(component.ChangePageRoutine(newIndex))));

            foreach (var routine in changeRoutines)
                yield return routine;

            this.InvokeComponents<TutorialComponent>((component) => component.UpdateCurrentIndex(newIndex));

            if (selector != null)
                selector.ActivateControls();
        }


        /// <summary>
        ///
        /// </summary>
        public virtual void TurnOff()
        {
            if (!active)
                return;

            ResolveSelector();

            StopAllCoroutines();

            StartCoroutine(TurnOffRoutine());
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ResolveSelector()
        {
            if (selector == null)
                return;

            selector.DeactivateControls();

            selector.OnNavigated.RemoveListener(ChangePage);

            selector.ResetIndexSelector();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator TurnOffRoutine()
        {
            PreResolveComponents();

            yield return DisplayUI();

            ResolveComponents();

            UpdateFreezeStateTo();

            SetActive(false);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void PreResolveComponents() =>
            this.InvokeComponents<TutorialComponent>((component) => component.PreResolve());

        /// <summary>
        ///
        /// </summary>
        protected virtual void ResolveComponents() => this.InvokeComponents<TutorialComponent>(ResolveComponent);

        /// <summary>
        ///
        /// </summary>
        /// <param name="component"></param>
        protected virtual void ResolveComponent(TutorialComponent component)
        {
            component.Resolve();

            component.UpdateCurrentIndex(0);
        }


        protected virtual void OnDestroy() => ResolveSelector();


        /// <inheritdoc/>
        public virtual void ToTitleScreen() => TurnOff();
    }
}