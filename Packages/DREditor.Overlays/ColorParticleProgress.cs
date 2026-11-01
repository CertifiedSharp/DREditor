//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using DREditor.LocalDialogues;
using DREditor.Services;
using UnityEngine;

namespace DREditor.Overlays
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    ///
    /// </remarks>
    [ClassHeader(nameof(ParticleProgress) + ", " + nameof(GameplayOverlay) + ", " + 
        nameof(IOverlayManager) + ", " + nameof(IToTitleScreen) + 
        "\n\n")]
    [DefaultExecutionOrder(1)]
    public class ColorParticleProgress : ParticleProgress
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected GameObject particlePrefab = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected Color allTriggeredColor = Color.blue;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Color untriggeredRequiredColor = Color.yellow;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Color untriggeredOptionalColor = Color.green;


        /// <inheritdoc/>
        protected override void ApplyParticleToObject(SelectableObject selectableObject)
        {
            if (selectableObject is not IDialogueProgression progressable)
                return;

            Color spawnColor = progressable.AllDialogueTriggered() ? allTriggeredColor :
                progressable.HasUnTriggeredRequiredFlags() ? untriggeredRequiredColor :
                untriggeredOptionalColor;

            SpawnProgressParticle(selectableObject.gameObject, spawnColor);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="targetObject"></param>
        /// <param name="particleColor"></param>
        protected virtual void SpawnProgressParticle(GameObject targetObject, Color particleColor)
        {
            GameObject particleGameObject = Instantiate(particlePrefab, targetObject.transform);

            InitializeParticleDimensions(targetObject, particleGameObject);

            ParticleSystem particleSystem = particleGameObject.GetComponent<ParticleSystem>();

            var particleModule = particleSystem.main;

            particleModule.startColor = particleColor;

            particles.Add(particleSystem);

            if (targetObject.GetComponent<ParticleProgressOffset>() is ParticleProgressOffset off)
                off.ApplyOffsets(particleSystem);

            particleSystem.Play();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="targetObject"></param>
        /// <param name="particleGameObject"></param>
        protected virtual void InitializeParticleDimensions(GameObject targetObject, GameObject particleGameObject)
        {
            Transform parentTransform = targetObject.transform;

            particleGameObject.transform.localPosition = Vector3.zero;

            particleGameObject.transform.localEulerAngles = parentTransform.localEulerAngles;

            if (parentTransform.TryGetComponent(out BoxCollider boxCollider))
            {
                particleGameObject.transform.localPosition = boxCollider.center;

                particleGameObject.transform.localScale = boxCollider.size;
            }
            else if (parentTransform.TryGetComponent(out MeshCollider collider))
            {
                particleGameObject.transform.localPosition = collider.sharedMesh.bounds.center;

                particleGameObject.transform.localScale = collider.sharedMesh.bounds.size;
            }
            else if (parentTransform.TryGetComponent(out MeshFilter parentRenderer))
                particleGameObject.transform.localScale = parentRenderer.mesh.bounds.size;
        }
    }
}