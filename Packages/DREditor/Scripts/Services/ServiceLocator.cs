//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.Services
{
    /// <summary>
    ///
    /// </summary>
    public static class ServiceLocator
    {
        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        private static readonly Dictionary<Type, object> _Registries = new();


        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceInstance"></param>
        public static void Register<T>(T serviceInstance) => _Registries[typeof(T)] = serviceInstance;

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static T GetService<T>()
        {
            if (!HasService<T>())
            {
                if (!ApplicationIsAboutToExitPlayMode() && Application.isPlaying)
                    UnityEngine.Debug.LogWarning($"ALERT: {nameof(ServiceLocator)}: There is no service {typeof(T)} registered!");

                return default;
            }

            T serviceInstance = (T)_Registries[typeof(T)];

            return serviceInstance;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public static bool ApplicationIsAboutToExitPlayMode()
        {
#if UNITY_EDITOR
            return IsExitingPlayMode;
#else
            return false;
#endif
        }

#if UNITY_EDITOR
        /// <summary>
        ///
        /// </summary>
        private static bool IsExitingPlayMode { get; set; } = false;

        /// <summary>
        ///
        /// </summary>
        [UnityEditor.InitializeOnLoadMethod]
        public static void RegisterPlayModeCallback()
        {
            UnityEditor.EditorApplication.playModeStateChanged += state =>
            {
                IsExitingPlayMode = state == UnityEditor.PlayModeStateChange.ExitingPlayMode;
            };
        }
#endif

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="serviceInstance"></param>
        /// <returns></returns>
        public static bool Unregister<T>(T serviceInstance)
        {
            bool result = _Registries.ContainsValue(serviceInstance);

            if (result)
                Unregister<T>();

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        public static void Unregister<T>() => _Registries.Remove(typeof(T));

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        public static bool HasService<T>() => _Registries.ContainsKey(typeof(T));

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="existing"></param>
        /// <returns></returns>
        public static bool HasService<T>(out T existing)
        {
            existing = default;

            bool result = _Registries.ContainsKey(typeof(T));

            if (result)
                existing = (T)_Registries[typeof(T)];

            return result;
        }
    }
}