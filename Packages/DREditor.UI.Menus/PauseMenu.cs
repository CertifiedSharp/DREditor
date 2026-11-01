//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Audio;
using DREditor.EventObjects;
using DREditor.Services;
using DREditor.UIHandling;
using System.Collections;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static UnityEngine.InputSystem.InputAction;

namespace DREditor.UI.Menus
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    ///
    /// </remarks>
	[ClassHeader("")]
    public class PauseMenu : ServiceHolder<PauseMenu>, IToTitleScreen
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected InputActionReference pause = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected InputActionReference cancel = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected PauseOptionData pauseData = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEvent onResumeFinished = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent pauseAccess = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inMenu = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inPause = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected SelectableMenuGroupBase firstGroup = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected EvaluateCondition openPauseMenuCondition = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected UIVisibilityComponent pauseIntroVisibility = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected UIVisibilityComponent pauseVisibility = null;


        [Header("")]
        [SerializeField] protected UnityEvent OnPauseMenuShow;


        [Header("")]
        [SerializeField] protected UnityEvent OnPauseMenuHide;


        /// <summary>
        ///
        /// </summary>
        protected bool resuming = false;

        /// <summary>
        ///
        /// </summary>
        protected bool fakeOverride = false;


        /// <summary>
        ///
        /// </summary>
        protected virtual void OnEnable() => pauseAccess.AddListener(OnPauseAccessChanged);

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDisable()
        {
            pauseAccess.RemoveListener(OnPauseAccessChanged);

            pause.action.started -= Pause;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnPauseAccessChanged(bool hasAccess)
        {
            if (hasAccess)
                pause.action.started += Pause;
            else
                pause.action.started -= Pause;
        }


        /// <summary>
        ///
        /// </summary>
        protected virtual void Pause(CallbackContext context)
        {
            if (!resuming && openPauseMenuCondition.Evaluate())
                StartPause();
            else if (resuming)
                UnityEngine.Debug.Log("ANALYSIS: Failed to open the pause menu because the game was resuming.");
        }

        /// <summary>
        ///
        /// </summary>
        public virtual void StartPause()
        {
            UnityEngine.Debug.Log("REPORT: " + nameof(PauseMenu) + ": Pausing.");

            ServiceLocator.GetService<SoundManagerBase>().StopAllButMusic();

            ServiceLocator.GetService<VOManagerBase>().StopVoiceLine();

            inMenu.Value = true;

            inPause.SetValue(true);

            OnPauseMenuShow?.Invoke();

            Time.timeScale = 0;

            InitializePauseOptions();

            if (pauseIntroVisibility != null)
                pauseIntroVisibility.Show();
            else
                ShowPauseGroup();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializePauseOptions()
        {
            for (int i = 0; i < firstGroup.selectables.Count && i < pauseData.pauseOptions.Length; i++)
                SetUnlockableOptionState(firstGroup.selectables[i], pauseData.pauseOptions[i]);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="selectable"></param>
        /// <param name="selectableUnlockState"></param>
        protected virtual void SetUnlockableOptionState(Selectable selectable, bool selectableUnlockState)
        {
            if (selectable != null && selectable.gameObject.activeSelf != selectableUnlockState)
                selectable.gameObject.SetActive(selectableUnlockState);

            if (selectable != null && selectable.gameObject.GetComponent<IUnlockableUIOption>() is IUnlockableUIOption option)
                selectable.gameObject.SetActive(option.IsUnlocked());
        }

        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        public virtual void ShowPauseGroup()
        {
            pauseVisibility.Show();

            if (fakeOverride)
                return;

            firstGroup.Show();

            cancel.action.started += UnPause;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        protected virtual void UnPause(CallbackContext context)
        {
            if (firstGroup.isActive && !ServiceLocator.GetService<MenuState>().Changing)
                Resume();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void Resume()
        {
            UnityEngine.Debug.Log(nameof(PauseMenu) + ": Unpausing.");

            cancel.action.started -= UnPause;

            EventSystem.current.SetSelectedGameObject(null);

            MenuGroupBase[] groups = GetComponentsInChildren<MenuGroupBase>();

            foreach (MenuGroupBase group in groups)
                group.RemoveBackInput();

            HidePauseGroup();
        }

        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        public virtual void HidePauseGroup() => StartCoroutine(HidePauseGroupRoutine());

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator HidePauseGroupRoutine()
        {
            resuming = true;

            firstGroup.Hide();

            OnPauseMenuHide?.Invoke();

            if (pauseIntroVisibility != null)
            {
                pauseIntroVisibility.Hide();

                yield return pauseIntroVisibility.WaitForChangeToComplete();
            }

            yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime * 2);

            if (pauseIntroVisibility == null)
                pauseVisibility.Hide();

            Time.timeScale = 1;

            onResumeFinished.Raise();

            resuming = false;

            inMenu.Value = false;

            inPause.SetValue(false);
        }

        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        public virtual void ShowFakePause()
        {
            fakeOverride = true;

            if (pauseIntroVisibility != null)
                pauseIntroVisibility.Show();
        }

        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        public virtual void HideFakePause()
        {
            fakeOverride = false;

            if (pauseIntroVisibility != null)
                pauseIntroVisibility.Hide();
        }

        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        public virtual void ResetPauseMenu() => Resume();


        /// <inheritdoc/>
        public virtual void ToTitleScreen() => cancel.action.started -= UnPause;
    }
}