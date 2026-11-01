//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.Actors
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    ///
    /// </remarks>
    [ClassHeader(nameof(LayerHandler) +
        "\n\n")]
    public class BasicLayerHandler : LayerHandler
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected List<Renderer> unfocusRenderers = new();

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<Renderer> focusRenderers = new();


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected int unfocusedLayer = 6;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int focusedLayer = 8;


        /// <inheritdoc/>
        public override void ToggleRender()
        {
            if (focusRenderers.Count == 0 || focusRenderers[0] == null)
                return;

            EnableRenderers(unfocusRenderers, focusRenderers[0].enabled);

            EnableRenderers(focusRenderers, !focusRenderers[0].enabled);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="renderers"></param>
        /// <param name="to"></param>
        protected virtual void EnableRenderers<T>(List<T> renderers, bool to) where T : Renderer
        {
            foreach (var renderer in renderers)
                if (renderer != null)
                    renderer.enabled = to;
        }


        /// <inheritdoc/>
        public override void CheckSpriteLayer()
        {
            if (focusRenderers.Count != 0 && focusRenderers[0] != null && focusRenderers[0].gameObject.layer != focusedLayer)
                FocusActor();
        }

        /// <inheritdoc/>
        public override void FocusActor() => SetActorLayers(focusedLayer);

        /// <inheritdoc/>
        public override void UnFocusActor() => SetActorLayers(unfocusedLayer);

        /// <summary>
        ///
        /// </summary>
        /// <param name="layer"></param>
        protected virtual void SetActorLayers(int layer)
        {
            SetRenderersLayer(unfocusRenderers, layer);

            SetRenderersLayer(focusRenderers, layer);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="renderers"></param>
        /// <param name="layer"></param>
        protected virtual void SetRenderersLayer<T>(List<T> renderers, int layer) where T : Renderer
        {
            foreach (var renderer in renderers)
                if (renderer != null)
                    renderer.gameObject.layer = layer;
        }
    }
}