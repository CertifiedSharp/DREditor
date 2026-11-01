//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using UnityEngine;

namespace DREditor.UI.SavingLoading
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(SceneEventListener) + ": Should listen to OnFinishedLoadingRoom." +
        "\n\n")]
    public class SavePointUIGameState : SceneEventListener
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent isLoadingFile = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent controlsMenuForceDisable = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent skipActivateControls = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent onLastObjective = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inTrialMode = null;

        /// <summary>
        ///
        /// </summary>
        [Divider]

        [Tooltip("")]
        [SerializeField] protected IntWithEvent currentAddState = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int endChapterStateIndex = 4;


        /// <summary>
        ///
        /// </summary>
        protected virtual void Awake() => InitializeEndOfChapter();

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeEndOfChapter()
        {
            if (currentAddState != null && onLastObjective.Value)
                currentAddState.SetValue(endChapterStateIndex);
        }


        /// <inheritdoc/>
        protected override void OnEnable()
        {
            base.OnEnable();

            controlsMenuForceDisable.SetValue(true);

            if (!isLoadingFile.Value)
                RemoveControls();
        }

        /// <inheritdoc/>
        protected override void OnDisable()
        {
            base.OnDisable();

            controlsMenuForceDisable.SetValue(false);

            AddControls();
        }


        /// <inheritdoc/>
        public override void OnEventRaised()
        {
            base.OnEventRaised();

            if (!isLoadingFile.Value)
                AddControls();
        }


        /// <summary>
        ///
        /// </summary>
        protected virtual void RemoveControls() => skipActivateControls.SetValue(true);

        /// <summary>
        ///
        /// </summary>
        protected virtual void AddControls() => skipActivateControls.SetValue(false);
    }
}