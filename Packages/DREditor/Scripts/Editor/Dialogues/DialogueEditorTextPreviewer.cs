//Author: Happy30
#if UNITY_EDITOR
using System.Linq;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace DREditor.Editor.Dialogues
{
    /// <summary>
    ///
    /// </summary>
    public static class DialogueEditorTextPreviewer
	{
        public static void TextPreview(string text, float width = 100, float height = 100)
        {
            GUIStyle style = new(GUI.skin.label)
            {
                richText = true,
                wordWrap = true
            };

            if (text.Contains("$"))
                text = $"{text}</color>";

            int c = text.Count(x => x == '@');

            var regex = new Regex(Regex.Escape("@"));

            for (int i = 0; i < c; i++)
            {
                if (i % 2 == 0)
                    text = regex.Replace(text, "<color=yellow>", 1);
                else
                    text = regex.Replace(text, "</color>", 1);
            }

            text = Regex.Replace(text, "\\$", "<color=cyan>");

            if (text == "")
                text = "<size=\"10\"><color=magenta>ANALYSIS: This line has no text. " +
                    "This will be skipped, and only the Dialogue Events in this line will be invoked." +
                    "You can ignore this if this is intended.</color></size>";

            EditorGUILayout.LabelField(new GUIContent(text, "Text Preview with formatting"), style, GUILayout.Height(height));
        }
    }
}
#endif