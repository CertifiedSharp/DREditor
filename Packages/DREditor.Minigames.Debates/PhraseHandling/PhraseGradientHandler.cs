//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

namespace DREditor.Minigames.Debates.PhraseHandling
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(PhraseTextComponentBase) + ", " + nameof(IPhraseHandlerComponent) + ", " +
        nameof(IGradientOverride) + ", " + nameof(IPhraseResetter) +
        "\n\n")]
    public class PhraseGradientHandler : PhraseTextComponentBase, IGradientOverride, IPhraseResetter
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string hitGradientName = "\"HitText\"";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string consentGradientName = "\"ConsentText\"";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected ScriptableObject defaultInput = null;


        /// <summary>
        ///
        /// </summary>
        protected PhraseBase phrase;

        /// <summary>
        ///
        /// </summary>
        protected PartlyHitModifier hitModifier;

        /// <summary>
        ///
        /// </summary>
        protected string gradientName = "";

        /// <summary>
        ///
        /// </summary>
        protected string gradientOverride = "";


        /// <inheritdoc/>
        public override void StartPhrase<T>(T phrase)
        {
            InitializePhrase(phrase);

            UpdateGradientName();

            EvaluateGradient();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="phrase"></param>
        protected virtual void InitializePhrase(PhraseBase phrase)
        {
            this.phrase = phrase;

            hitModifier = phrase.Get<PartlyHitModifier>();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void UpdateGradientName()
        {
            gradientName = phrase.TryGetModifier<GradientOverrideModifier>(out var modifier) ?
                modifier.Gradient.name : 
                phrase.Get<InputModifier>()?.Contains(defaultInput) == true?
                hitGradientName : "";

            if (gradientOverride != "")
            {
                gradientName = gradientOverride;

                tmpText.text = "<gradient=" + gradientName + ">" + tmpText.text + "</gradient>";
            }
        }
        
        /// <summary>
        ///
        /// </summary>
        protected virtual void EvaluateGradient()
        {
            if (gradientOverride != "" || gradientName != "")
                ApplyGradient();
            else
                RemoveGradient();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ApplyGradient()
        {
            if (hitModifier != null)
                ApplyPartlyGradient();
            else
                ApplyAllTextGradient();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ApplyPartlyGradient()
        {
            string[] arr = tmpText.text.Split(' ');

            arr[hitModifier.WordStart] = "<gradient=" + gradientName + ">" + arr[hitModifier.WordStart];

            arr[hitModifier.WordStart + hitModifier.WordCount - 1] += "</gradient>";

            tmpText.text = string.Join(" ", arr);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ApplyAllTextGradient()
            => tmpText.text = "<gradient=" + gradientName + ">" + tmpText.text + "</gradient>";

        /// <summary>
        ///
        /// </summary>
        protected virtual void RemoveGradient() => tmpText.colorGradientPreset = null;


        /// <inheritdoc/>
        public virtual void SetGradientTextOverride(string gradient) => gradientOverride = gradient;

        /// <inheritdoc/>
        public virtual void Reset() => SetGradientTextOverride("");
    }
}