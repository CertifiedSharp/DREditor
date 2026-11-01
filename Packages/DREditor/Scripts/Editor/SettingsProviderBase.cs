//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.IO;
using UnityEditor;
using UnityEngine;

namespace DREditor.Editor
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class SettingsProviderBase<T> where T : ProjectSettingsData<T>
    {
        /// <summary>
        ///
        /// </summary>
        private static readonly float PropertySpacing = 10;


        /// <summary>
        ///
        /// </summary>
        /// <returns>
        ///
        /// </returns>
        protected static void Initialize() 
            => ProjectSettingsData<T>.OnRequestInstance = () => LoadOrSpawnSettings();


        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        private static T LoadOrSpawnSettings()
        {
            string path = GetAssetPath();

            var _instance = AssetDatabase.LoadAssetAtPath<T>(path);

            if (_instance == null)
            {
                _instance = ScriptableObject.CreateInstance<T>();

                string directory = Path.GetDirectoryName(path);

                if (!Directory.Exists(directory))
                    Directory.CreateDirectory(directory);

                AssetDatabase.CreateAsset(_instance, path);

                AssetDatabase.SaveAssets();
            }

            return _instance;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        private static string GetAssetPath() => $"Assets/Settings/DREditor/{typeof(T).Name}.asset";


        /// <summary>
        ///
        /// </summary>
        /// <returns>
        ///
        /// </returns>
        protected static SettingsProvider CreateSettingsProvider()
        {
            var settings = ProjectSettingsData<T>.Instance;

            SerializedObject _cachedSerializedObject = null;

            return new SettingsProvider(settings.SettingsPath, SettingsScope.Project)
            {
                label = settings.MenuLabel ?? settings.GetClassLabel(),
                guiHandler = (searchContext) =>
                {
                    if (_cachedSerializedObject == null || _cachedSerializedObject.targetObject == null)
                        _cachedSerializedObject = new SerializedObject(ProjectSettingsData<T>.Instance);

                    _cachedSerializedObject.Update();

                    EditorGUILayout.Space(PropertySpacing);

                    EditorGUI.BeginChangeCheck();

                    SerializedProperty prop = _cachedSerializedObject.GetIterator();

                    if (prop.NextVisible(true))
                    {
                        do// First do-while loop of my life.
                        {
                            if (prop.name == "m_Script") 
                                continue;

                            GUIContent label = new(prop.name);

                            float requiredWidth = EditorStyles.label.CalcSize(label).x + PropertySpacing;

                            float originalWidth = EditorGUIUtility.labelWidth;

                            EditorGUIUtility.labelWidth = requiredWidth;

                            EditorGUILayout.PropertyField(prop, true);

                            EditorGUIUtility.labelWidth = originalWidth;
                        }
                        while (prop.NextVisible(false));
                    }

                    if (EditorGUI.EndChangeCheck())
                    {
                        _cachedSerializedObject.ApplyModifiedProperties();

                        EditorUtility.SetDirty(ProjectSettingsData<T>.Instance);
                    }
                }
            };
        }
    }
}