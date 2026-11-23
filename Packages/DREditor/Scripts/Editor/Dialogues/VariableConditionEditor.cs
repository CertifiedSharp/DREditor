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
    public class VariableConditionEditor : ProgressionContextEditorBase<VariableBase>
    {
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
        

        /// <summary>
        ///
        /// </summary>
        /// <param name="variableBase"></param>
        /// <returns></returns>
        public virtual VariableBase Display(VariableBase variableBase)
        {
            target = variableBase;

            OnInspectorGUI();

            return target;
        }

        protected override void OnInspectorGUI()
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

            target.Chapter = HandyFields.IntField("Chapter: ", target.Chapter, 200, 6, false);

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
            if (target.Chapter < 0 || target.Chapter > progression.Chapters.Count)
                target.Chapter = 0;

            chapter = progression.Chapters[target.Chapter];

            if (chapter.Objectives.Count > 0 && target.Objective <= chapter.Objectives.Count)
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

            DisplayFlags(target.Flags, "Flag");

            GUILayout.Space(5);

            DisplayFlags(target.FalseFlags, "False Flag");
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayObjectivePanel()
        {
            using (new EditorGUILayout.HorizontalScope())
            {
                target.Objective = EditorGUILayout.Popup(target.Objective, chapter.GetObjectives());

                conditionObjective = chapter.Objectives[target.Objective];

                if (GUILayout.Button("Add All Flags of Objective") && target.Flags.Count == 0)
                    for (int i = 0; i < conditionObjective.Flags.Count; i++)
                        target.Flags.Add(i);
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

            target.Objective = chapter.Objectives.Count;
        }
    }
}