//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using System.Collections;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DREditor.Minigames.Debates.Counters
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(CounterUIType) + ", " + nameof(CounterType) +
        "\n\n")]
    public class CounterConsent : CounterUIType
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected TMP_ColorGradient consentGradient = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Image consentImage = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Sprite[] consentTex;


        /// <summary>
        ///
        /// </summary>
        protected PhraseTextBase[] phraseTexts;

        /// <summary>
        ///
        /// </summary>
        protected TMP_Text[] texts;


        /// <inheritdoc/>
        protected override void Awake()
        {
            base.Awake();

            phraseTexts = FindObjectsByType<PhraseTextBase>();

            texts = phraseTexts.Select(phraseText => phraseText.GetComponentInChildren<TMP_Text>()).ToArray();
        }

        /// <inheritdoc/>
        public override IEnumerator DisplayCounter(PhraseBase textPhrase, Line currentPanel)
        {
            consentImage.sprite = consentTex[currentPanel.SpeakerNumber];

            consentImage.enabled = true;

            yield return base.DisplayCounter(textPhrase, currentPanel);
        }

        /// <inheritdoc/>
        public override bool Evaluate(PhraseBase textPhrase)
        {
            PhraseTextBase selectedText = phraseTexts.Where(phraseText => phraseText.textPhrase == textPhrase).FirstOrDefault();

            return selectedText != null && HasGradient(selectedText.GetComponentInChildren<TMP_Text>());
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        protected virtual bool HasGradient(TMP_Text text) =>
            text != null &&
            (text.colorGradientPreset == consentGradient || text.text.Contains("<gradient=" + consentGradient.name));
    }
}