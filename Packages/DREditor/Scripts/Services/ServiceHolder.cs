//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

namespace DREditor.Services
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class ServiceHolder<T> : MonoBehaviour where T : class
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected bool registerService = true;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool dontDestroyOnLoad = true;


        /// <summary>
        ///
        /// </summary>
        protected virtual void Awake()
        {
            if (registerService)
                InitializeService();
            else if (dontDestroyOnLoad)
                AttemptDontDestroyOnLoad();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeService() => ServiceInitializer.Initialize<T>(this, dontDestroyOnLoad);

        /// <summary>
        ///
        /// </summary>
        protected virtual void AttemptDontDestroyOnLoad()
        {
            if (transform.parent == null)
                MonoBehaviour.DontDestroyOnLoad(gameObject);
            else
                UnityEngine.Debug.LogWarning($"{gameObject.name}: Could not set: " +
                    $"don't destroy on load because the game object is not the root parent. " +
                    $"Please uncheck the dont destroy on load option.");
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDestroy() => ServiceLocator.Unregister(this as T);
    }
}