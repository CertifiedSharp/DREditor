//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.EventObjects;
using System.Linq;
using UnityEngine;

namespace DREditor.Minigames.Debates.Managers
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(IPhraseResetManager) +
        "\n\n")]
    public class PhraseResetManager : MonoBehaviour, IPhraseResetManager
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent trialTimeUp = null;


        /// <summary>
        ///
        /// </summary>
        protected IShatterGraphic shatter;

        /// <summary>
        ///
        /// </summary>
        protected IBulletEvaluatorManager bulletEvaluator;

        /// <summary>
        ///
        /// </summary>
        protected IPhraseResetter[] phraseResetters;


        protected virtual void Awake()
        {
            var allComponents = transform.root.GetComponentsInChildren<Component>();

            shatter = allComponents.OfType<IShatterGraphic>().FirstOrDefault();

            bulletEvaluator = allComponents.OfType<IBulletEvaluatorManager>().FirstOrDefault();

            phraseResetters = allComponents.OfType<IPhraseResetter>().ToArray();
        }

        protected virtual void OnEnable()
        {
            shatter.OnPostShatter.AddListener(ResetPhrases);

            bulletEvaluator.OnStopPanels += ResetPhrasesOnFuckUp;

            trialTimeUp.AddListener(ResetPhrasesOnTimeUp);
        }

        protected virtual void OnDisable()
        {
            shatter.OnPostShatter.RemoveListener(ResetPhrases);

            bulletEvaluator.OnStopPanels -= ResetPhrasesOnFuckUp;

            trialTimeUp.RemoveListener(ResetPhrasesOnTimeUp);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="timeUp"></param>
        protected virtual void ResetPhrasesOnTimeUp(bool timeUp)
        {
            if (timeUp)
                ResetPhrases();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="fuckUpDialogue"></param>
        protected virtual void ResetPhrasesOnFuckUp(DialogueCore fuckUpDialogue = null)
        {
            if (fuckUpDialogue != null)
                ResetPhrases();
        }


        /// <inheritdoc/>
        public virtual void ResetPhrases()
        {
            foreach (var resetter in phraseResetters)
                resetter.Reset();
        }
    }
}