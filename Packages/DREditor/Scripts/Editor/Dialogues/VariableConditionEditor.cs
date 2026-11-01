//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using DREditor.Dialogues;
using DREditor.Editor.Utility;
using DREditor.Progressing;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DREditor.Editor.Dialogues
{
    /// <summary>
    ///
    /// </summary>
    public class VariableConditionEditor : ProgressionContextEditorBase
    {
        /// <summary>
        ///
        /// </summary>
        protected VariableBase variableBase;

        /// <summary>
        ///
        /// </summary>
        protected Chapter chapter;

        /// <summary>
        ///
        /// </summary>
        protected Objective conditionObjective;

        /// <summary>
        ///
        /// </summary>
        protected string activelyDisplayedItemName;
        

        public VariableConditionEditor(ProgressionDatabase progression = null) : base(progression) { }


        /// <summary>
        ///
        /// </summary>
        public virtual void DisplayAddButton()
        {
            if (variableBase != null && !variableBase.Enabled && GUILayout.Button("Add Condition", GUILayout.Width(100)))
                variableBase.SetEnabled(true);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="variableBase"></param>
        /// <returns></returns>
        public virtual VariableBase Display(VariableBase variableBase)
        {
            this.variableBase = variableBase;

            DisplayCondition();

            return this.variableBase;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayCondition()
        {
            using (new EditorGUILayout.VerticalScope("Box", GUILayout.Width(400)))
                CustomLayoutUtility.DisplayVertical(DisplayVariableCondition);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayVariableCondition()
        {
            GUILayout.Label("Add Condition: ");

            variableBase.Chapter = HandyFields.IntField("Chapter: ", variableBase.Chapter);

            if (progression != null && progression.Chapters.Count != 0)
                OnProgressionValid();
            else
                GUILayout.Label("Set up a ProgressionDatabase or add some chapters to it " +
                            "to use ProgressionDatabase functionality.");
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnProgressionValid()
        {
            if (variableBase.Chapter < 0 || variableBase.Chapter > progression.Chapters.Count)
                variableBase.Chapter = 0;

            chapter = progression.Chapters[variableBase.Chapter];

            if (chapter.Objectives.Count > 0 && variableBase.Objective <= chapter.Objectives.Count)
                OnObjectivesValid();
            else
                OnObjectiveOutOfBounds();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnObjectivesValid()
        {
            DisplayObjectivePanel();

            if (ObjectiveIsEmpty())
                return;

            DisplayFlags(variableBase.Flags, "Flag");

            GUILayout.Space(5);

            DisplayFlags(variableBase.FalseFlags, "False Flag");
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayObjectivePanel()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                variableBase.Objective = EditorGUILayout.Popup(variableBase.Objective, chapter.GetObjectives());

                conditionObjective = chapter.Objectives[variableBase.Objective];

                if (GUILayout.Button("Add All Flags of Objective") && variableBase.Flags.Count == 0)
                    for (int i = 0; i < conditionObjective.Flags.Count; i++)
                        variableBase.Flags.Add(i);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool ObjectiveIsEmpty()
        {
            bool result = conditionObjective.Flags.Count == 0;

            if (result)
                GUILayout.Label("The Current Objective does not have any" +
                    " flags, add some to continue setup.");

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="items"></param>
        /// <param name="nameOfItem"></param>
        protected virtual void DisplayFlags(List<int> items, string nameOfItem)
        {
            activelyDisplayedItemName = nameOfItem;

            if (items.Count != 0 && conditionObjective.Flags.Count != 0)
                if (items.Count <= conditionObjective.Flags.Count)
                    DisplayFlagList(items);
                else
                    ClearListOnOutOfBounds(items, nameOfItem + "s", "index is greater than compared count.");

            HandyFields.AddItemButton(items, activelyDisplayedItemName);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="items"></param>
        protected virtual void DisplayFlagList(List<int> items)
        {
            string[] flagNames = conditionObjective.GetFlagNames();

            for (int i = 0; i < items.Count; i++)
            {
                if (items[i] > conditionObjective.Flags.Count)
                    items[i] = conditionObjective.Flags.Count - 1;

                using (new EditorGUILayout.HorizontalScope())
                {
                    items[i] = EditorGUILayout.Popup(items[i], flagNames);

                    HandyFields.RemoveItemButton(items, activelyDisplayedItemName, i);
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="items"></param>
        /// <param name="listName"></param>
        /// <param name="message"></param>
        protected virtual void ClearListOnOutOfBounds<T>(List<T> items, string listName = "A list", string message = "was out of bounds.")
        {
            UnityEngine.Debug.LogWarning("ALERT: " + listName + " " + message);

            items.Clear();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnObjectiveOutOfBounds()
        {
            GUILayout.Label("The set objective is either too high or too low so \n I'm setting it" +
                                " to the last available objective for the chapter, \n" +
                                "If you're still seeing this message it means the chapter has no \n" +
                                "objectives and needs to have one.");

            variableBase.Objective = chapter.Objectives.Count;
        }
    }
}