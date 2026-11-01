//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.LineReading;
using DREditor.EventObjects;
using UnityEngine;

namespace DREditor.Actors.Implementation
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    ///
    /// </remarks>
    [ClassHeader(nameof(DialogueComponent) + ", " + nameof(ILineProcessor) +
        "\n\n")]
    public class ActorTrialSpriteChanger : DialogueComponent, ILineProcessor
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SelectableObjectWithEvent currentActor = null;


        /// <inheritdoc/>
        public virtual void LineProcess(Line currentLine)
        {
            if (currentActor.Value is SelectableObject actor &&
                currentLine.Expression != null && currentLine.Expression.Sprite != null)
                SetTrialExpression(actor, currentLine.Expression.Sprite.mainTexture);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="actor"></param>
        /// <param name="texture"></param>
        protected virtual void SetTrialExpression(SelectableObject actor, Texture texture)
        {
            var meshRenderers = actor.GetComponentsInChildren<MeshRenderer>();

            foreach(MeshRenderer meshRenderer in meshRenderers)
                meshRenderer.material.mainTexture = texture;
        }
    }
}