//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

namespace DREditor.Overlays
{
    /// <summary>
    ///
    /// </summary>
	[ClassHeader("")]
    public class ParticleProgressOffset : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected bool debugMode = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected ParticleSystem particle = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected bool overrideValues = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Vector3 offsetPosition = Vector3.zero;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Vector3 offsetScale = Vector3.zero;


        /// <summary>
        ///
        /// </summary>
        /// <param name="particle"></param>
        public virtual void ApplyOffsets(ParticleSystem particle)
        {
            if (overrideValues)
                SetOffsets(particle);
            else
                AddOffsets(particle);

            this.particle = particle;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="particle"></param>
        protected virtual void SetOffsets(ParticleSystem particle)
        {
            particle.gameObject.transform.localPosition = LocalizeVectors(offsetPosition);

            particle.gameObject.transform.localScale = LocalizeVectors(offsetScale);
        }
        
        /// <summary>
        ///
        /// </summary>
        /// <param name="particle"></param>
        protected virtual void AddOffsets(ParticleSystem particle)
        {
            particle.gameObject.transform.localPosition += LocalizeVectors(offsetPosition);

            particle.gameObject.transform.localScale += LocalizeVectors(offsetScale);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="vector"></param>
        /// <returns></returns>
        protected virtual Vector3 LocalizeVectors(Vector3 vector)
        {
            return new Vector3(
                vector.x / transform.localScale.x,
                vector.y / transform.localScale.y,
                vector.z / transform.localScale.z
            );
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void Update()
        {
            if (debugMode && particle != null)
                SetOffsets(particle);
        }
    }
}