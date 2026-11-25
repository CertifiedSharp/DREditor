//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using DREditor.Dialogues;
using DREditor.Editor.Utility;
using UnityEngine;

namespace DREditor.Editor.Dialogues
{
    /// <summary>
    /// Custom Class editor for the End Event <see cref="SceneTransition"/>.
    /// </summary>
    [CustomClassEditor(typeof(SceneTransition))]
    public class SceneTransitionEditor : ClassEditorTypeBase<SceneTransition>
    {
        /// <inheritdoc/>
        protected override void OnInspectorGUI() => CustomLayoutUtility.DisplayVertical("Box", DisplaySceneTransitionLayout);

        /// <summary>
        /// Displays the different fields for the scene transition vertically.
        /// </summary>
        protected virtual void DisplaySceneTransitionLayout() => CustomLayoutUtility.DisplayVertical(DisplaySceneTransition);

        /// <summary>
        /// Displays all the different fields for the scene transition.
        /// </summary>
        protected virtual void DisplaySceneTransition()
        {
            DisplayField<string>("Name of next Scene:", nameof(target.Scene), HandyFields.StringField);

            DisplayField<bool>("To Dark: ", nameof(target.ToDark), HandyFields.Option);

            DisplayField<bool>("To Menu: ", nameof(target.ToMenu), HandyFields.Option);

            DisplayField<bool>("OnLoadNoDark: ", nameof(target.OnLoadNoDark), HandyFields.Option);

            DisplayGate();

            DisplayField<string>("Move To Objective: ", nameof(target.MoveToObjective), HandyFields.StringField);

            DisplayField<bool>("To Next Natural: ", nameof(target.MoveToNextNaturalObjective), HandyFields.Option);
        }

        /// <summary>
        /// Displays the Gate to move to at the end of the dialogue.
        /// </summary>
        protected virtual void DisplayGate() => 
            target.AtEnd = HandyFields.UnityField(new GUIContent("Move To At End: ", "The Gate to move to at the end of the dialogue."),
                (Gates.Gate)target.AtEnd);
    }
}
#endif