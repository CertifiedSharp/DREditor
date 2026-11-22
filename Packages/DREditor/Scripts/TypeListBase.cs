//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
//I had the idea for this system after dissecting SeleniumSoul's Dialogue Event List script.
#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;

namespace DREditor
{
    /// <summary>
    /// Tracks a list of classes that inherit <typeparamref name="T"/> in the unity project.
    /// </summary>
    /// <typeparam name="T">The base type to track the family of.</typeparam>
    public abstract class TypeListBase<T>
    {
        /// <summary>
        /// The tracked list of classes that inherit <typeparamref name="T"/>.
        /// </summary>
        protected static readonly List<Type> ListOfClasses = new();


        /// <summary>
        /// Retrieves the tracked list of classes that inherit <typeparamref name="T"/>.
        /// </summary>
        /// <returns>The cached list of class types that inherit <typeparamref name="T"/>.</returns>
        public static List<Type> GetList() => ListOfClasses;

        /// <summary>
        /// Scans all the assemblies for classes that inherit <typeparamref name="T"/>.
        /// </summary>
        /// <returns>A list of types that are classes that inherit <typeparamref name="T"/>.</returns>
        public static List<Type> GetInheritedClasses() => AppDomain.CurrentDomain.GetAssemblies()
            .SelectMany(assembly => assembly.GetTypes())
            .Where(type => typeof(T).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract)
            .ToList();

        /// <summary>
        /// Clears the active lists of classes and repopulates it.
        /// </summary>
        public static void UpdateList()
        {
            ListOfClasses.Clear();

            var inheritedClasses = GetInheritedClasses();

            foreach (var type in inheritedClasses)
                ListOfClasses.Add(type);
        }

        /// <summary>
        /// Updates the cached list of class types and outputs those types along with the names of each to be displayed to GUI.
        /// </summary>
        /// <param name="_types">Cached list of class types inheriting <typeparamref name="T"/>.</param>
        /// <param name="choices">String array of the names of each type inheriting <typeparamref name="T"/>.</param>
        public static void UpdateTypeList(out List<Type> _types, out string[] choices)
        {
            UpdateTypeList(out _types);

            choices = new string[_types.Count];

            for (int i = 0; i < choices.Length; i++)
                choices[i] = _types[i].Name;
            // This function is called a lot, so this log should be kept here for internal debugging just in case.
            //UnityEngine.Debug.Log("Updating Type List: " + nameof(T));
        }

        /// <summary>
        /// Updates the cached list of class types if the active list is empty. <br/>
        /// Outputs the cached list of class types inheriting <typeparamref name="T"/>.
        /// </summary>
        /// <param name="_types">Cached list of class types inheriting <typeparamref name="T"/>.</param>
        public static void UpdateTypeList(out List<Type> _types)
        {
            if (GetList().Count == 0)
                UpdateList();

            _types = GetList();
        }
    }
}
#endif