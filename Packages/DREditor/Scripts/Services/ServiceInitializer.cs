//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

namespace DREditor.Services
{
    /// <summary>
    ///
    /// </summary>
    public static class ServiceInitializer
    {
        /// <summary>
        ///
        /// </summary>
        private static readonly string DontDestroyOnLoadSceneName = "DontDestroyOnLoad";


        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <param name="dontDestroyOnLoad"></param>
        public static void Initialize<T>(MonoBehaviour instance, bool dontDestroyOnLoad = true) where T : class
        {
            if (!ServiceLocator.HasService<T>())
                ServiceLocator.Register(instance as T);
            else
                MonoBehaviour.Destroy(instance.gameObject);

            InitializeDestroyOnLoad(instance.gameObject, dontDestroyOnLoad);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <param name="gameObject"></param>
        /// <param name="dontDestroyOnLoad"></param>
        public static void Initialize<T>(T instance, GameObject gameObject, bool dontDestroyOnLoad = true) where T : class
        {
            if (!ServiceLocator.HasService<T>())
                ServiceLocator.Register(instance);
            else
                MonoBehaviour.Destroy(gameObject);

            InitializeDestroyOnLoad(gameObject, dontDestroyOnLoad);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="instance"></param>
        /// <param name="dontDestroyOnLoad"></param>
        /// <remarks>
        ///
        /// </remarks>
        public static void HardInitialize<T>(MonoBehaviour instance, bool dontDestroyOnLoad = true) where T : class
        {
            InitializeDestroyOnLoad(instance.gameObject, dontDestroyOnLoad);

            if (ServiceLocator.HasService(out T existing) && existing is MonoBehaviour existingBehaviour)
            {
                bool existingIsPersistent = existingBehaviour.gameObject.IsPersistent();

                // If I am the persistent one and the existing one is just temporary,
                // I should kick the old one out instead of killing myself.
                if (dontDestroyOnLoad && !existingIsPersistent)
                {
                    ServiceLocator.Register(instance as T); // Overwrite the weak one.

                    return;
                }

                // If I'm not persistent and someone is already there that is, I'm the duplicate.
                if (!dontDestroyOnLoad && existingIsPersistent)
                {
                    MonoBehaviour.Destroy(instance.gameObject);

                    return;
                }
            }

            ServiceLocator.Register(instance as T);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="go"></param>
        /// <returns></returns>
        public static bool IsPersistent(this GameObject go) => go.scene.name == DontDestroyOnLoadSceneName;

        /// <summary>
        ///
        /// </summary>
        /// <param name="gameObject"></param>
        /// <param name="dontDestroyOnLoad"></param>
        static void InitializeDestroyOnLoad(GameObject gameObject, bool dontDestroyOnLoad = true)
        {
            if (dontDestroyOnLoad)
                if (gameObject.transform.parent == null)
                    MonoBehaviour.DontDestroyOnLoad(gameObject);
                else
                    UnityEngine.Debug.LogWarning($"ALERT: {gameObject.name}: Could not set: " +
                        $"don't destroy on load because the game object is not the root parent. " +
                        $"Please uncheck the dont destroy on load option.");
        }
    }
}