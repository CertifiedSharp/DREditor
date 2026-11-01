//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using System;
using System.IO;
using System.Text;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace DREditor.Editor.Code
{
    /// <summary>
    ///
    /// </summary>
    public static class TooltipToXmlConverter
    {
        /// <summary>
        ///
        /// </summary>
        private static readonly RegexOptions Options = RegexOptions.Compiled | RegexOptions.Singleline;

        /// <summary>
        ///
        /// </summary>
        private static readonly string TooltipRegexString = @"\[Tooltip\s*\(((?:[^()]|\((?:[^()]|\([^()]*\))*\))*)\)\]";

        /// <summary>
        ///
        /// </summary>
        private static readonly Regex TooltipRegex = new(TooltipRegexString, Options);

        /// <summary>
        ///
        /// </summary>
        private static readonly string NameofRegexString = @"@?""([^""]*)""|nameof\s*\(\s*([^)]+)\s*\)";

        /// <summary>
        ///
        /// </summary>
        private static readonly Regex NameofRegex = new(NameofRegexString, Options);

        /// <summary>
        ///
        /// </summary>
        private static readonly string SummaryTag = "<summary>";

        /// <summary>
        ///
        /// </summary>
        private static readonly string SummaryEndTag = "</summary>";


        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        [MenuItem("DREditor/Coding/Documentation/Apply XML Summaries to Script(s) Tooltips", true)]
        public static bool ConvertSelectedScriptsValidate()
        {
            foreach (string guid in Selection.assetGUIDs)
                if (IsValidScriptSelected(AssetDatabase.GUIDToAssetPath(guid)))
                    return true;

            return false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="path"></param>
        /// <returns></returns>
        private static bool IsValidScriptSelected(string path) =>
            !string.IsNullOrEmpty(path) && path.EndsWith(".cs", StringComparison.OrdinalIgnoreCase);


        /// <summary>
        ///
        /// </summary>
        [MenuItem("DREditor/Coding/Documentation/Apply XML Summaries to Script(s) Tooltips", false, 20)]
        public static void ConvertSelectedScripts()
        {
            string[] guids = Selection.assetGUIDs;

            int filesProcessed = 0;

            foreach (string guid in guids)
                ConvertedScriptFromGUID(guid, ref filesProcessed);

            UnityEngine.Debug.Log($"REPORT: Successfully processed {filesProcessed} script(s).");

            if (filesProcessed > 0)
                AssetDatabase.Refresh();
            else
                UnityEngine.Debug.LogWarning("REPORT: No valid C# scripts were processed or modified.");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="guid"></param>
        /// <param name="filesProcessed"></param>
        private static void ConvertedScriptFromGUID(string guid, ref int filesProcessed)
        {
            string assetPath = AssetDatabase.GUIDToAssetPath(guid);

            if (!IsValidScriptSelected(assetPath))
                return;

            string fullPath = Path.GetFullPath(Path.Combine(Application.dataPath, "..", assetPath));

            if (ProcessScriptFile(fullPath))
                filesProcessed++;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        private static bool ProcessScriptFile(string filePath)
        {
            string fileText = File.ReadAllText(filePath);

            MatchCollection matches = TooltipRegex.Matches(fileText);

            if (matches.Count == 0)
                return false;

            StringBuilder updatedCode = new();

            int lastIndex = 0;

            bool isModified = false;

            foreach (Match match in matches)
                if (TryUpdateMatchedTooltips(fileText, updatedCode, match, lastIndex, out lastIndex))
                    isModified = true;

            if (isModified)
            {
                updatedCode.Append(fileText[lastIndex..]);

                File.WriteAllText(filePath, updatedCode.ToString().TrimEnd(), Encoding.UTF8);
            }

            return isModified;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="fileText"></param>
        /// <param name="updatedCode"></param>
        /// <param name="match"></param>
        /// <param name="lastIndex"></param>
        /// <param name="outLastIndex"></param>
        /// <returns></returns>
        private static bool TryUpdateMatchedTooltips(string fileText, StringBuilder updatedCode, Match match, int lastIndex,
            out int outLastIndex)
        {
            outLastIndex = lastIndex;

            int topAttributeIndex = FindTopmostAttributeStart(fileText, match.Index);

            if (HasExistingSummary(fileText, topAttributeIndex) ||
                !TryBuildSummary(match, fileText, topAttributeIndex,
                out string xmlSummaryContent, out int lineStart, out string indentation))
                return false;

            updatedCode.Append(fileText[lastIndex..lineStart]);

            updatedCode.AppendLine($"{indentation}/// {SummaryTag}");

            updatedCode.Append(xmlSummaryContent);

            updatedCode.AppendLine($"{indentation}/// {SummaryEndTag}");

            outLastIndex = lineStart;

            return true;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="fileText"></param>
        /// <param name="attributeIndex"></param>
        /// <returns></returns>
        private static int FindTopmostAttributeStart(string fileText, int attributeIndex)
        {
            int topmostStart = attributeIndex;

            int checkPos = attributeIndex - 1;

            while (checkPos >= 0)
            {
                char c = fileText[checkPos];

                if (char.IsWhiteSpace(c))
                {
                    checkPos--;

                    continue;
                }

                if (c == ']')
                {
                    int openBracketPos = fileText.LastIndexOf('[', checkPos);

                    if (openBracketPos != -1)
                    {
                        topmostStart = openBracketPos;

                        checkPos = openBracketPos - 1;

                        continue;
                    }
                }

                break;
            }

            return topmostStart;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="fileText"></param>
        /// <param name="checkPos"></param>
        /// <returns></returns>
        private static bool HasExistingSummary(string fileText, int checkPos)
        {
            checkPos--;

            while (checkPos >= 0)
            {
                char c = fileText[checkPos];

                if (char.IsWhiteSpace(c))
                {
                    checkPos--;

                    continue;
                }

                int summaryLength = SummaryEndTag.Length;

                if (checkPos >= summaryLength - 1)
                {
                    string potentialEndTag = fileText.Substring(checkPos - summaryLength + 1, summaryLength);

                    if (potentialEndTag == SummaryEndTag)
                        return true;
                }

                break;
            }

            return false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="match"></param>
        /// <param name="fileText"></param>
        /// <param name="topAttributeIndex"></param>
        /// <param name="xmlSummaryContent"></param>
        /// <param name="lineStart"></param>
        /// <param name="indentation"></param>
        /// <returns></returns>
        private static bool TryBuildSummary(Match match, string fileText, int topAttributeIndex,
            out string xmlSummaryContent, out int lineStart, out string indentation)
        {
            lineStart = fileText.LastIndexOf('\n', topAttributeIndex);

            lineStart = (lineStart == -1) ? 0 : lineStart + 1;

            string linePrefix = fileText[lineStart..topAttributeIndex];

            indentation = Regex.Match(linePrefix, @"^\s*").Value;

            string rawArguments = match.Groups[1].Value.Trim();

            xmlSummaryContent = BuildTooltipSummary(rawArguments, indentation);

            return !string.IsNullOrEmpty(xmlSummaryContent);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="rawArguments"></param>
        /// <param name="indentation"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static string BuildTooltipSummary(string rawArguments, string indentation)
        {
            StringBuilder builder = new();

            MatchCollection matches = NameofRegex.Matches(rawArguments);

            if (matches.Count == 0)
                return string.Empty;

            builder.Append($"{indentation}/// ");

            for (int i = 0; i < matches.Count; i++)
            {
                Match match = matches[i];

                string rawValue = match.Groups[1].Success ? match.Groups[1].Value : match.Groups[2].Value;

                string sanitizedValue = rawValue.Replace("\\n", "").Replace("\\r", "");

                string segment = GetAppendString(sanitizedValue, match.Groups[1].Success);

                builder.Append(segment);

                if (i < matches.Count - 1)
                {
                    int currentEnd = match.Index + match.Length;

                    int nextStart = matches[i + 1].Index;

                    string gapText = rawArguments[currentEnd..nextStart];

                    if (gapText.Contains('\n') || gapText.Contains('\r'))
                    {
                        builder.AppendLine(" <br/>");

                        builder.Append($"{indentation}/// ");
                    }
                }
            }

            builder.AppendLine();

            return builder.ToString();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="appendString"></param>
        /// <param name="notReference"></param>
        /// <returns></returns>
        private static string GetAppendString(string appendString, bool notReference) =>
            notReference ? appendString : $"<see cref=\"{appendString.Trim()}\"/>";
    }
}