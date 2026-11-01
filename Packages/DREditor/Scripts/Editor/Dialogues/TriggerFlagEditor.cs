//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
#if UNITY_EDITOR
using DREditor.Dialogues;
using DREditor.Editor.Utility;
using DREditor.Progressing;
using UnityEditor;
using UnityEngine;

namespace DREditor.Editor.Dialogues
{
    public class TriggerFlagEditor : ProgressionContextEditorBase
    {
        protected TriggerFlag FlagTrigger;

        protected Chapter chapter;

        protected Objective objective;

        protected string[] flagNames;

        protected int flagIndex;

        public TriggerFlagEditor(ProgressionDatabase progression) : base(progression) { }

        public virtual void DisplayAddButton()
        {
            if (FlagTrigger != null && !FlagTrigger.Enabled && HandyFields.Button("Trigger Flag"))
                FlagTrigger.SetEnabled(true);
        }

        public virtual TriggerFlag Display(TriggerFlag FlagTrigger)
        {
            this.FlagTrigger = FlagTrigger;

            if (ShouldBeDisplayed())
                CustomLayoutUtility.DisplayScoped(new EditorGUILayout.VerticalScope("Box", GUILayout.Width(400)),
                    DisplayTriggerFlagScoped);

            return FlagTrigger;
        }

        protected virtual bool ShouldBeDisplayed()
        {
            bool result = FlagTrigger.Enabled && progression != null && progression.Chapters.Count != 0;

            if (FlagTrigger.Enabled && !result)
                GUILayout.Label("Set up a ProgressionDatabase or add some chapters to it " +
                            "to use ProgressionDatabase functionality.");

            return result;
        }


        protected virtual void DisplayTriggerFlagScoped()
        {
            CustomLayoutUtility.DisplayVertical(DisplayTriggerFlag);

            DisplayRemoveButton();
        }

        protected virtual void DisplayTriggerFlag()
        {
            GUILayout.Label("Trigger Flag: ");

            DisplayChapter();

            if (ObjectiveValid())
                OnObjectiveValid();
            else
                OnObjectiveInvalid();
        }

        protected virtual void DisplayChapter()
        {
            FlagTrigger.Chapter = HandyFields.IntField("Chapter: ", FlagTrigger.Chapter);

            if (FlagTrigger.Chapter < 0 || FlagTrigger.Chapter > progression.Chapters.Count)
                FlagTrigger.Chapter = 0;

            chapter = progression.Chapters[FlagTrigger.Chapter];
        }

        protected virtual bool ObjectiveValid() => chapter.Objectives.Count > 0 && FlagTrigger.Objective < chapter.Objectives.Count;

        protected virtual void OnObjectiveInvalid()
        {
            GUILayout.Label("The set objective is either too high or too low so " +
                    "\nI'm setting it to the last available objective for the chapter, " +
                    "\nIf you're still seeing this message it means the chapter has no " +
                    "\nobjectives and needs to have one, or try pressing the remove button and try again.");

            FlagTrigger.Objective = chapter.Objectives.Count - 1;
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
            FlagTrigger.SetObjective(EditorGUILayout.Popup(FlagTrigger.Objective, chapter.GetObjectives()));

            objective = chapter.Objectives[FlagTrigger.Objective];

            flagNames = objective.GetFlagNames();
        }

        protected virtual bool ShouldDisplayFlags() => FlagTrigger.Flags.Count != 0 && objective.Flags.Count != 0;

        protected virtual void DisplayFlags()
        {
            if (WithinObjectiveFlagCount())
                DisplayAssignedFlags();
            else
                ForceRemoveFlags();
        }

        protected virtual bool WithinObjectiveFlagCount() => FlagTrigger.Flags.Count <= objective.Flags.Count;

        protected virtual void DisplayAssignedFlags()
        {
            for (flagIndex = 0; flagIndex < FlagTrigger.Flags.Count; flagIndex++)
                CustomLayoutUtility.DisplayHorizontal(AssertValidFlagIndex, DisplayFlagPopup, DisplayRemoveFlagButton);
        }

        protected virtual void AssertValidFlagIndex()
        {
            if (FlagTrigger.Flags[flagIndex] > objective.Flags.Count)
                FlagTrigger.Flags[flagIndex] = objective.Flags.Count - 1;
        }

        protected virtual void DisplayFlagPopup() => 
            FlagTrigger.Flags[flagIndex] = EditorGUILayout.Popup(FlagTrigger.Flags[flagIndex], flagNames);

        protected virtual void DisplayRemoveFlagButton()
        {
            if (HandyFields.Button("Remove Flag"))
                FlagTrigger.Flags.Remove(FlagTrigger.Flags[flagIndex]);
        }

        protected virtual void ForceRemoveFlags() => FlagTrigger.Flags.Remove(FlagTrigger.Flags.Count - 1);

        protected virtual void DisplayAddFlagButton()
        {
            if (objective.Flags.Count == 0)
                GUILayout.Label("The Current Objective does not have any flags, add some to continue setup.");
            else if (HandyFields.Button("Add Flag"))
                FlagTrigger.Flags.Add(0);
        }

        protected virtual void DisplayRemoveButton()
        {
            if (HandyFields.Button("Remove"))
            {
                FlagTrigger.SetEnabled(false);

                FlagTrigger.SetObjective(0);

                FlagTrigger.Flags.Clear();
            }
        }
    }
}
#endif