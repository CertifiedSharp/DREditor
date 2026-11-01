//Idea originally from the Dialogue Event List script Author: SeleniumSoul for DREditor
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
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class TypeListBase<T>
    {
        /// <summary>
        ///
        /// </summary>
        protected static readonly List<Type> ListOfClasses = new();


        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public static List<Type> GetList() => ListOfClasses;

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public static List<Type> GetInheritedClasses()
        {
            return AppDomain.CurrentDomain.GetAssemblies().SelectMany(assembly => assembly.GetTypes())
                .Where(type => typeof(T).IsAssignableFrom(type) && !type.IsInterface && !type.IsAbstract).ToList();
        }

        /// <summary>
        ///
        /// </summary>
        public static void UpdateList()
        {
            ListOfClasses.Clear();

            var allInheritedClasses = GetInheritedClasses();

            foreach (var type in allInheritedClasses)
                ListOfClasses.Add(type);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="_types"></param>
        /// <param name="choices"></param>
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
        ///
        /// </summary>
        /// <param name="_types"></param>
        public static void UpdateTypeList(out List<Type> _types)
        {
            if (GetList().Count == 0)
                UpdateList();

            _types = GetList();
        }
    }


    /// <summary>
    ///
    /// </summary>
    public abstract class ClassEditorTypeBase<T> : ClassEditorBase
    {
        /// <summary>
        ///
        /// </summary>
        protected T target;


        public override void OnGUI(object target, params object[] objects)
        {
            this.target = (T)target;

            base.OnGUI(target, objects);
        }
    }

    /// <summary>
    ///
    /// </summary>
    public abstract class ClassEditorBase
    {
        /// <summary>
        ///
        /// </summary>
        protected object baseTarget;

        /// <summary>
        ///
        /// </summary>
        protected object[] parameters;

        /// <summary>
        ///
        /// </summary>
        /// <param name="target"></param>
        /// <param name="objects"></param>
        public virtual void OnGUI(object target, params object[] objects)
        {
            baseTarget = target;

            parameters = objects;

            OnInspectorGUI();
        }

        /// <summary>
        ///
        /// </summary>
        protected abstract void OnInspectorGUI();

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected virtual T GetFirstParameter<T>() => parameters.OfType<T>().FirstOrDefault(item => item != null);
    }

    /// <summary>
    ///
    /// </summary>
    [AttributeUsage(AttributeTargets.Class)]
    public sealed class CustomClassEditorAttribute : Attribute
    {
        /// <summary>
        ///
        /// </summary>
        public Type TargetType { get; private set; }

        public CustomClassEditorAttribute(Type targetType) => TargetType = targetType;
    }

    /// <summary>
    ///
    /// </summary>
    public static class ClassEditorFactory
    {
        /// <summary>
        ///
        /// </summary>
        private const string onGUIMethodName = "OnGUI";

        /// <summary>
        ///
        /// </summary>
        private static readonly List<object> EditorInstances = new();

        /// <summary>
        ///
        /// </summary>
        private static readonly Lazy<List<Type>> _Types = new(() =>
        {
            ClassEditorTypeList.UpdateTypeList(out var list);

            return list;
        });


        /// <summary>
        ///
        /// </summary>
        /// <param name="target"></param>
        /// <param name="objects"></param>
        /// <returns></returns>
        public static bool DrawEditorFor(object target, params object[] objects) => DrawEditorFor(target, false, objects);


        public static bool DrawEditorFor(object target, bool trackInstances, params object[] objects)
        {
            if (!HasValidTargetType(target, out Type editorType))
                return false;

            bool spawnedEditor = SpawnedEditor(editorType, trackInstances, out object editorInstance);

            if (spawnedEditor)
                UseCustomEditor(target, editorInstance, objects);

            return spawnedEditor;
        }

        private static Type GetEditorTargetType(object target) 
            => _Types.Value
                .FirstOrDefault(type => type.GetCustomAttribute<CustomClassEditorAttribute>()?.TargetType == target.GetType());

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

        private static object ContainsTarget(Type editorType)
        {
            foreach (var instance in EditorInstances)
                if (instance.GetType() == editorType)
                    return instance;

            return null;
        }

        private static bool HasValidTargetType(object target, out Type editorType)
        {
            editorType = GetEditorTargetType(target);

            return target != null && editorType != null;
        }

        public static object CreateEditorInstance(object target) 
            => HasValidTargetType(target, out Type editorType) ? Activator.CreateInstance(editorType) : null;

        public static List<object> CreateEditorInstances<T>(List<T> targets)
        {
            List<object> editorInstances = new();

            foreach (var target in targets)
                editorInstances.Add(CreateEditorInstance(target));

            return editorInstances;
        }

        public static void UseCustomEditor(object target, object editorInstance, params object[] objects)
        {
            if (editorInstance != null)
                editorInstance.GetType()?.GetMethod(onGUIMethodName)?.Invoke(editorInstance, new object[] { target, objects });
            else
                EditorGUILayout.LabelField("ALERT: A targeted classes editor instance was null. Contact developer for support.");
        }
    }

    /// <summary>
    ///
    /// </summary>
    [InitializeOnLoad]
    public class ClassEditorTypeList : TypeListBase<ClassEditorBase>
    {
        static ClassEditorTypeList()
        {
            if (!SessionState.GetBool("ClassEditorTypeInitDone", false))
            {
                UpdateList();

                SessionState.SetBool("ClassEditorTypeInitDone", true);
            }
        }
    }
}
#endif