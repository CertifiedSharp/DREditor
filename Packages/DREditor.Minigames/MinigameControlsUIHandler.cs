//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using DREditor.UI.UIControls;
using UnityEngine;

namespace DREditor.Minigames
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(CallableControlledUI) + ", " + nameof(CallableUIBase) +
        "\n\n")]
    public class MinigameControlsUIHandler : CallableControlledUI
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected StringWithEvent currentControlsKey = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string minigameControlsKey = "";


        /// <summary>
        ///
        /// </summary>
        protected TrialTimerBase trialTimer;

        /// <summary>
        ///
        /// </summary>
        protected bool timedOut = false;


        protected virtual void Awake() => trialTimer = transform.root.GetComponentInChildren<TrialTimerBase>();

        protected virtual void Start() => currentControlsKey.SetValue(minigameControlsKey);

        /// <inheritdoc/>
        protected override void OnDisable()
        {
            base.OnDisable();

            UnsubscribeTimeUp();
        }

        
        /// <inheritdoc/>
        protected override void OpenControlsMenu()
        {
            SubscribeTimeUp();

            base.OpenControlsMenu();
        }

        /// <inheritdoc/>
        protected override void OnUIClosed()
        {
            if (!timedOut)
                base.OnUIClosed();
            else
                timedOut = false;

            UnsubscribeTimeUp();
        }


        /// <summary>
        ///
        /// </summary>
        protected virtual void SubscribeTimeUp()
        {
            if (trialTimer != null)
                trialTimer.TimeUp += OnTimeUp;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void UnsubscribeTimeUp()
        {
            if (trialTimer != null)
                trialTimer.TimeUp -= OnTimeUp;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnTimeUp() => timedOut = true;
    }
}