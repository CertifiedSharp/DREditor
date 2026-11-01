//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.LineReading;
using DREditor.Dialogues.Loading;
using DREditor.EventObjects;
using UnityEngine;

namespace DREditor.DialogueMechanics
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DialogueComponent) + ", " + nameof(ILineProcessor) + ", " + nameof(ILoadToLineable) +
        "\n\n")]
    public class ProtagHandler : DialogueComponent, ILineProcessor, ILoadToLineable
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent isProtagSpeaking = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected StringWithEvent protagFirstName = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string tutorialCharacterFirstName = "Tutorial";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string startingProtagFirstName = "";


        /// <summary>
        ///
        /// </summary>
        protected virtual void Start()
        {
            if (!string.IsNullOrEmpty(startingProtagFirstName))
                SetProtagFirstName(startingProtagFirstName);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="firstName"></param>
        protected virtual void SetProtagFirstName(string firstName) => protagFirstName.SetValue(firstName);

        /// <summary>
        ///
        /// </summary>
        /// <param name="currentLine"></param>
        protected virtual void EvaluateIfProtagSpeaking(Line currentLine)
        {
            if (currentLine.Speaker == null)
                return;

            bool speakerIsTutorial = currentLine.Speaker.FirstName.Equals(tutorialCharacterFirstName);

            bool speakerIsProtagFirstName = currentLine.Speaker.FirstName.Equals(protagFirstName.Value);

            bool protagIsSpeaking = speakerIsProtagFirstName || speakerIsTutorial;

            isProtagSpeaking.SetValue(protagIsSpeaking);
        }


        /// <inheritdoc/>
        public virtual void LineProcess(Line currentLine) => EvaluateIfProtagSpeaking(currentLine);

        /// <inheritdoc/>
        public virtual void OnLoadToLine(Line currentLine) => LineProcess(currentLine);
    }
}