//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.LineReading;
using DREditor.EventObjects;
using UnityEngine;

namespace DREditor.Nameplates
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(ILineProcessor) + ", " + nameof(ITransparentableNameplate) + ", " + nameof(ISceneEventListener) +
        "\n\n")]
    public class DialogueNameplateHandler : NameplateHandler, ITransparentableNameplate, ISceneEventListener
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected Texture2D transparentImage = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEvent allNameplatesTransparent = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected bool setNameTransparentIfTutorial = true;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string tutorialCharacterFirstName = "Tutorial";


        /// <summary>
        ///
        /// </summary>
        protected virtual void OnEnable()
        {
            if (allNameplatesTransparent != null)
                allNameplatesTransparent.RegisterListener(this);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDisable()
        {
            if (allNameplatesTransparent != null)
                allNameplatesTransparent.UnregisterListener(this);
        }

        /// <inheritdoc/>
        public override void EvaluateNameplate(Line currentLine)
        {
            bool tutorialIsLineSpeaker = currentLine.Speaker == null 
                || currentLine.Speaker.FirstName.Contains(tutorialCharacterFirstName);

            if (setNameTransparentIfTutorial && tutorialIsLineSpeaker)
                SetNameTransparent();
            else
                base.EvaluateNameplate(currentLine);
        }

        /// <inheritdoc/>
        public virtual void OnEventRaised() => SetNameTransparent();

        public virtual void SetNameTransparent() => SetTexture(transparentImage);
    }
}