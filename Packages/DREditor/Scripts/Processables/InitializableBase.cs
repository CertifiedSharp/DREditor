//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor.
using UnityEngine;

namespace DREditor.Processables
{
    /// <summary>
    /// Simple abstract class implementing <see cref="IInitializable"/> to set up initializing on awake and initialization verification.
    /// </summary>
    public abstract class InitializableBase : MonoBehaviour, IInitializable
    {
        /// <summary>
        /// Whether this script should attempt to initialize during the Unity Awake function.
        /// </summary>
        [Tooltip("Whether this script should attempt to initialize during the Unity Awake function.")]
        [SerializeField] protected bool initializeOnAwake = false;


        /// <summary>
        /// Whether this script has already been through or started it's initialization process.
        /// </summary>
        protected bool initialized = false;


        protected virtual void Awake()
        {
            if (ShouldInitialize())
                Initialize();
        }

        /// <summary>
        /// Checks whether this script should call it's Initialization process.
        /// </summary>
        /// <returns>True if this script should call it's Initialization process.</returns>
        protected virtual bool ShouldInitialize() => initializeOnAwake;

        /// <inheritdoc/>
        public virtual void Initialize()
        {
            if (!AlreadyInitialized())
                ConfirmApplyInitialization();
        }

        /// <summary>
        /// Sets this process as initialized and calls it's initialization process.
        /// </summary>
        private void ConfirmApplyInitialization()
        {
            SetInitialized();

            Init();
        }

        /// <summary>
        /// Checks whether this script has already been initialized.
        /// </summary>
        /// <returns>True if this script has already been initialized.</returns>
        public virtual bool AlreadyInitialized() => initialized;

        /// <summary>
        /// Sets this script as initialized.
        /// </summary>
        protected virtual void SetInitialized() => initialized = true;

        /// <summary>
        /// The pre-verified initialization process for this script.
        /// </summary>
        protected abstract void Init();
    }
}