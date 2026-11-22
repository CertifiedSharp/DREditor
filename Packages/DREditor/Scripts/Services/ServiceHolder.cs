//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

namespace DREditor.Services
{
    /// <summary>
    /// Base class for services if they have no base component other than monobehaviour for reduced clutter.
    /// </summary>
    /// <typeparam name="T">The type of class to be registered.</typeparam>
    public class ServiceHolder<T> : MonoBehaviour where T : class
    {
        /// <summary>
        /// When true, only allows one of this script to be active in the game at a time.
        /// </summary>
        [Header("Service Holders are scripts that may hold a service that carries over this game object across scenes instead of destroying it. " +
            "\n\nIf there is no header displaying what type the service is being held, it is likely on another script and is just here to hold it.")]
        [Header("Service Settings")]
        [Space]
        [Tooltip("When true, only allows one of this script to be active in the game at a time.")]
        [SerializeField] protected bool registerService = true;

        /// <summary>
        /// When true, carries over the gameobject to other scenes if it's the root gameobject.  <br/>
        /// ANALYSIS: If this game object is not a root prefab, it depends on the root parent to destroy on load.
        /// </summary>
        [Tooltip("When true, carries over the gameobject to other scenes if it's the root gameobject." +
            "\n\nANALYSIS: If this game object is not a root prefab, it depends on the root parent to destroy on load.")]
        [SerializeField] protected bool dontDestroyOnLoad = true;


        protected virtual void Awake()
        {
            if (registerService)
                InitializeService();
            else if (dontDestroyOnLoad)
                AttemptDontDestroyOnLoad();
        }

        /// <summary>
        /// Uses the <see cref="ServiceInitializer"/> to register this service properly.
        /// </summary>
        protected virtual void InitializeService() => ServiceInitializer.Initialize<T>(this, dontDestroyOnLoad);

        /// <summary>
        /// Attempts to set this game object to not destroy when a new scene is loaded.
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


        protected virtual void OnDestroy() => UnregisterService();

        /// <summary>
        /// Unregisters this service.
        /// </summary>
        protected virtual void UnregisterService() => ServiceLocator.Unregister(this as T);
    }
}