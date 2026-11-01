//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.IO;
using System.Linq;
using UnityEditor;

namespace DREditor.Editor.Code
{
    /// <summary>
    ///
    /// </summary>
    public static class TextFileUtility
    {
        /// <summary>
        ///
        /// </summary>
        [MenuItem("DREditor/Utility/Text Files/Trim Comments on Text File", false, 3)]
        public static void TrimCommentsFromFile()
        {
            string path = EditorUtility.OpenFilePanelWithFilters("Open dialogue text file", "", new string[] { "Text file", "txt" });

            if (path.Length != 0)
                File.WriteAllLines(path, RemoveCommentsFromText(File.ReadAllLines(path)));
            else
                UnityEngine.Debug.LogWarning(nameof(TextFileUtility) + ": No input file selected. Process aborted");

            UnityEngine.Debug.Log(nameof(TextFileUtility) + ": Tool Finished.");
        }

        /// <summary>
        ///
        /// </summary>
        [MenuItem("DREditor/Utility/Text Files/Trim Comments from clipboard", false, 3)]
        public static void TrimCommentsFromClipboard()
        {
            var removed = RemoveCommentsFromText(EditorGUIUtility.systemCopyBuffer.Split("\n"));

            EditorGUIUtility.systemCopyBuffer = string.Join("\n", removed);

            UnityEngine.Debug.Log(nameof(TextFileUtility) + ": Tool Finished.");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        public static string[] RemoveCommentsFromText(string[] lines)
        {
            var linesList = lines.ToList();

            for (int i = 0; i < linesList.Count; i++)
                if (linesList[i].StartsWith("//"))
                    linesList[i] = "\n";

            return linesList.ToArray();
        }
    }
}