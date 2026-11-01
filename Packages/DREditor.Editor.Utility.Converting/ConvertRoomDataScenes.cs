//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.RoomBuilding;
using DREditor.RoomBuilding.Old;
using System.Collections.Generic;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

namespace DREditor.Editor.Utility.Converting
{
    /// <summary>
    ///
    /// </summary>
    public class ConvertRoomDataScenes : ScriptableWizard
    {
        // Get the asset of the RoomStateManager Prefab

        // Get all valid scenes guids
        // Get all valid scene assets
        // Get the paths to each valid scene
        // Open each scene 

        // If that scene contains a RoomManager, 
        // Then 
        // Instantiate the prefab in the scene
        // Select Prefab
        // Delete the RoomManager GameObject
        // Set the scene dirty and save
        // ELSE
        // Move to the next scene

        /// <summary>
        ///
        /// </summary>
        static readonly List<RoomStateBuilder> Builders = new();

        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Tooltip("")]
        [SerializeField] protected GameObject roomStateManagerPrefab = null;


        [MenuItem("DREditor/Convert/Convert Scene Room Managers")]
        public static void CreateWizard() => DisplayWizard<ConvertRoomDataScenes>("Convert Room Data", "Convert");

        protected virtual void OnWizardCreate()
        {
            if (EditorUtility.DisplayDialog("Confirm", "You MUST convert the Room Data Assets FIRST." +
                "\nYou can do this by calling: DREditor/Convert/Convert All Room Data Builders." +
                "\nHave you done this?", "Yes", "No"))
                ConvertSceneRoomManagers();
        }


        /// <summary>
        ///
        /// </summary>
        protected virtual void ConvertSceneRoomManagers()
        {
            InitializeBuilders();

            ConvertAllValidScenes();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeBuilders()
        {
            Builders.Clear();

            var found = AssetDatabase.FindAssets("t:RoomStateBuilder");

            if (found.Length != 0)
                foreach (var asset in found)
                {
                    string path = AssetDatabase.GUIDToAssetPath(asset);

                    var roomStateBuilder = AssetDatabase.LoadAssetAtPath<RoomStateBuilder>(path);

                    Builders.Add(roomStateBuilder);
                }
        }


        /// <summary>
        ///
        /// </summary>
        protected virtual void ConvertAllValidScenes()
        {
            var scenes = CustomEditorUtility.GetAllValidSceneAssets();

            foreach (var scene in scenes)
                ConvertScene(scene);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="scene"></param>
        protected virtual void ConvertScene(SceneAsset scene)
        {
            try
            {
                CustomEditorUtility.LoadScene(scene);
            }
            catch
            {
                UnityEngine.Debug.LogWarning("Had trouble opening scene: " + scene.name + " , Skipping conversion.");

                return;
            }

            ExecuteRoomConvert();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ExecuteRoomConvert()
        {
            var roomManagers = MonoBehaviour.FindObjectsByType<RoomManager>();

            if (roomManagers.Length > 0 && HasExistingBuilder(out RoomStateBuilder builder))
            {
                var managerObject = (GameObject)PrefabUtility.InstantiatePrefab(roomStateManagerPrefab);

                var manager = managerObject.GetComponent<RoomStateManager>();

                manager.SetBuilder(builder);

                EditorUtility.SetDirty(manager);

                MonoBehaviour.DestroyImmediate(roomManagers[0].gameObject);

                EditorUtility.SetDirty(managerObject);

                EditorSceneManager.SaveScene(EditorSceneManager.GetActiveScene());
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="builder"></param>
        /// <returns></returns>
        protected virtual bool HasExistingBuilder(out RoomStateBuilder builder)
        {
            builder = null;

            if (Builders.Count != 0)
            {
                foreach (var roomStateBuilder in Builders)
                {
                    if (roomStateBuilder != null && roomStateBuilder.name == EditorSceneManager.GetActiveScene().name)
                    {
                        builder = roomStateBuilder;

                        return true;
                    }
                }
            }

            return false;
        }
    }
}