//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using DREditor.UI;
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace DREditor.Minigames.Debates
{
    /// <summary>
    ///
    /// </summary>
	[ClassHeader("")]
    public class DebateTruthBulletMenu : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected InputActionReference bulletList = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected InputActionReference cancel = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inMenu = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected MenuGroupBase truthMenu = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inputAllowed = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inCycle = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent canFire = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent isChangingSpeed = null;


        /// <summary>
        ///
        /// </summary>
        protected TrialTimerBase trialTimer;

        /// <summary>
        ///
        /// </summary>
        protected bool timedOut = false;

        /// <summary>
        ///
        /// </summary>
        protected float initialTimeScale;

        /// <summary>
        ///
        /// </summary>
        protected float initialDOTweenTimeScale;


        protected virtual void Awake() => trialTimer = transform.root.GetComponentInChildren<TrialTimerBase>();

        protected virtual void OnEnable() => inputAllowed.AddListener(OnUpdateInput);

        protected virtual void OnDisable()
        {
            inputAllowed.RemoveListener(OnUpdateInput);

            UnsubscribeTimeUp();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="inputAllowed"></param>
        protected virtual void OnUpdateInput(bool inputAllowed)
        {
            if (inputAllowed)
                bulletList.action.started += CheckTruthMenu;
            else
                bulletList.action.started -= CheckTruthMenu;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        protected virtual void CheckTruthMenu(CallbackContext context)
        {
            if (isChangingSpeed.Value || !canFire.Value || truthMenu.isActive || inMenu.Value)
                return;

            inputAllowed.SetValue(false);

            inMenu.SetValue(true);

            InitializeSpeed();

            SetGameSpeed(0);

            truthMenu.Show();

            SubscribeTimeUp();

            StartCoroutine(WaitToLeaveMenu(() => truthMenu.isActive));
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeSpeed()
        {
            initialTimeScale = Time.timeScale;
#if DOTWEEN
            initialDOTweenTimeScale = DG.Tweening.DOTween.timeScale;
#endif
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="speed"></param>
        protected virtual void SetGameSpeed(float speed)
        {
            Time.timeScale = speed;
#if DOTWEEN
            DG.Tweening.DOTween.timeScale = speed;
#endif
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
        protected virtual void OnTimeUp() => timedOut = true;

        /// <summary>
        ///
        /// </summary>
        /// <param name="callbackContext"></param>
        protected virtual void HideMenu(CallbackContext callbackContext)
        {
            cancel.action.started -= HideMenu;

            truthMenu.Hide();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="InMenu"></param>
        protected virtual IEnumerator WaitToLeaveMenu(Func<bool> InMenu)
        {
            yield return new WaitUntil(InMenu);

            cancel.action.started += HideMenu;

            while (InMenu())
                yield return null;

            Time.timeScale = initialTimeScale;

            RestoreSpeed();

            inMenu.SetValue(false);

            EvaluateRestoreInput();

            UnsubscribeTimeUp();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void RestoreSpeed()
        {
            Time.timeScale = initialTimeScale;
#if DOTWEEN
            DG.Tweening.DOTween.timeScale = initialDOTweenTimeScale;
#endif
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void EvaluateRestoreInput()
        {
            if (!timedOut)
                inputAllowed.SetValue(true);
            else
                timedOut = false;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void UnsubscribeTimeUp()
        {
            if (trialTimer != null)
                trialTimer.TimeUp -= OnTimeUp;
        }
    }
}