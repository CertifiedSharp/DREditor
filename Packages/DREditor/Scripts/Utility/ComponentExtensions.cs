//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DREditor.Utility
{
    /// <summary>
    /// Extension Methods for read-only lists, in combination with monobehaviours so that families of behaviours can be called en masse.
    /// </summary>
    public static class ComponentExtensions
    {
        /// <summary>
        /// Checks a list of components that inherit <typeparamref name="T"/> that also are on the currently selected GameObject.
        /// </summary>
        /// <typeparam name="T">The type the components should inherit from to be eligible for evaluation.</typeparam>
        /// <param name="components">List of components to filter through.</param>
        /// <returns>The <typeparamref name="T"/> instance that is on the currently selected GameObject. Otherwise null.</returns>
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
        /// Invokes a method repeatedly called sending each component in the list.
        /// </summary>
        /// <typeparam name="T">The type of component targeted to pass through the <paramref name="invocationMethod"/>.</typeparam>
        /// <param name="components">The list of components where each will be passed through the <paramref name="invocationMethod"/>.</param>
        /// <param name="invocationMethod">The method to be invoked passing each component in the cache/list with every call.</param>
        public static void InvokeComponents<T>(this IReadOnlyList<T> components, Action<T> invocationMethod)
        {
            if (components == null || invocationMethod == null)
                return;

            for (int i = 0; i < components.Count; i++)
                invocationMethod(components[i]);
        }

        /// <summary>
        /// Invokes the behavour as <typeparamref name="T"/>, 
        /// as well as applies this to all children in it's cache if it implements <see cref="IComponentsCache"/>. <br/>
        /// Then, gets all child Game Objects and calls <see cref="InvokeThisPlusComponents"/> on them recursively.
        /// </summary>
        /// <typeparam name="T">The type of component targeted to pass through the <paramref name="invocationMethod"/>.</typeparam>
        /// <param name="behaviour">The component to invoke as <typeparamref name="T"/>.</param>
        /// <param name="invocationMethod">The method to be invoked passing each component in the cache/list with every call.</param>
        public static void InvokeFamily<T>(this MonoBehaviour behaviour, Action<T> invocationMethod)
        {
            InvokeThisPlusComponents(behaviour, invocationMethod);

            InvokeChildren(behaviour, invocationMethod);
        }

        /// <summary>
        /// Invokes the function passing the behaviour if it is of type <typeparamref name="T"/>. <br/>
        /// Additionally, if the behaviour implements <see cref="IComponentsCache"/>: <br/>
        /// Invokes <paramref name="invocationMethod"/> for each component in the cache.
        /// </summary>
        /// <typeparam name="T">The type of component targeted to pass through the <paramref name="invocationMethod"/>.</typeparam>
        /// <param name="behaviour">The component to invoke as <typeparamref name="T"/>.</param>
        /// <param name="invocationMethod">The method to be invoked passing each component in the cache/list with every call.</param>
        public static void InvokeThisPlusComponents<T>(this MonoBehaviour behaviour, Action<T> invocationMethod)
        {
            behaviour.InvokeThisAs(invocationMethod);

            if (behaviour is IComponentsCache cache && cache.CachedComponents != null)
                cache.CachedComponents.InvokeComponents(invocationMethod);
        }

        /// <summary>
        /// Invokes the function passing the behaviour if it is of type <typeparamref name="T"/>.
        /// </summary>
        /// <typeparam name="T">The type of component targeted to pass through the <paramref name="invocationMethod"/>.</typeparam>
        /// <param name="behaviour">The behaviour to to pass to the invoke method as <typeparamref name="T"/>.</param>
        /// <param name="invocationMethod">The method to be invoked passing each component in the cache/list with every call.</param>
        public static void InvokeThisAs<T>(this MonoBehaviour behaviour, Action<T> invocationMethod)
        {
            if (behaviour == null || invocationMethod == null)
                return;

            if (behaviour is T soughtType)
                invocationMethod(soughtType);
        }

        /// <summary>
        /// Invokes <paramref name="invocationMethod"/> for each <typeparamref name="T"/> in the list of <typeparamref name="T2"/>.
        /// </summary>
        /// <typeparam name="T">The type that components in the list of <typeparamref name="T2"/> must inherit.</typeparam>
        /// <typeparam name="T2">The type consisting of the list <paramref name="components"/>.</typeparam>
        /// <param name="components">List of components to filter through to invoke.</param>
        /// <param name="invocationMethod">The method to be invoked passing each component in the cache/list with every call.</param>
        public static void InvokeComponents<T, T2>(this IReadOnlyList<T2> components, Action<T> invocationMethod)
        {
            if (components == null || invocationMethod == null)
                return;

            for (int i = 0; i < components.Count; i++)
                if (components[i] is T soughtType)
                    invocationMethod(soughtType);
        }

        /// <summary>
        /// If the behaviour calling this function is <see cref="IComponentsCache"/>: <br/>
        /// Invokes the <paramref name="invocationMethod"/> passing each <typeparamref name="T"/> component found in the cache, 
        /// then checking and invoking their children if they too are a <see cref="IComponentsCache"/>. <br/>
        /// Otherwise, nothing.
        /// </summary>
        /// <typeparam name="T">The type of component targeted to pass through the <paramref name="invocationMethod"/>.</typeparam>
        /// <param name="behaviour">The behaviour to invoke cached components on if applicable.</param>
        /// <param name="invocationMethod">The method to be invoked passing each component in the cache/list with every call.</param>
        public static void InvokeChildren<T>(this MonoBehaviour behaviour, Action<T> invocationMethod)
        {
            if (behaviour is IComponentsCache cache)
                InvokeChildCachesRecursively(cache, invocationMethod);
        }

        /// <summary>
        /// For each component in the cache, invokes the <paramref name="invocationMethod"/>, passing each component as the parameter.
        /// </summary>
        /// <typeparam name="T">The type of component targeted to pass through the <paramref name="invocationMethod"/>.</typeparam>
        /// <param name="parentCache">The parent to invoke it's cached children on if applicable.</param>
        /// <param name="invocationMethod">The method to be invoked passing each component in the cache/list with every call.</param>
        public static void InvokeComponents<T>(this IComponentsCache parentCache, Action<T> invocationMethod)
        {
            if (parentCache.CachedComponents == null)
                return;

            foreach (var component in parentCache.CachedComponents)
                if (component is T soughtType)
                    invocationMethod(soughtType);
        }

        /// <summary>
        /// Invokes the <paramref name="invocationMethod"/> passing each <typeparamref name="T"/> component found in the cache, 
        /// then checking and invoking their children if they too are a <see cref="IComponentsCache"/>.
        /// </summary>
        /// <typeparam name="T">The type of component targeted to pass through the <paramref name="invocationMethod"/>.</typeparam>
        /// <param name="parentCache">The parent to invoke it's cached children on if applicable.</param>
        /// <param name="invocationMethod">The method to be invoked passing each component in the cache/list with every call.</param>
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
}