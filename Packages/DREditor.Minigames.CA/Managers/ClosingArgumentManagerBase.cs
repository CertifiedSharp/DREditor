// Original Author: Henry (Meowpixel)
// Updated/Reworked by: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using DREditor.Fading;
using DREditor.Services;
using System.Collections;
using UnityEngine;

namespace DREditor.Minigames.CA.Managers
{
    /// <summary>
    ///
    /// </summary>
    public abstract class ClosingArgumentManagerBase : MinigameManagerBase
    {
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
        [SerializeField] protected bool debugTransition = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected ArgumentBuilder builder;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected ArgumentInitializerBase initializer = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float fadeToBlackDuration = 1;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inputAllowed = null;


        /// <summary>
        ///
        /// </summary>
        protected int totalStock = 0;


        protected virtual void Start()
        {
            if (debug)
                Initialize(builder);

            if (debugTransition)
                PlayMinigame(builder);
        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="obj"></param>
        /// <remarks></remarks>
        public void PlayMinigame(ScriptableObject obj)
            => FindAnyObjectByType<ClosingArgumentManagerBase>().PlayClosingArgument((ArgumentBuilder)obj);

        /// <summary>
        ///
        /// </summary>
        /// <param name="builder"></param>
        protected virtual void PlayClosingArgument(ArgumentBuilder builder)
            => StartCoroutine(ArgumentStartRoutine(builder));


        /// <summary>
        ///
        /// </summary>
        /// <param name="builder"></param>
        protected virtual IEnumerator ArgumentStartRoutine(ArgumentBuilder builder)
        {
            if (ServiceLocator.HasService<GlobalFade>())
                yield return ServiceLocator.GetService<GlobalFade>().FadeToRoutine(fadeToBlackDuration);

            Initialize(builder);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="builder"></param>
        protected virtual void Initialize(ArgumentBuilder builder)
        {
            this.builder = builder;

            TrialTimer.Initialize(builder.times);

            initializer.InitializeArgument(builder);

            totalStock = builder.GetTotalStock();
        }


        /// <inheritdoc/>
        protected override void OnPlayerDeath()
        {
            base.OnPlayerDeath();

            StopGame();
        }

        /// <inheritdoc/>
        protected override void OnTimeUp()
        {
            base.OnTimeUp();

            StopGame();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void StopGame()
        {
            TrialTimer.StopTimer();

            inputAllowed.SetValue(false);
        }
    }
}