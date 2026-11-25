//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
#if UNITY_EDITOR
using DREditor.Dialogues;
using DREditor.Editor.Utility;
using UnityEditor;
using UnityEngine;

namespace DREditor.Editor.Dialogues
{
    /// <summary>
    /// Custom class editor for the POCO End Event <see cref="StartInvestigation"/>.
    /// </summary>
    [CustomClassEditor(typeof(StartInvestigation))]
    public class PhaseChangeEditor : ClassEditorTypeBase<StartInvestigation>
    {
        protected override void OnInspectorGUI()
        {
            using (new EditorGUILayout.VerticalScope("Box", GUILayout.Width(400)))
                CustomLayoutUtility.DisplayHorizontal(DisplayPhaseChange);
        }

        /// <summary>
        /// Displays the details of the phase change.
        /// </summary>
        protected virtual void DisplayPhaseChange()
        {
            target.NextObjective = HandyFields.Option("Next Objective: ", target.NextObjective);

            target.NewDialogue = HandyFields.UnityField(target.NewDialogue, 120, 30);

            target.ID = HandyFields.IntField("ID: ", target.ID, 100);
        }
    }
}
#endif