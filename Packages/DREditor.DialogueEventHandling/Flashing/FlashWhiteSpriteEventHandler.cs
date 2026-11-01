//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Kill/Cure
using DREditor.EventObjects;
using UnityEngine;

namespace DREditor.DialogueEventHandling.Flashing
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DialogueEventHandler) + ", " + nameof(FlashEventHandlerBase) + ", " + nameof(FlashEventSimpleHandlerBase) +
        "\n\n")]
    public class FlashWhiteSpriteEventHandler : FlashEventSimpleHandlerBase
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SelectableObjectWithEvent currentDialogueObject = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Material flashMaterial = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string shaderStringTextureRef = "_MainTex";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string shaderStringWhiteFloatRef = "_White";


        /// <summary>
        ///
        /// </summary>
        protected SpriteRenderer spriteRenderer;

        /// <summary>
        ///
        /// </summary>
        protected Material originalMaterial = null;


        /// <inheritdoc/>
        protected override void DisplayFlashEvent()
        {
            InitializeMaterial();

            base.DisplayFlashEvent();
        }

        /// <inheritdoc/>
        protected override bool FailFlash()
        {
            bool result = currentDialogueObject.Value == null;

            if (result)
                UnityEngine.Debug.LogWarning("ALERT: Failed flash because the current dialogue object is null!");

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeMaterial()
        {
            spriteRenderer = GetSpriteRendererToFlash();

            originalMaterial = spriteRenderer.material;

            flashMaterial.SetTexture(shaderStringTextureRef, originalMaterial.mainTexture);

            spriteRenderer.material = flashMaterial;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual SpriteRenderer GetSpriteRendererToFlash()
        {
            var renderers = currentDialogueObject.Value.GetComponentsInChildren<SpriteRenderer>();

            foreach (var renderer in renderers)
                if (renderer.transform.childCount != 0)
                    return renderer;

            return null;
        }

        /// <inheritdoc/>
        protected override void FlashUpdate(float value) => flashMaterial.SetFloat(shaderStringWhiteFloatRef, value);

        /// <inheritdoc/>
        protected override void OnDisable()
        {
            base.OnDisable();

            ResetFlash();
        }

        /// <inheritdoc/>
        protected override void ResetFlash()
        {
            ResetMaterials();

            ClearData();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ResetMaterials()
        {
            if (spriteRenderer != null && spriteRenderer.material.shader == flashMaterial.shader)
                spriteRenderer.material = originalMaterial;

            flashMaterial.SetFloat(shaderStringWhiteFloatRef, 0);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ClearData()
        {
            spriteRenderer = null;

            originalMaterial = null;
        }
    }
}