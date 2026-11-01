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
    [System.Serializable]
    public static class MultipleChoiceOptionEditor
    {
        /// <summary>
        ///
        /// </summary>
        public static void Display(MultipleChoiceOption option)
        {
            using (new GUILayout.HorizontalScope())
            {
                option.SetText(HandyFields.StringField("Choice Text: ", option.text));

                GUILayout.Space(5);

                EditorGUILayout.LabelField("Is Answer: ", GUILayout.Width(70));

                option.SetIsAnswer(EditorGUILayout.Toggle(option.isAnswer));
            }

            if (!option.isAnswer)
            {
                GUILayout.Space(5);

                using (new GUILayout.HorizontalScope())
                {
                    GUILayout.Label("On Wrong Dialogue: ", GUILayout.Width(120));

                    option.SetWrongDialogue(HandyFields.UnityField(option.wrongDialogue, 170, 25));
                }
            }
            else if (option.wrongDialogue != null)
                option.SetWrongDialogue(null);

            GUILayout.Space(20);
        }
    }

}
#endif