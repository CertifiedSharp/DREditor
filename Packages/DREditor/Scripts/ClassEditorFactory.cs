//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;

namespace DREditor
{
    /// <summary>
    /// Factory class to instantiate usable custom POCO class editors that inherit <see cref="ClassEditorBase"/>.
    /// </summary>
    public static class ClassEditorFactory
    {
        /// <summary>
        /// The name of the Unity Native OnGUI Method.
        /// </summary>
        private const string onGUIMethodName = "OnGUI";

        /// <summary>
        /// The currently instantiated editor instances active in the inspector.
        /// </summary>
        private static readonly List<object> EditorInstances = new();

        /// <summary>
        /// The list of class types that inherit <see cref="ClassEditorBase"/>.
        /// </summary>
        private static readonly Lazy<List<Type>> _Types = new(() =>
        {
            ClassEditorTypeList.UpdateTypeList(out var list);

            return list;
        });


        /// <summary>
        /// Public function to display an editor for the target object.
        /// </summary>
        /// <param name="target">The target object to draw an editor for.</param>
        /// <param name="objects">Additionally sent parameters in order to assist the editing process of the target.</param>
        /// <returns>True if the creation of a custom editor to use was successful.</returns>
        public static bool DrawEditorFor(object target, params object[] objects) => DrawEditorFor(target, false, objects);

        /// <summary>
        /// Public function to display an editor for the target object with the option to track active editor instances.
        /// </summary>
        /// <param name="target">The target object to draw an editor for.</param>
        /// <param name="trackInstances">Whether to keep track of the spawned editor instance.</param>
        /// <param name="objects">Additionally sent parameters in order to assist the editing process of the target.</param>
        /// <returns>True if the creation of a custom editor to use was successful.</returns>
        public static bool DrawEditorFor(object target, bool trackInstances, params object[] objects)
        {
            if (!HasValidTargetType(target, out Type editorType))
                return false;

            bool spawnedEditor = SpawnedEditor(editorType, trackInstances, out object editorInstance);

            if (spawnedEditor)
                UseCustomEditor(target, editorInstance, objects);

            return spawnedEditor;
        }

        /// <summary>
        /// Attempts to spawn an Custom Class Editor of the specified editor type.
        /// </summary>
        /// <param name="editorType">The specified editor type to spawn.</param>
        /// <param name="trackInstances">Whether to keep track of the spawned editor instance.</param>
        /// <param name="editorInstance">The spawned editor instance that will be used to draw the target.</param>
        /// <returns>True if an editor instance was successfully instantiated.</returns>
        private static bool SpawnedEditor(Type editorType, bool trackInstances, out object editorInstance)
        {
            if (trackInstances && ContainsTarget(editorType) is object validEditorInstance)
                editorInstance = validEditorInstance;
            else
            {
                editorInstance = Activator.CreateInstance(editorType);

                if (trackInstances)
                    EditorInstances.Add(editorInstance);
            }

            return editorInstance != null;
        }

        /// <summary>
        /// Searches the active editor instances that are the specified type.
        /// </summary>
        /// <param name="editorType">The type of the editor being searched for.</param>
        /// <returns>The active editor instance of the specified type if it is available.</returns>
        private static object ContainsTarget(Type editorType)
        {
            foreach (var instance in EditorInstances)
                if (instance.GetType() == editorType)
                    return instance;

            return null;
        }

        /// <summary>
        /// Checks whether the system can find an appropriate editor type for the specified target.
        /// </summary>
        /// <param name="target">The target object to draw an editor for.</param>
        /// <param name="editorType">The type for the custom editor to use for the target if found.</param>
        /// <returns>True if the system can find an appropriate editor type for the specified target.</returns>
        private static bool HasValidTargetType(object target, out Type editorType)
        {
            editorType = GetEditorTargetType(target);

            return target != null && editorType != null;
        }

        /// <summary>
        /// Retrieves the type of editor that should be used for the specified type.
        /// </summary>
        /// <param name="target">The target object to draw an editor for.</param>
        /// <returns>Type of editor that should be used for the specified type.</returns>
        private static Type GetEditorTargetType(object target)
            => _Types.Value
                .FirstOrDefault(type => type.GetCustomAttribute<CustomClassEditorAttribute>()?.TargetType == target.GetType());


        /// <summary>
        /// Creates an editor instance to use for the specified target.
        /// </summary>
        /// <param name="target">The target object to draw an editor for.</param>
        /// <returns>An instance of an editor to use for the sent target.</returns>
        public static object CreateEditorInstance(object target) 
            => HasValidTargetType(target, out Type editorType) ? Activator.CreateInstance(editorType) : null;


        /// <summary>
        /// Use the editor instance to edit the sent target, using additional parameters to asset the editing process if applicable.
        /// </summary>
        /// <param name="target">The target object to draw an editor for.</param>
        /// <param name="editorInstance">The editor instance used to edit the target data.</param>
        /// <param name="objects">Additionally sent parameters in order to assist the editing process of the target.</param>
        public static void UseCustomEditor(object target, object editorInstance, params object[] objects)
        {
            if (editorInstance != null)
                editorInstance.GetType()?.GetMethod(onGUIMethodName)?.Invoke(editorInstance, new object[] { target, objects });
            else
                EditorGUILayout.LabelField("ALERT: A targeted classes editor instance was null. Contact developer for support.");
        }

        // Scrapped
        ///// <summary>
        ///// 
        ///// </summary>
        ///// <typeparam name="T"></typeparam>
        ///// <param name="targets"></param>
        ///// <returns></returns>
        //public static List<object> CreateEditorInstances<T>(List<T> targets)
        //{
        //    List<object> editorInstances = new();

        //    foreach (var target in targets)
        //        editorInstances.Add(CreateEditorInstance(target));

        //    return editorInstances;
        //}
    }
}
#endif