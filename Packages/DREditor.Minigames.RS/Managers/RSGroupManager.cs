//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.EventObjects;
using DREditor.Minigames.Debates;
using DREditor.Minigames.Debates.Managers;
using DREditor.Minigames.Debates.PhraseHandling;
using DREditor.Minigames.Stats;
using DREditor.Progressing;
using DREditor.Services;
using DREditor.UIHandling;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DREditor.Minigames.RS.Managers
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DebateGroupManagerBase<RSGroup>) + ", " + nameof(ServiceHolder<IDebateGroupManager>) +
        ", " + nameof(IDebateGroupManager) +
        "\n\n")]
    public class RSGroupManager : DebateGroupManagerBase<RSGroup>
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inDialogue = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected IntWithEvent cutCount = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEvent onAdvance = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEvent onRetreat = null; // Used by Enemy Win Mechanic.


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected UIVisibilityComponent advanceAnimation = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected UIVisibilityComponent retreatAnimation = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected float startOfSectionWaitTime = 0.59f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float damageDivisor = 3;


        /// <summary>
        ///
        /// </summary>
        protected int groupResult;

        /// <summary>
        ///
        /// </summary>
        protected TrialTimerBase trialTimer;

        /// <summary>
        ///
        /// </summary>
        protected IEnumerable<IPhraseShatterable> shatterablePhrases;

        /// <summary>
        ///
        /// </summary>
        protected IEnumerable<IPhraseManager> phraseManagers;

        /// <summary>
        ///
        /// </summary>
        protected IDebateLineManager lineManager = null;

        /// <summary>
        ///
        /// </summary>
        protected IPhraseResetManager resetManager;

        /// <summary>
        ///
        /// </summary>
        protected ISplitScreen splitScreenHandler = null;

        /// <summary>
        ///
        /// </summary>
        protected ITieBreaker tieBreaker;


        /// <inheritdoc/>
        protected override void Awake()
        {
            base.Awake();

            var allComponents = transform.root.GetComponentsInChildren<Component>();

            resetManager = allComponents.OfType<IPhraseResetManager>().FirstOrDefault();

            lineManager = allComponents.OfType<IDebateLineManager>().FirstOrDefault();

            trialTimer = allComponents.OfType<TrialTimerBase>().FirstOrDefault();

            phraseManagers = allComponents.OfType<IPhraseManager>();

            shatterablePhrases = allComponents.OfType<IPhraseShatterable>();
        }

        protected virtual void OnEnable()
        {
            foreach (var phrase in shatterablePhrases)
                phrase?.OnShattered.AddListener(OnPhraseShattered);

            foreach (var manager in phraseManagers)
                manager?.OnPhraseInitialized.AddListener(EnemyAttackOnPhraseEnd);
        }

        protected virtual void OnDisable()
        {
            foreach (var phrase in shatterablePhrases)
                phrase?.OnShattered.RemoveListener(OnPhraseShattered);

            foreach (var manager in phraseManagers)
                manager?.OnPhraseInitialized.RemoveListener(EnemyAttackOnPhraseEnd);
        }

        protected virtual void Start()
        {
            if (ServiceLocator.HasService<ISplitScreen>())
                splitScreenHandler = ServiceLocator.GetService<ISplitScreen>();

            if (ServiceLocator.HasService<ITieBreaker>())
                tieBreaker = ServiceLocator.GetService<ITieBreaker>();

            resetManager?.ResetPhrases();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="phraseBase"></param>
        protected virtual void OnPhraseShattered(IPhraseShatterable phraseBase)
        {
            if (!((MonoBehaviour)phraseBase).TryGetComponent<PhraseTextBase>(out var phrase))
                return;

            if (phrase.textPhrase != null && !phrase.textPhrase.Has<HittableModifierBase>())
                splitScreenHandler?.PlayerAttack(currentGroup.GetSectionHitablePhraseCount());
            else
                splitScreenHandler?.EnemyAttack(GetEnemyNumber(), CalculateDamage());
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual int GetEnemyNumber()
        {
            if (debate != null && lineManager != null && lineManager.GetCurrentPanel() != null)
                return debate.PanelCharacterNumbers.IndexOf(lineManager.GetCurrentPanel().SpeakerNumber);
            else
                return 0;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual float CalculateDamage() => currentGroup.GetSectionHitablePhraseCount() / damageDivisor;


        /// <summary>
        ///
        /// </summary>
        /// <param name="panel"></param>
        /// <param name="phraseText"></param>
        /// <param name="clipLength"></param>
        protected virtual void EnemyAttackOnPhraseEnd(Panel panel, PhraseTextBase phraseText, float clipLength)
            => StartCoroutine(OnPhraseAnimated(panel, phraseText, clipLength));

        /// <summary>
        ///
        /// </summary>
        /// <param name="panel"></param>
        /// <param name="phraseText"></param>
        /// <param name="clipLength"></param>
        protected virtual IEnumerator OnPhraseAnimated(Panel panel, PhraseTextBase phraseText, float clipLength)
        {
            if (AnimationInvalid(phraseText))
                yield break;

            yield return new WaitForSeconds(clipLength);

            // If there is ever a scenario where damage is still making it through at the end of phrases.
            // This first line was the fix for it, so if damage is still leaking through then remove this code and try something else.
            if (phraseText.textPhrase is IPhraseHealth health && health.GetHealth() == 0 ||
                phraseText.GetComponent<IPhraseShatterable>() is not IPhraseShatterable shatter ||
                splitScreenHandler == null)
                yield break;

            bool inDialogue = this.inDialogue != null && this.inDialogue.Value;

            if (!inDialogue &&
                !shatter.IsShattering() &&
                !phraseText.textPhrase.Has<HittableModifierBase>() &&
                debate.PanelCharacterNumbers.Contains(panel.SpeakerNumber))
            {
                int enemyIndex = debate.PanelCharacterNumbers.IndexOf(panel.SpeakerNumber);

                float damage = currentGroup.GetSectionHitablePhraseCount();

                splitScreenHandler?.EnemyAttack(enemyIndex, damage);
            }

            yield break;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="phraseText"></param>
        /// <returns></returns>
        protected virtual bool AnimationInvalid(PhraseTextBase phraseText)
        {
            bool missingData = debate == null || currentGroup == null;

            bool phraseInvalid = phraseText == null || phraseText.textPhrase is not Phrase;

            return missingData || phraseInvalid;
        }


        /// <inheritdoc/>
        protected override IEnumerator PlayValidGroups()
        {
            splitScreenHandler?.AnimateToResetState();

            splitScreenHandler?.InitializePanelCount(currentGroup.GetSectionHitablePhraseCount());

            yield return new WaitForSeconds(startOfSectionWaitTime);

            if (ServiceLocator.HasService<TrialStatsBase>())
                ServiceLocator.GetService<TrialStatsBase>().ShowTrialStats();

            trialTimer.ResumeTimer();

            if (cutCount != null)
                cutCount.SetValue(currentGroup.CutCount);

            yield return lineManager.PlayLines(currentGroup.GetLines<Panel>());

            trialTimer.PauseTimer();

            if (splitScreenHandler != null)
                yield return EvaluateSectionResult();
            else
                yield return Stall();
        }

        /// <inheritdoc/>
        protected override IEnumerator EvaluateSectionResult()
        {
            groupResult = splitScreenHandler.CalculateDomination();

            UnityEngine.Debug.Log("REPORT: Section Result is: " + groupResult);

            bool tieResult = groupResult == tieNumber;

            if (tieResult)
                yield return tieBreaker?.TieBreakerProcess((tieBreakerResult) =>
                {
                    groupResult = tieBreakerResult ? winNumber : loseNumber;
                });

            bool winResult = groupResult == winNumber;

            bool loseResult = groupResult == loseNumber;

            bool stallResult = groupResult == stallNumber;

            bool didntLoseOnFinal = !loseResult && currentGroupIndex == debateLineCount - 1;

            bool missingBreakerOnTie = tieResult && tieBreaker == null;

            bool missingResponseWin = winResult && currentGroup.GetResponsePanels<Panel>().Count == 0;

            if (stallResult || didntLoseOnFinal || missingBreakerOnTie || missingResponseWin)
                yield return Stall();
            else if (winResult)
                yield return Advance();
            else if (loseResult)
                yield return Retreat();
        }

        /// <inheritdoc/>
        protected override IEnumerator Stall()
        {
            onPanelsResolved?.Invoke();

            splitScreenHandler?.AnimateToResetState();

            ServiceLocator.GetService<IDialogueManager>().StartDialogue(null, currentGroup.EndGroupDialogue);

            yield break;
        }

        /// <inheritdoc/>
        protected override IEnumerator Advance()
        {
            onPanelsResolved?.Invoke();

            if (onAdvance != null)
                onAdvance.Raise();

            if (advanceAnimation != null)
                yield return advanceAnimation.ShowTillComplete();

            yield return lineManager.PlayLines(currentGroup.GetResponsePanels<Panel>());

            currentGroupIndex++;

            onRestartPanels?.Invoke();
        }

        /// <inheritdoc/>
        protected override IEnumerator Retreat()
        {
            if (onRetreat != null)
                onRetreat.Raise();
            
            if (currentGroupIndex != 0 && retreatAnimation != null)
                yield return retreatAnimation.ShowTillComplete();

            if (currentGroupIndex != 0)
                currentGroupIndex--;

            splitScreenHandler?.AnimateToResetState();

            var retreatDialogue = currentGroup.RetreatDialogue != null ?
                currentGroup.RetreatDialogue : debate?.DefaultRetreatDialogue;

            if (retreatDialogue != null)
                onStopPanels?.Invoke(retreatDialogue);
        }
    }
}