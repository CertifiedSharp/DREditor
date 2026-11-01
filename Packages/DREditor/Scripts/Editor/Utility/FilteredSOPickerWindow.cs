//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
#if UNITY_EDITOR
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DREditor.Editor.Utility
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class FilteredSOPickerWindow<T> : PickerWindowBase<ScriptableObject>
    {
        /// <summary>
        ///
        /// </summary>
        protected readonly List<ScriptableObject> allAssets = new();

        /// <summary>
        ///
        /// </summary>
        protected List<ScriptableObject> filteredObjects = new();

        /// <summary>
        ///
        /// </summary>
        protected string search = string.Empty;

        /// <summary>
        ///
        /// </summary>
        protected Vector2 scroll;

        /// <summary>
        ///
        /// </summary>
        protected List<Type> types;


        protected virtual void OnEnable() => GenericTypeList<T>.UpdateTypeList(out types, out _);


        protected override void Initialize() => RefreshAssets();

        /// <summary>
        ///
        /// </summary>
        protected virtual void RefreshAssets()
        {
            allAssets.Clear();

            foreach (Type type in types)
            {
                string[] guids = AssetDatabase.FindAssets($"t:{type.Name}");

                foreach (string guid in guids)
                {
                    string path = AssetDatabase.GUIDToAssetPath(guid);

                    var scriptableObject = AssetDatabase.LoadAssetAtPath(path, type) as ScriptableObject;

                    if (scriptableObject != null && !allAssets.Contains(scriptableObject))
                        allAssets.Add(scriptableObject);
                }
            }

            allAssets.Sort((a, b) => string.Compare(a.name, b.name, StringComparison.OrdinalIgnoreCase));

            ApplySearch();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ApplySearch()
        {
            filteredObjects = string.IsNullOrWhiteSpace(search) ?
                new List<ScriptableObject>(allAssets) :
                allAssets.Where(so => SearchInString(so.name) || SearchInString(so.GetType().Name)).ToList();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="input"></param>
        /// <returns></returns>
        protected virtual bool SearchInString(string input)
            => input.IndexOf(search, StringComparison.OrdinalIgnoreCase) >= 0;


        /// <summary>
        ///
        /// </summary>
        protected virtual void DrawFilteredAssetList()
        {
            scroll = EditorGUILayout.BeginScrollView(scroll);

            if (filteredObjects.Count == 0)
                EditorGUILayout.HelpBox(
                    $"No ScriptableObjects implementing {typeof(T).Name} found in the project.",
                    MessageType.Info);
            else
                foreach (var asset in filteredObjects)
                    DrawRow(asset);

            EditorGUILayout.EndScrollView();
        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="asset"></param>
        protected abstract void DrawRow(ScriptableObject asset);


        /// <summary>
        ///
        /// </summary>
        /// <param name="asset"></param>
        protected virtual void Assign(ScriptableObject asset)
        {
            if (targetObject != null)
                ApplyAssignedModifications(asset);
            else
                UnityEngine.Debug.LogWarning($"ALERT: {nameof(SimpleFilteredSOWindow<T>)} " +
                    $"SerializedObject was destroyed — reopen the picker from the inspector.");

            Close();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="asset"></param>
        protected virtual void ApplyAssignedModifications(ScriptableObject asset)
        {
            targetObject.Update();

            SerializedProperty prop = targetObject.FindProperty(propertyPath);

            if (prop != null)
            {
                prop.objectReferenceValue = asset;

                targetObject.ApplyModifiedProperties();
            }
            else
                UnityEngine.Debug.LogWarning($"ALERT: {nameof(SimpleFilteredSOWindow<T>)} " +
                    $"Property path '{propertyPath}' not found.");
        }
    }
}
#endif