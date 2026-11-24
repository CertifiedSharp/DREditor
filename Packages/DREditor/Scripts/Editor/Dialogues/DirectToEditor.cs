//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
#if UNITY_EDITOR
using DREditor.Dialogues;
using UnityEditor;
using UnityEngine;

namespace DREditor.Editor.Dialogues
{
    /// <summary>
    /// 
    /// </summary>
    [CustomClassEditor(typeof(DirectTo))]
    public class DirectToEditor : ClassEditorTypeBase<DirectTo>
    {
        protected int value;

        protected Color background;

        protected override void OnInspectorGUI()
        {
            DisplayDirectTo();
        }

        protected virtual void DisplayDirectTo()
        {
            using (new EditorGUILayout.VerticalScope("Box", GUILayout.Width(400)))
            {
                using (new EditorGUILayout.HorizontalScope())
                {
                    GUILayout.Label("Next Dialogue:", GUILayout.Width(90));

                    background = GUI.backgroundColor;

                    if (target.NewDialogue == null)
                        GUI.backgroundColor = Color.cyan;

                    target.SetNewDialogue(
                        (DialogueCore)EditorGUILayout.ObjectField(target.NewDialogue, typeof(DialogueCore), false,
                            GUILayout.Width(180)));

                    GUI.backgroundColor = background;

                    if (target.NewDialogue != null)
                    {
                        string[] nextDialogueTexts = new string[target.NewDialogue.LineCount];

                        int[] optValues = new int[target.NewDialogue.LineCount];

                        for (int j = 0; j < target.NewDialogue.LineCount; j++)
                        {
                            nextDialogueTexts[j] = target.NewDialogue.GetLine(j).Text;
                            optValues[j] = value;
                            value++;
                        }

                        target.SetNewDialogueIndex(EditorGUILayout.IntPopup(
                            target.NewDialogueIndex, nextDialogueTexts, optValues, GUILayout.Width(100)));
                    }

                }

                value = 0;
            }
        }
    }
}
#endif