//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Audio;
using DREditor.Services;
using DREditor.UIHandling;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace DREditor.UI
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
    [ClassHeader(nameof(SelectableMenuGroupBase) + 
        "\n\n")]
    public class MenuGroup : SelectableMenuGroupBase
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected bool skipSelection = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool keepLastSelectedOnReset = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool keepCanvas = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool leaveCanvasOnlyOnBack = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool popUpBackInput = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool skipBackInput = false;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected Animator animator = null;


        /// <summary>
        ///
        /// </summary>
        protected Coroutine showCoroutine;


        /// <summary>
        ///
        /// </summary>
        protected override void Start()
        {
            InitializeSelectables();

            base.Start();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeSelectables()
        {
            if (selectables.Count == 0)
            {
                var childSelectables = GetComponentsInChildren<Selectable>().ToList();

                foreach(var child in childSelectables)
                    selectables.Add(child);
            }

            if (selectables.Count != 0)
                TurnOffSelectables();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void TurnOffSelectables()
        {
            if (!isActive)
                EnableSelectables(false);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="isEnabled"></param>
        public virtual void EnableSelectables(bool isEnabled)
        {
            foreach (Selectable selectable in selectables)
                selectable.enabled = isEnabled;
        }


        /// <inheritdoc/>
        public override void ChangeGroup(MenuGroupBase group)
        {
            if (Changing)
            {
                UnityEngine.Debug.LogWarning("ALERT: Menu's are still changing!");

                return;
            }

            ServiceLocator.GetService<MenuState>().SetChanging(true);

            UnityEngine.Debug.Log($"REPORT: Group: {gameObject.name} is changing to Group: " + group.gameObject.name);

            StartCoroutine(Change(group));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="group"></param>
        protected virtual IEnumerator Change(MenuGroupBase group)
        {
            LastSelected = EventSystem.current.currentSelectedGameObject;

            yield return null;

            EventSystem.current.SetSelectedGameObject(null);

            HideProcess(group);

            if (leaveCanvasOnlyOnBack && group == backGroup)
                yield return OnMovingToBackGroup();

            group.Show();

            if (group != backGroup)
                ServiceLocator.GetService<SFXManagerBase>().PlaySubmit();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator OnMovingToBackGroup()
        {
            HideCanvas();

            yield break;
        }


        /// <inheritdoc/>
        public override void Show()
        {
            UnityEngine.Debug.Log("REPORT: Attempting to Reveal Group: " + gameObject.name);

            if (isActive)
            {
                UnityEngine.Debug.Log($"REPORT: {gameObject.name} was already Active");

                ServiceLocator.GetService<MenuState>().SetChanging(false);

                return;
            }

            EnableSelectables(true);

            BeforeStart?.Invoke();

            showCoroutine = StartCoroutine(ShowProcess());
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator ShowProcess()
        {
            UnityEngine.Debug.Log("REPORT: Starting Evaluate Selection for Group: " + gameObject.name);

            if (!skipSelection)
                yield return SetEvaluateSelection();

            yield return null;

            yield return null;

            UnityEngine.Debug.Log("REPORT: Evaluate Selection ended for Group: " + gameObject.name);

            if (!skipBackInput)
                AddBackInput();

            StartEvents?.Invoke();

            SetActive(true);

            ServiceLocator.GetService<MenuState>().SetChanging(false);
        }


        /// <inheritdoc/>
        public override void Hide() => HideProcess();

        /// <summary>
        ///
        /// </summary>
        /// <param name="shownGroup"></param>
        protected virtual void HideProcess(MenuGroupBase shownGroup = null)
        {
            RemoveBackInput();

            EnableSelectables(false);

            EndEvents?.Invoke();

            if (!ServiceLocator.GetService<UIHandlerBase>().MovingToTitleScreen && popUpBackInput && backGroup != null)
            {
                backGroup.AddBackInput();

                backGroup.SetEvaluateSelect();
            }

            if (HideCanvasEvaluation())
                HideCanvas();

            OnHideComplete();

            UnityEngine.Debug.Log($"REPORT: {gameObject.name}'s Hide process finished and should now not be active.");
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void HideCanvas()
        {
            if (GetComponent<CanvasUIVisibility>() is CanvasUIVisibility canvas)
                canvas.Hide();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnHideComplete() => StopBeingActive();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool HideCanvasEvaluation() => !keepCanvas && !leaveCanvasOnlyOnBack;

        /// <summary>
        ///
        /// </summary>
        protected virtual void StopBeingActive()
        {
            if (showCoroutine != null)
                StopCoroutine(showCoroutine);

            SetActive(false);
        }


        /// <inheritdoc/>
        protected override void ResetGroup()
        {
            if (!keepLastSelectedOnReset)
                LastSelected = null;

            if (isActive)
                HideProcess();

            RemoveBackInput();
        }
    }
}