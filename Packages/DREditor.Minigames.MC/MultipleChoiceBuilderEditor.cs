//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using DREditor.Editor.Utility;
using UnityEditor;
using UnityEngine;

namespace DREditor.Minigames.MC
{
    /// <summary>
    ///
    /// </summary>
    [CustomEditor(typeof(MultipleChoiceBuilder))]
    public class MultipleChoiceBuilderEditor : TimedMinigameEditor<MultipleChoiceBuilder>
    {
        /// <inheritdoc/>
        public override void OnInspectorGUI()
        {
            serializedObject.Update();

            CreateForm();

            EditorUtility.SetDirty(builder);

            serializedObject.ApplyModifiedProperties();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void CreateForm()
        {
            DisplayBuilderInformation();

            DisplayChoices();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayBuilderInformation()
        {
            builder.SetQuestion(HandyFields.StringArea("Question: ", builder.question));

            builder.SetDamageOnWrong(HandyFields.IntField("Damage on Wrong: ", builder.damageOnWrong, 25, 7));

            builder.SetDifficultyTimes(TimerDifficultyEditor.DisplaySettings(builder.times));

            using (new GUILayout.HorizontalScope())
                GUILayout.Label("", GUILayout.Width(120));
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayChoices()
        {
            for (int i = 0; i < builder.choices.Count; i++)
            {
                MultipleChoiceOptionEditor.Display(builder.choices[i]);

                if (GUILayout.Button("Remove Choice", GUILayout.Width(120)))
                    builder.choices.RemoveAt(i);

                using (new GUILayout.HorizontalScope())
                    GUILayout.Label("", GUILayout.Width(120));
            }

            using (new GUILayout.HorizontalScope())
                GUILayout.Label("", GUILayout.Width(120));

            if (GUILayout.Button("Add Choice", GUILayout.Width(120)))
                builder.choices.Add(new());
        }
    }
}
#endif