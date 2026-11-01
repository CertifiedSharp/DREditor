//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections.Generic;
using System.IO;
using UnityEditor;

namespace DREditor.Editor.Code
{
    /// <summary>
    ///
    /// </summary>
    public static class CodingUtility
    {
        /// <summary>
        ///
        /// </summary>
        [MenuItem("DREditor/Coding/Create Nameof from clipboard")]
        public static void CreateNameOfString()
        {
            string output = "";

            var inheritedClasses = EditorGUIUtility.systemCopyBuffer.Split(',');

            for (int i = 0; i < inheritedClasses.Length; i++)
                if (i == inheritedClasses.Length - 1)
                    output += "nameof(" + inheritedClasses[i] + ") +";
                else
                    output += "nameof(" + inheritedClasses[i] + ") + \", \" + ";

            EditorGUIUtility.systemCopyBuffer = output;

            UnityEngine.Debug.Log("Finished Process: " + output);
        }

        /// <summary>
        ///
        /// </summary>
        [MenuItem("DREditor/Coding/Clipboard Add Tooltips")]
        public static void FindToolTip()
        {
            string input = EditorGUIUtility.systemCopyBuffer;

            string output = "";

            using (StringReader stringReader = new(input))
            {
                while (stringReader.ReadLine() is string line && line != null)
                {
                    if (line != "")
                    {
                        if (line.Contains("[SerializeField]") || line.Contains("public "))
                            line = "[Tooltip(\"\")]" + "\n" + line;

                        output += "\n" + line + "\n";
                    }
                    else
                        output += line;
                }
            }

            EditorGUIUtility.systemCopyBuffer = output;
        }

        /// <summary>
        ///
        /// </summary>
        [MenuItem("DREditor/Coding/Log Scripts in Selected Folders")]
        public static void LogScriptsInSelectedFolders() => LogScriptsInSelected(SearchOption.AllDirectories);

        /// <summary>
        ///
        /// </summary>
        [MenuItem("DREditor/Coding/Log Selected Scripts in Single Folder")]
        public static void LogScriptsInSelectedFolder() => LogScriptsInSelected(SearchOption.TopDirectoryOnly);

        /// <summary>
        ///
        /// </summary>
        /// <param name="searchOption"></param>
        private static void LogScriptsInSelected(SearchOption searchOption)
        {
            UnityEngine.Object selectedObject = Selection.activeObject;

            if (selectedObject == null)
            {
                UnityEngine.Debug.LogError("No folder selected. Please select a folder in the Project window.");

                return;
            }

            string path = AssetDatabase.GetAssetPath(selectedObject);

            if (!AssetDatabase.IsValidFolder(path))
            {
                UnityEngine.Debug.LogError("Selected object is not a folder.");

                return;
            }

            List<string> scriptNames = new();

            string[] files = Directory.GetFiles(path, "*.cs", searchOption);

            foreach (string file in files)
            {
                string scriptName = Path.GetFileNameWithoutExtension(file);

                scriptNames.Add(scriptName);
            }

            foreach (string script in scriptNames)
                UnityEngine.Debug.Log("Script found: " + script);

            UnityEngine.Debug.Log("Total Scripts Found: " + scriptNames.Count);

            scriptNames.Sort();

            EditorGUIUtility.systemCopyBuffer = scriptNames.Count + "\n" + string.Join("\n", scriptNames);

            UnityEngine.Debug.Log("Copied script names to clipboard.");
        }
    }
}