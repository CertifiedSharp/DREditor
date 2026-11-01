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
    [ClassHeader(nameof(DialogueComponent) + ", " + nameof(ILineProcessor) +
        "\n\n")]
    public class NameplateHandler : DialogueComponent, ILineProcessor
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected TextureWithEvent textureEvent = null;


        /// <summary>
        ///
        /// </summary>
        protected virtual void Start() => CheckIfNameplateValid();

        /// <summary>
        ///
        /// </summary>
        protected virtual void CheckIfNameplateValid()
        {
            if (textureEvent == null)
                UnityEngine.Debug.LogError("Nameplate Handler on GameObject: " 
                    + gameObject.name + " does not have it's graphic assigned!");
        }


        /// <inheritdoc/>
        public virtual void LineProcess(Line currentLine) => EvaluateNameplate(currentLine);

        /// <summary>
        ///
        /// </summary>
        /// <param name="currentLine"></param>
        public virtual void EvaluateNameplate(Line currentLine) => SetNameplate(currentLine);

        /// <summary>
        ///
        /// </summary>
        /// <param name="currentLine"></param>
        public virtual void SetNameplate(Line currentLine)
        {
            if (currentLine == null || currentLine.Speaker == null || textureEvent == null)
                return;

            if (currentLine.AliasNumber != 0)
                SetTexture(currentLine.Speaker.Aliases[currentLine.AliasNumber - 1].Nameplate);
            else if (currentLine.Speaker.Nameplate != null)
                SetTexture(currentLine.Speaker.Nameplate);
            else
                UnityEngine.Debug.LogWarning($"{ToString()}: failed to evaluate nameplate");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="texture"></param>
        protected virtual void SetTexture(Texture2D texture)
        {
            if (textureEvent.Value == texture)
                textureEvent.Invoke();
            else
                textureEvent.SetValue(texture);
        }
    }
}