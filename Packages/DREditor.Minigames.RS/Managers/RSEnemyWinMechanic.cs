//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using DREditor.Fading;
using DREditor.Minigames.Stats;
using DREditor.Services;
using System.Collections;
using UnityEngine;

namespace DREditor.Minigames.RS.Managers
{
    /// <summary>
    ///
    /// </summary>
	[ClassHeader("")]
    public class RSEnemyWinMechanic : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEventListener advanceListener;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEventListener retreatListener;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected GameObject interjectAnimationObject;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected InterjectHandlerBase enemyInterject;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float fadeOutDuration = 0.5f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int targetAdvance = 0;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int targetRetreat = 0;


        /// <summary>
        ///
        /// </summary>
        protected int advanced = 0;

        /// <summary>
        ///
        /// </summary>
        protected int retreated = 0;

        /// <summary>
        ///
        /// </summary>
        protected MinigameManagerBase minigameManager;

        /// <summary>
        ///
        /// </summary>
        protected IShatterGraphic shatter;


        protected virtual void OnEnable()
        {
            advanceListener.Response.AddListener(OnAdvance);

            retreatListener.Response.AddListener(OnRetreat);
        }

        protected virtual void OnDisable()
        {
            advanceListener.Response.RemoveListener(OnAdvance);

            retreatListener.Response.RemoveListener(OnRetreat);
        }

        protected virtual void Start()
        {
            minigameManager = transform.root.GetComponent<MinigameManagerBase>();

            shatter = GetComponentInChildren<IShatterGraphic>();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnAdvance()
        {
            advanced++;

            if (retreated != 0)
                retreated--;

            CheckIfEnemyWin();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnRetreat()
        {
            retreated++;

            if (advanced != 0)
                advanced--;

            CheckIfEnemyWin();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void CheckIfEnemyWin()
        {
            if (advanced == targetAdvance && retreated == targetRetreat)
                EnemyCounter();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void EnemyCounter()
        {
            if (minigameManager != null)
                minigameManager.StopAllCoroutines();

            StartCoroutine(EnemyCounterProcess());
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator EnemyCounterProcess()
        {
            shatter?.OnPostShatter.AddListener(InvokeResolveMinigame);

            ServiceLocator.GetService<TrialStatsBase>().HideTrialStats();

            if (enemyInterject != null)
                yield return enemyInterject.InterjectionProcess(interjectAnimationObject);

            yield break;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InvokeResolveMinigame()
        {
            shatter?.OnPostShatter.RemoveListener(InvokeResolveMinigame);

            if (minigameManager == null)
                return;

            ServiceLocator.GetService<GlobalFade>().FadeTo(fadeOutDuration);

            StartCoroutine(minigameManager.ResolveMinigame());
        }
    }
}