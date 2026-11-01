//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DREditor.Editor.Utility
{
    /// <summary>
    ///
    /// </summary>
    public class ScriptableObjectAssigner : EditorWindow
    {
        /// <summary>
        ///
        /// </summary>
        public List<ScriptableObject> scriptableObjects = new();

        /// <summary>
        ///
        /// </summary>
        public List<MonoScript> components;

        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        public string sendMessage = string.Empty;

        /// <summary>
        /// 
        /// </summary>
        public bool thisSceneOnly = true;

        /// <summary>
        ///
        /// </summary>
        public bool applyToBuildScenesOnly = false;

        /// <summary>
        ///
        /// </summary>
        public bool scriptableObjectNameIsField = true;

        /// <summary>
        ///
        /// </summary>
        public List<string> fieldNames = new();


        /// <summary>
        ///
        /// </summary>
        protected int assignedCount = 0;

        /// <summary>
        ///
        /// </summary>
        protected SerializedObject scriptableObjectsProperty;

        /// <summary>
        ///
        /// </summary>
        protected SerializedObject componentsProperty;

        /// <summary>
        ///
        /// </summary>
        protected SerializedObject fieldNamesProperty;


        [MenuItem("DREditor/Utility/Scriptable Object Assigner")]
        public static void ShowWindow() => GetWindow<ScriptableObjectAssigner>("Scriptable Object Assigner");

        protected virtual void OnGUI()
        {
            DisplayListProperties();

            CustomLayoutUtility.DisplayHorizontal(SendMessageField);

            DisplayOptionFields();

            try
            {
                DisplayFieldNamesProperty();

                if (GUILayout.Button("Assign to All Components in Scenes"))
                    AssignAllInstancesInScenes();
            }
            catch (ArgumentException exception)
            {
                UnityEngine.Debug.Log("Skipping Noticed argument exception: " + exception.ToString());
                // This is to skip getting the 
                // ArgumentException: Getting control 4's position in a group with only 4 controls when doing repaint
            }
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void DisplayListProperties()
        {
            GUILayout.Label("Assign Scriptable Object to Components", EditorStyles.boldLabel);

            scriptableObjectsProperty = DisplayProperty(scriptableObjectsProperty, "scriptableObjects");

            componentsProperty = DisplayProperty(componentsProperty, "components");
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void SendMessageField()
        {
            GUILayout.Label("Send Message:");

            sendMessage = GUILayout.TextField(sendMessage);
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void DisplayOptionFields()
        {
            applyToBuildScenesOnly = GUILayout.Toggle(applyToBuildScenesOnly, "Apply to Build Scenes Only");

            thisSceneOnly = GUILayout.Toggle(thisSceneOnly, "This Scene Only");

            if (scriptableObjects != null)
                scriptableObjectNameIsField = GUILayout.Toggle(scriptableObjectNameIsField, "Scriptable Object Name Is Field");
        }

        /// <summary>
        /// 
        /// </summary>
        protected virtual void DisplayFieldNamesProperty()
        {
            if (fieldNames.Count != scriptableObjects.Count)
            {
                fieldNames.Clear();

                foreach (var obj in scriptableObjects)
                    if (obj != null)
                        fieldNames.Add(obj.name);
            }

            if (!scriptableObjectNameIsField &&
                scriptableObjects != null && scriptableObjects.Count > 0 && scriptableObjects[0] != null)
                fieldNamesProperty = DisplayProperty(fieldNamesProperty, "fieldNames");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="serializedObject"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        protected virtual SerializedObject DisplayProperty(SerializedObject serializedObject, string fieldName)
        {
            if (serializedObject != null)
            {
                serializedObject.Update();

                EditorGUILayout.PropertyField(serializedObject.FindProperty(fieldName), true);

                serializedObject.ApplyModifiedProperties();
            }
            else
                serializedObject = new SerializedObject(this);

            return serializedObject;
        }

        #region AssignAllInstancesInScenes

        /// <summary>
        ///
        /// </summary>
        protected virtual void AssignAllInstancesInScenes()
        {
            if (!ValidObjects() || !ValidComponentTypes())
                return;

            if (!thisSceneOnly && GetScenePaths() is string[] allScenePaths)
                foreach (var scenePath in allScenePaths)
                    AssignAllInstancesIn(EditorSceneManager.OpenScene(scenePath, OpenSceneMode.Single));
            else
                AssignAllInstancesIn(SceneManager.GetActiveScene());

            EditorUtility.DisplayDialog("Complete", $"Completed assigning Scriptable Objects {assignedCount} times.", "OK");
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool ValidObjects()
        {
            bool result = !scriptableObjects.Contains(null) && !components.Contains(null);

            if (!result)
                EditorUtility.DisplayDialog("Error", "Please ensure Scriptable Objects and Components are assigned.", "OK");

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool ValidComponentTypes()
        {
            foreach (var component in components)
                if (!ValidComponentType(component))
                    return false;

            return true;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool ValidComponentType(MonoScript component)
        {
            bool result = component.GetClass() != null && typeof(MonoBehaviour).IsAssignableFrom(component.GetClass());

            if (!result)
                EditorUtility.DisplayDialog("Error", "The assigned script is not a MonoBehaviour.", "OK");

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual string[] GetScenePaths()
            => applyToBuildScenesOnly
                ? GetBuildScenesPaths()
                : AssetDatabase.FindAssets("t:Scene").Select(AssetDatabase.GUIDToAssetPath).ToArray();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        public static string[] GetBuildScenesPaths()
            => EditorBuildSettings.scenes
            .Where(scene => scene.enabled)
            .Select(scene => scene.path)
            .ToArray();

        #endregion

        #region AssignAllInstancesInSceneAndObjects

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scene"></param>
        protected virtual void AssignAllInstancesIn(Scene scene)
        {
            var allObjects = scene.GetRootGameObjects();

            int initialCount = assignedCount;

            foreach (var gameObject in allObjects)
                if (components.Count != 0)
                    foreach (var component in components)
                        AssignAllInstancesOnObject(scene, gameObject, component.GetClass());
                else
                    AssignAllInstancesInFamily(scene, gameObject);

            if (initialCount != assignedCount)
                EditorSceneManager.SaveScene(scene);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="gameObject"></param>
        protected virtual void AssignAllInstancesInFamily(Scene scene, GameObject gameObject)
        {
            var components = gameObject.GetComponents<Component>();

            foreach (var component in components)
                foreach (var scriptableObject in scriptableObjects)
                {
                    AssignAllInstancesOnComponent(component, component.GetType(), scriptableObject);

                    EditorUtility.SetDirty(component);
                }

            if (gameObject.transform.childCount > 0)
                foreach (Transform child in gameObject.transform)
                    AssignAllInstancesInFamily(scene, child.gameObject);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="gameObject"></param>
        /// <param name="componentType"></param>
        protected virtual void AssignAllInstancesOnObject(Scene scene, GameObject gameObject, Type componentType)
        {
            var components = gameObject.GetComponentsInChildren(componentType, true);

            if (components.Length > 0 && !thisSceneOnly)
                UnityEngine.Debug.Log(GetType() + ": " + components.Length +
                    " Found on Object: " + gameObject.name + " in scene: " + scene.name);

            foreach (var component in components)
                foreach (var scriptableObject in scriptableObjects)
                    AssignAllInstancesOnComponent(component, componentType, scriptableObject);
        }

        #endregion

        #region AssignAllInstancesOnComponent

        /// <summary>
        ///
        /// </summary>
        /// <param name="component"></param>
        /// <param name="componentType"></param>
        /// <param name="scriptableObject"></param>
        /// <remarks></remarks>
        protected virtual void AssignAllInstancesOnComponent(Component component, Type componentType,
            ScriptableObject scriptableObject)
        {
            FieldInfo fieldInfo = GetFieldInfo(componentType, fieldNames[scriptableObjects.IndexOf(scriptableObject)]);

            if (FieldMatchesObject(fieldInfo, scriptableObject))
                AssignObjectToField(component, fieldInfo, scriptableObject);
            else if (!thisSceneOnly)
                UnityEngine.Debug.LogWarning(GetType() + ": ALERT: The field Info for: "
                    + fieldNames[scriptableObjects.IndexOf(scriptableObject)] + " couldn't be found.");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="componentType"></param>
        /// <param name="fieldName"></param>
        /// <returns></returns>
        protected virtual FieldInfo GetFieldInfo(Type componentType, string fieldName)
        {
            FieldInfo fieldInfo = componentType
                .GetField(fieldName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance);

            if (fieldInfo == null && fieldName.Length >= 2 && char.IsUpper(fieldName[0]))
            {
                string pascalName = char.ToLower(fieldName[0]) + fieldName[1..];

                if (!thisSceneOnly)
                    UnityEngine.Debug.Log(GetType().ToString() + ": Field info couldn't be found with name : " + fieldName +
                        " So checking with name: " + pascalName);

                return GetFieldInfo(componentType, pascalName);
            }

            return fieldInfo;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="fieldInfo"></param>
        /// <param name="scriptableObject"></param>
        /// <returns></returns>
        protected virtual bool FieldMatchesObject(FieldInfo fieldInfo, ScriptableObject scriptableObject)
            => fieldInfo != null && fieldInfo.FieldType == scriptableObject.GetType();

        /// <summary>
        ///
        /// </summary>
        /// <param name="component"></param>
        /// <param name="fieldInfo"></param>
        /// <param name="scriptableObject"></param>
        protected virtual void AssignObjectToField(Component component, FieldInfo fieldInfo, ScriptableObject scriptableObject)
        {
            UnityEngine.Debug.Log($"{GetType()}: Field info was found! Assigning value.");

            fieldInfo.SetValue(component, scriptableObject);

            if (!string.IsNullOrEmpty(sendMessage))
            {
                component.SendMessage(sendMessage, SendMessageOptions.DontRequireReceiver);

                EditorUtility.SetDirty(component);
            }

            assignedCount++;
        }

        #endregion
    }
}