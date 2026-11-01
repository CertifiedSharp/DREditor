//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

namespace DREditor.Minigames.Debates.Managers
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(PhraseManagerBase) + ", " + nameof(IPhraseManager) +
        "\n\n")]
    public class PhraseManager : PhraseManagerBase
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool logPanelDuration = false;


        /// <summary>
        ///
        /// </summary>
        protected IBulletEvaluatorManager bulletEvaluator;


        protected virtual void Awake() => bulletEvaluator = transform.root.GetComponentInChildren<IBulletEvaluatorManager>();

        protected virtual void OnEnable() => InitializePhraseTexts();

        protected virtual void OnDisable() => ResolvePhraseTexts();

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializePhraseTexts()
        {
            foreach (PhraseTextBase phraseText in phraseTexts)
                if (phraseText is IPhraseHittable hittable)
                    hittable.OnHitEvaluate.AddListener(bulletEvaluator.EvaluateBulletHit);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ResolvePhraseTexts()
        {
            foreach (PhraseTextBase phraseText in phraseTexts)
                if (phraseText is IPhraseHittable hittable)
                    hittable.OnHitEvaluate.RemoveListener(bulletEvaluator.EvaluateBulletHit);
        }

        /// <inheritdoc/>
        public override float PlayPhrases(Panel panel)
        {
            activePhrases.Clear();

            float panelLength = 0;

            float clipLength = 0;

            for (int i = 0; i < panel.Phrases.Count; i++)
            {
                if (panel.Phrases[i] is Phrase currentPhrase)
                {
                    float duration = phraseTexts[i].Initialize(currentPhrase);

                    OnPhraseInitialized?.Invoke(panel, phraseTexts[i], duration);

                    if (i == 0 || clipLength < duration)
                        panelLength = duration;

                    clipLength = duration;

                    activePhrases.Add(phraseTexts[i]);

                    StartCoroutine(RemovePhraseWhenExpired(phraseTexts[i], clipLength));
                }
            }

            if (logPanelDuration)
            {
                string additionalWaitTime = panel.WaitTime > 0 ? " with additional wait time: " + panel.WaitTime : "";

                UnityEngine.Debug.Log("REPORT: Panel Length is: " + panelLength + additionalWaitTime);
            }

            return panelLength + panel.WaitTime;
        }
    }
}