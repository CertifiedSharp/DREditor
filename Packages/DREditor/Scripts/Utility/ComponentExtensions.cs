//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DREditor.Utility
{
    /// <summary>
    ///
    /// </summary>
    public static class ComponentExtensions
    {
        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="components"></param>
        /// <returns></returns>
        public static T GetSelectedObjectOfType<T>(this IReadOnlyList<object> components)
        {
            foreach (var component in components)
                if (component is T targetDigit &&
                    component is MonoBehaviour behaviour &&
                    behaviour.gameObject == EventSystem.current.currentSelectedGameObject)
                    return targetDigit;

            return default;
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="components"></param>
        /// <param name="invocationMethod"></param>
        public static void InvokeComponents<T>(this IReadOnlyList<T> components, Action<T> invocationMethod)
        {
            if (components == null || invocationMethod == null)
                return;

            for (int i = 0; i < components.Count; i++)
                invocationMethod(components[i]);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="behaviour"></param>
        /// <param name="invocationMethod"></param>
        public static void InvokeFamily<T>(this MonoBehaviour behaviour, Action<T> invocationMethod)
        {
            InvokeThisPlusComponents(behaviour, invocationMethod);

            InvokeChildren(behaviour, invocationMethod);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="behaviour"></param>
        /// <param name="invocationMethod"></param>
        public static void InvokeThisPlusComponents<T>(this MonoBehaviour behaviour, Action<T> invocationMethod)
        {
            behaviour.InvokeThisAs(invocationMethod);

            if (behaviour is IComponentsCache cache && cache.CachedComponents != null)
                cache.CachedComponents.InvokeComponents(invocationMethod);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="behaviour"></param>
        /// <param name="invocationMethod"></param>
        public static void InvokeThisAs<T>(this MonoBehaviour behaviour, Action<T> invocationMethod)
        {
            if (behaviour == null || invocationMethod == null)
                return;

            if (behaviour is T soughtType)
                invocationMethod(soughtType);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <typeparam name="T2"></typeparam>
        /// <param name="components"></param>
        /// <param name="invocationMethod"></param>
        public static void InvokeComponents<T, T2>(this IReadOnlyList<T2> components, Action<T> invocationMethod)
        {
            if (components == null || invocationMethod == null)
                return;

            for (int i = 0; i < components.Count; i++)
                if (components[i] is T soughtType)
                    invocationMethod(soughtType);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="behaviour"></param>
        /// <param name="invocationMethod"></param>
        public static void InvokeChildren<T>(this MonoBehaviour behaviour, Action<T> invocationMethod)
        {
            if (behaviour is IComponentsCache cache)
                InvokeChildCachesRecursively(cache, invocationMethod);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parentCache"></param>
        /// <param name="invocationMethod"></param>
        public static void InvokeComponents<T>(this IComponentsCache parentCache, Action<T> invocationMethod)
        {
            if (parentCache.CachedComponents == null)
                return;

            foreach (var component in parentCache.CachedComponents)
                if (component is T soughtType)
                    invocationMethod(soughtType);
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="parentCache"></param>
        /// <param name="invocationMethod"></param>
        private static void InvokeChildCachesRecursively<T>(IComponentsCache parentCache, Action<T> invocationMethod)
        {
            if (parentCache.CachedComponents == null) 
                return;

            foreach (var child in parentCache.CachedComponents)
            {
                if (child == null || child is IComponentsCache childCache && childCache == parentCache) 
                    continue;

                if (child is T soughtType)
                    invocationMethod(soughtType);

                if (child is MonoBehaviour childMono)
                    childMono.InvokeThisPlusComponents(invocationMethod);

                child.InvokeChildren(invocationMethod);
            }
        }
    }

    /// <summary>
    ///
    /// </summary>
    public interface IComponentsCache
    {
        /// <summary>
        ///
        /// </summary>
        MonoBehaviour[] CachedComponents { get; }
    }
}