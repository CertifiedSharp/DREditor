//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace DREditor.Editor
{
    /// <summary>
    ///
    /// </summary>
    public class NamespaceAdderWindow : EditorWindow
    {
        /// <summary>
        ///
        /// </summary>
        private string replaceNamespace = "DREditor.Editor.Utility.Editor";

        /// <summary>
        ///
        /// </summary>
        private string targetNamespace = "DREditor.Editor.Utility.Editor";

        /// <summary>
        ///
        /// </summary>
        private Object selectedObject;


        [MenuItem("DREditor/Coding/Namespace Adder")]
        public static void ShowWindow() => GetWindow<NamespaceAdderWindow>("Namespace Adder");

        protected virtual void OnGUI()
        {
            GUILayout.Label("Namespace Tools", EditorStyles.boldLabel);

            DisplayFields();

            DisplayButtons();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayFields()
        {
            replaceNamespace = EditorGUILayout.TextField("Target Replace Namespace", replaceNamespace);

            targetNamespace = EditorGUILayout.TextField("Target Namespace", targetNamespace);

            selectedObject = EditorGUILayout.ObjectField("Selected Object", selectedObject, typeof(Object), false);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayButtons()
        {
            if (GUILayout.Button("Apply Namespace"))
                ProcessFiles(replaceExisting: false, addUsing: false);

            if (GUILayout.Button("Replace Existing Namespace"))
                ProcessFiles(replaceExisting: true, addUsing: false);

            if (GUILayout.Button("Add Using Statement"))
                ProcessFiles(replaceExisting: false, addUsing: true);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="replaceExisting"></param>
        /// <param name="addUsing"></param>
        protected virtual void ProcessFiles(bool replaceExisting, bool addUsing)
        {
            if (selectedObject == null)
            {
                UnityEngine.Debug.LogError("Please select an object in the Project window.");

                return;
            }

            string folderPath = AssetDatabase.GetAssetPath(selectedObject);

            if (!Directory.Exists(folderPath))
            {
                UnityEngine.Debug.LogError("Selected object is not a folder.");

                return;
            }

            foreach (var filePath in Directory.GetFiles(folderPath, "*.cs", SearchOption.AllDirectories))
            {
                string content = File.ReadAllText(filePath);

                if (addUsing)
                {
                    string updated = AddUsingStatement(content);

                    if (updated != content)
                    {
                        File.WriteAllText(filePath, updated, System.Text.Encoding.UTF8);

                        UnityEngine.Debug.Log("Added using in: " + filePath);
                    }

                    continue;
                }

                bool hasNamespace = Regex.IsMatch(content, @"^\s*namespace\s+\S+", RegexOptions.Multiline);

                if (!hasNamespace || replaceExisting)
                {
                    string updated = AddOrReplaceNamespace(content, replaceExisting);

                    File.WriteAllText(filePath, updated, System.Text.Encoding.UTF8);

                    UnityEngine.Debug.Log((replaceExisting ? "Replaced" : "Applied") + " namespace in: " + filePath);
                }
            }

            AssetDatabase.Refresh();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        protected virtual string AddUsingStatement(string content)
        {
            content = content.Replace("\r\n", "\n").Replace("\n", "\r\n");

            if (Regex.IsMatch(content, $@"^\s*using\s+{Regex.Escape(targetNamespace)}\s*;", RegexOptions.Multiline))
                return content;

            var lines = content.Split(new[] { "\r\n" }, System.StringSplitOptions.None);

            int insertIndex = EvaluateInsertIndex(lines);

            var newLines = new string[lines.Length + 1];

            for (int i = 0; i < insertIndex; i++)
                newLines[i] = lines[i];

            newLines[insertIndex] = $"using {targetNamespace};";

            for (int i = insertIndex; i < lines.Length; i++)
                newLines[i + 1] = lines[i];

            return string.Join("\r\n", newLines);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="lines"></param>
        /// <returns></returns>
        protected virtual int EvaluateInsertIndex(string[] lines)
        {
            int insertIndex = 0;

            for (int i = 0; i < lines.Length; i++)
                if (lines[i].StartsWith("using "))
                    insertIndex = i + 1;
                else if (!string.IsNullOrWhiteSpace(lines[i]) && !lines[i].StartsWith("//"))
                    break;

            return insertIndex;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="content"></param>
        /// <param name="replace"></param>
        /// <returns></returns>
        protected virtual string AddOrReplaceNamespace(string content, bool replace = false)
        {
            content = content.Replace("\r\n", "\n").Replace("\n", "\r\n");

            string[] lines = content.Split(new[] { "\r\n" }, System.StringSplitOptions.None);

            string firstLine = lines[0].Trim();

            if (replace && Regex.IsMatch(content, @"namespace " + targetNamespace, RegexOptions.Multiline))
                return Regex.Replace(content, @"namespace " + targetNamespace, 
                    $"namespace {replaceNamespace}", RegexOptions.Multiline);

            if (Regex.IsMatch(content, @"^\s*namespace\s+\S+\s*;", RegexOptions.Multiline))
                return Regex.Replace(content, @"^\s*namespace\s+\S+\s*;", $"namespace {targetNamespace};", RegexOptions.Multiline);

            // Post 2025 rework, if this code is not needed, remove this commented code.
            //if (Regex.IsMatch(content, @"^\s*namespace\s+\S+\s*\{", RegexOptions.Multiline))
            //{
            //    content = Regex.Replace(content, @"^\s*namespace\s+\S+\s*\{", "", RegexOptions.Multiline);

            //    content = Regex.Replace(content, @"^\s*\}\s*$", "", RegexOptions.Multiline);
            //}

            string code = string.Join("\r\n", lines);

            if (firstLine.StartsWith("//") || firstLine.StartsWith("/*") || firstLine.StartsWith("*"))
            {
                string comment = lines[0] + "\r\n";

                code = string.Join("\r\n", lines, 1, lines.Length - 1);

                return comment + $"namespace {targetNamespace}\r\n{{\r\n{IndentCode(code)}\r\n}}";
            }

            return $"namespace {targetNamespace}\r\n{{\r\n{IndentCode(code)}\r\n}}";
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="code"></param>
        /// <returns></returns>
        protected virtual string IndentCode(string code)
        {
            var lines = code.Split(new[] { "\r\n" }, System.StringSplitOptions.None);

            string indent = "    ";

            string result = "";

            foreach (var line in lines)
                if (string.IsNullOrWhiteSpace(line))
                    result += System.Environment.NewLine;
                else
                    result += indent + line + System.Environment.NewLine;

            return result;
        }
    }
}