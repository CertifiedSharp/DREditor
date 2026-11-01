//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Utility;
using DREditor.Editor.Utility;
using UnityEditor;
using UnityEngine;

namespace DREditor.Minigames.Debates.Editor
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    ///
    /// </remarks>
    [System.Serializable]
    public class PhrasePresetEditor
    {
        /// <summary>
        ///
        /// </summary>
        protected PhrasePresetDatabase presetDatabase;

        /// <summary>
        ///
        /// </summary>
        protected int presetNum = 0;

        /// <summary>
        ///
        /// </summary>
        protected string presetName;

        /// <summary>
        ///
        /// </summary>
        protected bool initialized = false;

        /// <summary>
        ///
        /// </summary>
        protected PhraseBase activeInstance;


        /// <summary>
        ///
        /// </summary>
        public virtual void Initialize()
        {
            if (initialized)
                return;

            DatabaseUtility.InitializeDatabase(presetDatabase, out presetDatabase);

            initialized = true;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual bool DatabaseAvailable() => presetDatabase != null;

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual bool HasPresets() => DatabaseAvailable() && presetDatabase.Presets.Count != 0;


        /// <summary>
        ///
        /// </summary>
        /// <param name="activeInstance"></param>
        public virtual void DisplayPreset(PhraseBase activeInstance)
        {
            this.activeInstance = activeInstance;

            using (new GUILayout.VerticalScope("Box"))
            {
                DisplayDatabaseToolbar();

                if (presetDatabase != null)
                    DisplayPresetCreation();
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayDatabaseToolbar()
        {
            using (new GUILayout.HorizontalScope())
            {
                if (presetDatabase != null && presetDatabase.Presets.Count != 0)
                {
                    DisplayPresetDropdown();

                    DisplayPresetApplyButton();
                }

                DisplayDatabaseField();
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayPresetDropdown()
        {
            presetNum = EditorGUILayout.IntPopup(presetNum, presetDatabase.GetNames(), presetDatabase.GetInts(),
                    HandyFields.CalculateWidthOption(presetDatabase.GetNames()[presetNum]));
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayPresetApplyButton()
        {
            if (HandyFields.Button("Apply"))
                DisplayApplyOptions();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayApplyOptions()
        {
            Rect buttonRect = GUILayoutUtility.GetLastRect();

            buttonRect.x = Event.current.mousePosition.x;

            buttonRect.y = Event.current.mousePosition.y;

            GenericMenu menu = new();

            menu.AddItem(new GUIContent("Apply Modifications"), false, ApplyModifications);

            menu.AddItem(new GUIContent("Apply Animations"), false, ApplyAnimations);

            menu.DropDown(buttonRect);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ApplyModifications() => activeInstance.ApplyModifiers(GetSelectedPreset().modifiers);

        /// <summary>
        ///
        /// </summary>
        protected virtual void ApplyAnimations() => activeInstance.ApplyAnimations(GetSelectedPreset().animations);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual PhraseBase GetSelectedPreset() => presetDatabase.Presets[presetNum].preset.DeepCopy();

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayDatabaseField()
        {
            GUILayout.Label("Database:", HandyFields.CalculateWidthOption("Database:"));

            presetDatabase = HandyFields.UnityField(presetDatabase, 170, 20);
        }


        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayPresetCreation()
        {
            using (new GUILayout.HorizontalScope())
            {
                GUILayout.Label("New Preset Name:", GUILayout.MaxWidth(125));

                presetName = EditorGUILayout.TextField(presetName, GUILayout.MinWidth(50), HandyFields.CalculateWidthOption(presetName));

                if (HandyFields.Button("Create New Preset"))
                {
                    presetDatabase.Presets.Add(new PhrasePreset(presetName, activeInstance.DeepCopy()));

                    EditorUtility.SetDirty(presetDatabase);

                    UnityEngine.Debug.Log($"REPORT: Preset {presetName} created.");
                }
            }
        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="preset"></param>
        /// <returns></returns>
        public virtual bool DisplaySpawnPreset(out PhraseBase preset)
        {
            preset = null;

            if (presetDatabase.Presets.Count != 0)
                preset = DisplaySpawnToolbar();

            return preset != null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual PhraseBase DisplaySpawnToolbar()
        {
            DisplayPresetDropdown();

            return DisplaySpawnButton();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual PhraseBase DisplaySpawnButton()
        {
            if (HandyFields.Button("Add From Preset"))
                return GetSelectedPreset();
            else
                return null;
        }
    }
}