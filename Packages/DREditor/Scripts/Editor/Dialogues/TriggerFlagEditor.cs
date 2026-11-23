//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
#if UNITY_EDITOR
using DREditor.Dialogues;
using DREditor.Editor.Utility;
using DREditor.Progressing;
using UnityEditor;
using UnityEngine;

namespace DREditor.Editor.Dialogues
{
    [CustomClassEditor(typeof(TriggerFlag))]
    public class TriggerFlagEditor : ProgressionContextEditorBase<TriggerFlag>
    {
        protected Chapter chapter;

        protected Objective objective;

        protected string[] flagNames;

        protected int flagIndex;


        protected override void OnInspectorGUI()
        {
            if (ShouldBeDisplayed())
                CustomLayoutUtility.DisplayScoped(new EditorGUILayout.VerticalScope("Box", GUILayout.Width(400)),
                    DisplayTriggerFlagScoped);
        }

        protected virtual bool ShouldBeDisplayed()
        {
            bool result = progression != null && progression.Chapters.Count != 0;

            if (!result)
                GUILayout.Label("Set up a ProgressionDatabase or add some chapters to it " +
                            "to use ProgressionDatabase functionality.");

            return result;
        }


        protected virtual void DisplayTriggerFlagScoped() => CustomLayoutUtility.DisplayVertical(DisplayTriggerFlag);

        protected virtual void DisplayTriggerFlag()
        {
            DisplayChapter();

            if (ObjectiveValid())
                OnObjectiveValid();
            else
                OnObjectiveInvalid();
        }

        protected virtual void DisplayChapter()
        {
            target.Chapter = HandyFields.IntField("Chapter: ", target.Chapter, 200, 6, false);

            if (target.Chapter < 0 || target.Chapter > progression.Chapters.Count)
                target.Chapter = 0;

            chapter = progression.Chapters[target.Chapter];

            GUILayout.Space(10);
        }

        protected virtual bool ObjectiveValid() => chapter.Objectives.Count > 0 && target.Objective < chapter.Objectives.Count;

        protected virtual void OnObjectiveInvalid()
        {
            GUILayout.Label("The set objective is either too high or too low so " +
                    "\nI'm setting it to the last available objective for the chapter, " +
                    "\nIf you're still seeing this message it means the chapter has no " +
                    "\nobjectives and needs to have one, or try pressing the remove button and try again.");

            target.Objective = chapter.Objectives.Count - 1;
        }

        protected virtual void OnObjectiveValid()
        {
            DisplayObjectivePopup();

            if (ShouldDisplayFlags())
                DisplayFlags();

            DisplayAddFlagButton();
        }

        protected virtual void DisplayObjectivePopup()
        {
            target.SetObjective(EditorGUILayout.Popup(target.Objective, chapter.GetObjectives()));

            objective = chapter.Objectives[target.Objective];

            flagNames = objective.GetFlagNames();
        }

        protected virtual bool ShouldDisplayFlags() => target.Flags.Count != 0 && objective.Flags.Count != 0;

        protected virtual void DisplayFlags()
        {
            if (WithinObjectiveFlagCount())
                DisplayAssignedFlags();
            else
                ForceRemoveFlags();
        }

        protected virtual bool WithinObjectiveFlagCount() => target.Flags.Count <= objective.Flags.Count;

        protected virtual void DisplayAssignedFlags()
        {
            for (flagIndex = 0; flagIndex < target.Flags.Count; flagIndex++)
                CustomLayoutUtility.DisplayHorizontal(AssertValidFlagIndex, DisplayFlagPopup, DisplayRemoveFlagButton);
        }

        protected virtual void AssertValidFlagIndex()
        {
            if (target.Flags[flagIndex] > objective.Flags.Count)
                target.Flags[flagIndex] = objective.Flags.Count - 1;
        }

        protected virtual void DisplayFlagPopup() => 
            target.Flags[flagIndex] = EditorGUILayout.Popup(target.Flags[flagIndex], flagNames);

        protected virtual void DisplayRemoveFlagButton()
        {
            if (HandyFields.Button("Remove Flag"))
                target.Flags.Remove(target.Flags[flagIndex]);
        }

        protected virtual void ForceRemoveFlags() => target.Flags.Remove(target.Flags.Count - 1);

        protected virtual void DisplayAddFlagButton()
        {
            if (objective.Flags.Count == 0)
                GUILayout.Label("The Current Objective does not have any flags, add some to continue setup.");
            else if (HandyFields.Button("Add Flag"))
                target.Flags.Add(0);
        }
    }
}
#endif