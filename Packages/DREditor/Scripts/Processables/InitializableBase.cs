//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor.
using UnityEngine;

namespace DREditor.Processables
{
    /// <summary>
    ///
    /// </summary>
    public abstract class InitializableBase : MonoBehaviour, IInitializable
    {
        [Tooltip("")]
        [SerializeField] protected bool initializeOnAwake = false;


        /// <summary>
        ///
        /// </summary>
        protected bool initialized = false;


        protected virtual void Awake()
        {
            if (initializeOnAwake)
                Initialize();
        }


        public virtual void Initialize()
        {
            if (initialized)
                return;
            else
                initialized = true;

            Init();
        }

        /// <summary>
        ///
        /// </summary>
        protected abstract void Init();
    }
}