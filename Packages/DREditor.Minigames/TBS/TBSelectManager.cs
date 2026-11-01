//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using DREditor.UI;
using DREditor.UIHandling;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace DREditor.Minigames.TBS
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(MinigameManagerBase) + ", " +
        "\n\n")]
    public class TBSelectManager : MinigameManagerBase
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected IntWithEvent currentChapterIndex = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected QuestionToggle questionToggle = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected bool debug = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected TBSelectBuilder builder = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [FormerlySerializedAs("menu")]
        [SerializeField] protected MenuGroupBase truthBulletMenu = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected MenuGroupBase confirmUI = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected UIVisibilityComponent answerUI = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float menuFadeWaitTime = 0.5f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float postIntroBufferDuration = 0.75f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float resolveRestartBufferDuration = 1;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool lastSelectedOnRestart = false;


        /// <summary>
        ///
        /// </summary>
        protected Button lastButton = null;

        /// <summary>
        ///
        /// </summary>
        protected bool inConfirmUI = false;

        /// <summary>
        ///
        /// </summary>
        protected Button[] buttons;


        /// <summary>
        ///
        /// </summary>
        protected virtual void Awake()
        {
            if (debug)
                currentChapterIndex.SetValue(builder.chapter);
        }

        /// <inheritdoc/>
        protected override void OnMinigameDisable()
        {
            base.OnMinigameDisable();

            foreach (var button in buttons)
                button.onClick.RemoveAllListeners();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void Start()
        {
            if (debug)
                StartSelection(builder);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="asset"></param>
        public void PlayTBSelect(ScriptableObject asset) => FindAnyObjectByType<TBSelectManager>().StartSelection((TBSelectBuilder)asset);

        /// <summary>
        ///
        /// </summary>
        /// <param name="builder"></param>
        public virtual void StartSelection(TBSelectBuilder builder)
        {
            this.builder = builder;

            StartCoroutine(BeginSelection());
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator BeginSelection()
        {
            yield return new WaitForSecondsRealtime(Time.unscaledDeltaTime);

            truthBulletMenu.Show();

            EventSystem.current.SetSelectedGameObject(GetComponentsInChildren<Selectable>().First().gameObject);

            if (TrialTimer != null)
                TrialTimer.ShowInitialize(builder.times, true);

            yield return new WaitForSecondsRealtime(postIntroBufferDuration);

            questionToggle.Initialize(builder.question);

            buttons = GetComponentsInChildren<Button>();

            foreach (var button in buttons)
                button.onClick.AddListener(EnterConfirmUI);

            yield break;
        }


        /// <summary>
        ///
        /// </summary>
        protected virtual void EnterConfirmUI()
        {
            if (inConfirmUI) 
                return;

            inConfirmUI = true;

            lastButton = EventSystem.current.currentSelectedGameObject.GetComponent<Button>();

            EventSystem.current.SetSelectedGameObject(null);

            confirmUI.Show(); // The Menu Group will auto select it for us
        }


        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        public virtual void LeaveConfirmUI() => StartCoroutine(BufferLeaveConfirmUI());

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator BufferLeaveConfirmUI()
        {
            EventSystem.current.SetSelectedGameObject(null);

            confirmUI.Hide();

            yield return confirmUI.WaitForChangeToComplete();
            
            EventSystem.current.SetSelectedGameObject(lastButton.gameObject);

            inConfirmUI = false;
        }


        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        public void ConfirmSelection()
        {
            EventSystem.current.SetSelectedGameObject(null);

            confirmUI.Hide();

            TrialTimer.PauseTimer();

            HideVisuals();

            var selectionText = lastButton.GetComponentInChildren<TextMeshProUGUI>();

            TBSelection selection = builder.GetSelection(selectionText.text);

            if (selection != null && selection.isAnswer)
                StartCoroutine(ResolveMinigame());
            else
                TrialMistake(selection.wrongDialogue);
        }

        /// <inheritdoc/>
        public override IEnumerator ResolveMinigame()
        {
            yield return new WaitForSeconds(menuFadeWaitTime);

            if (answerUI != null)
                yield return answerUI.ShowTillComplete();

            yield return base.ResolveMinigame();

            yield break;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void HideVisuals()
        {
            truthBulletMenu.Hide();

            TrialTimer.Hide();

            questionToggle.Deactivate();
        }

        /// <inheritdoc/>
        protected override void Restart()
        {
            truthBulletMenu.Show();

            var selected = lastButton != null && lastSelectedOnRestart ?
                lastButton.gameObject : GetComponentsInChildren<Selectable>().First().gameObject;

            EventSystem.current.SetSelectedGameObject(selected);

            TrialTimer.Show();

            questionToggle.Activate();

            StartCoroutine(ResolveRestart());
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator ResolveRestart()
        {
            yield return new WaitForSecondsRealtime(resolveRestartBufferDuration);

            TrialTimer.ResumeTimer();

            inConfirmUI = false;

            yield break;
        }

        /// <inheritdoc/>
        protected override void OnTimeUp()
        {
            EventSystem.current.SetSelectedGameObject(null);

            confirmUI.Hide();

            HideVisuals();

            base.OnTimeUp();
        }
    }
}