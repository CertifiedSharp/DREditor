//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text.RegularExpressions;
using UnityEditor;

namespace DREditor.Editor.Code
{
    /// <summary>
    ///
    /// </summary>
    public static class InheritDocProcessor
    {
        /// <summary>
        ///
        /// </summary>
        private static readonly string IndentKeyword = @"indent";

        /// <summary>
        ///
        /// </summary>
        private static readonly string SignatureKeyword = @"signature";

        /// <summary>
        ///
        /// </summary>
        private static readonly string InheritDoc = "/// <inheritdoc/>";

        /// <summary>
        ///
        /// </summary>
        private static readonly BindingFlags CheckedFlags = 
            BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.DeclaredOnly;


        /// <summary>
        ///
        /// </summary>
        [MenuItem("DREditor/Coding/Documentation/Apply Inherit Docs to Selected", priority = 30)]
        public static void ProcessSelectedScripts()
        {
            if (TryGetScripts(out MonoScript[] selectedScripts) && ProcessedScripts(selectedScripts) > 0)
                AssetDatabase.Refresh();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="selectedScripts"></param>
        /// <returns></returns>
        private static bool TryGetScripts(out MonoScript[] selectedScripts)
        {
            selectedScripts = Selection.objects.OfType<MonoScript>().ToArray();

            bool result = selectedScripts.Length != 0;

            if (!result)
                UnityEngine.Debug.LogWarning("ALERT: No scripts selected. PROPOSAL: Highlight target files in the Project window.");

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="selectedScripts"></param>
        /// <returns></returns>
        private static int ProcessedScripts(MonoScript[] selectedScripts)
        {
            int modifiedFilesCount = 0;

            foreach (var script in selectedScripts)
                if (FileModified(script))
                    modifiedFilesCount++;

            UnityEngine.Debug.Log($"REPORT: Finished processing. Modified {modifiedFilesCount} script(s).");

            return modifiedFilesCount;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="script"></param>
        /// <returns></returns>
        private static bool FileModified(MonoScript script) =>
            TryGetClass(script, out Type scriptType) &&
            TryGetFullPath(script, out string fullPath) &&
            SourceCodeUpdated(scriptType, fullPath);

        /// <summary>
        ///
        /// </summary>
        /// <param name="script"></param>
        /// <param name="scriptType"></param>
        /// <returns></returns>
        private static bool TryGetClass(MonoScript script, out Type scriptType)
        {
            scriptType = script.GetClass();

            return scriptType != null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="script"></param>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        private static bool TryGetFullPath(MonoScript script, out string fullPath)
        {
            string assetPath = AssetDatabase.GetAssetPath(script);

            fullPath = Path.GetFullPath(assetPath);

            return File.Exists(fullPath);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="scriptType"></param>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        private static bool SourceCodeUpdated(Type scriptType, string fullPath)
        {
            string sourceCode = File.ReadAllText(fullPath);

            bool fileChanged = false;

            ProcessInterfaceMembers(scriptType, ref sourceCode, ref fileChanged);

            ProcessBaseClassOverrides(scriptType, ref sourceCode, ref fileChanged);

            if (fileChanged)
                File.WriteAllText(fullPath, sourceCode);

            return fileChanged;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="scriptType"></param>
        /// <param name="sourceCode"></param>
        /// <param name="fileChanged"></param>
        private static void ProcessInterfaceMembers(Type scriptType, ref string sourceCode, ref bool fileChanged)
        {
            Type[] implementedInterfaces = scriptType.GetInterfaces();

            foreach (Type iface in implementedInterfaces)
            {
                var members = iface
                    .GetMethods(CheckedFlags)
                    .Where(methodInfo => !methodInfo.IsSpecialName)
                    .Cast<MemberInfo>()
                    .Concat(iface.GetProperties(CheckedFlags));

                foreach (MemberInfo member in members)
                    if (TryApplyInheritDoc(ref sourceCode, member.Name, $"{iface.Name}.{member.Name}"))
                        fileChanged = true;
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="scriptType"></param>
        /// <param name="sourceCode"></param>
        /// <param name="fileChanged"></param>
        private static void ProcessBaseClassOverrides(Type scriptType, ref string sourceCode, ref bool fileChanged)
        {
            HashSet<string> processedMemberNames = new();

            IEnumerable<MemberInfo> overriddenMethods = scriptType.GetMethods(CheckedFlags)
                .Where(m => !m.IsSpecialName && m.IsVirtual && m.GetBaseDefinition().DeclaringType != scriptType);

            ApplyOverrides(ref overriddenMethods, ref processedMemberNames, ref sourceCode, ref fileChanged);

            IEnumerable<MemberInfo> overriddenProperties = scriptType.GetProperties(CheckedFlags)
                .Where(p =>
                {
                    var accessor = p.GetGetMethod(true) ?? p.GetSetMethod(true);

                    return accessor != null && accessor.IsVirtual && accessor.GetBaseDefinition().DeclaringType != scriptType;
                });

            ApplyOverrides(ref overriddenProperties, ref processedMemberNames, ref sourceCode, ref fileChanged);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="collection"></param>
        /// <param name="processedMemberNames"></param>
        /// <param name="sourceCode"></param>
        /// <param name="fileChanged"></param>
        /// <remarks></remarks>
        private static void ApplyOverrides(ref IEnumerable<MemberInfo> collection, ref HashSet<string> processedMemberNames, 
            ref string sourceCode, ref bool fileChanged)
        {
            foreach (var member in collection)
                if (processedMemberNames.Add(member.Name))
                    if (TryApplyInheritDoc(ref sourceCode, member.Name, string.Empty))
                        fileChanged = true;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="code"></param>
        /// <param name="implicitName"></param>
        /// <param name="explicitName"></param>
        /// <returns></returns>
        private static bool TryApplyInheritDoc(ref string code, string implicitName, string explicitName) =>
            GetRegex(implicitName, explicitName) is Regex regex && regex.Match(code).Success &&
            UpdatedCodeApplied(ref code, GetUpdatedCode(ref code, regex, implicitName));

        /// <summary>
        ///
        /// </summary>
        /// <param name="implicitName"></param>
        /// <param name="explicitName"></param>
        /// <returns></returns>
        private static Regex GetRegex(string implicitName, string explicitName) =>
            new(GetFunctionPattern(implicitName, explicitName), RegexOptions.Multiline);

        /// <summary>
        ///
        /// </summary>
        /// <param name="implicitName"></param>
        /// <param name="explicitName"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        private static string GetFunctionPattern(string implicitName, string explicitName)
        {
            // Must be the very first piece so Multiline mode works properly
            string lineStart = @"^";

            // Placed right after the line start to check the preceding line
            string noExistingXmlDocs = @"(?<!\/\/\/.*?\r?\n[ \t]*)";

            // Capture ALL indentation spaces/tabs at the start of this line
            string findIndentation = @"(?<" + IndentKeyword + @">[ \t]*)";

            // Requires an access modifier (public, protected, private, internal) or override modifier near the start
            string requiredAccessModifiers = @"(?=(?:public|protected|private|internal|override|virtual)\b)";

            // Match the signature keywords up to the member name
            string matchSignatureKeywords = @"(?<" + SignatureKeyword + @">" + requiredAccessModifiers + @"[a-zA-Z0-9_\<\>][^\r\n]*?)";

            // Matches through interfaceFunctionName or interfaceName.interfaceFunctionName
            string targetNames = string.IsNullOrEmpty(explicitName)
                ? Regex.Escape(implicitName)
                : $"(?:{Regex.Escape(implicitName)}|{Regex.Escape(explicitName)})";

            // Matches using above.
            string matchMemberName = @"[ \t]+" + targetNames;

            // Must be followed immediately by a method parenthesis '(' or generic bracket '<'
            string validMethodFollowUp = @"(?=[\(\<\{=]|\r?\n)";

            return lineStart + noExistingXmlDocs + findIndentation + matchSignatureKeywords + matchMemberName + validMethodFollowUp;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="code"></param>
        /// <param name="regex"></param>
        /// <param name="implicitName"></param>
        /// <returns></returns>
        private static string GetUpdatedCode(ref string code, Regex regex, string implicitName)
        {
            string newLineEnding = code.Contains("\r\n") ? "\r\n" : "\n";

            return regex.Replace(code, match => { return MatchEvaluation(match, newLineEnding, implicitName); }, 1);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="match"></param>
        /// <param name="newLine"></param>
        /// <param name="implicitName"></param>
        /// <returns></returns>
        private static string MatchEvaluation(Match match, string newLine, string implicitName)
        {
            string indent = match.Groups[IndentKeyword].Value;

            string signature = match.Groups[SignatureKeyword].Value;

            return indent + InheritDoc + newLine + indent + signature + " " + implicitName;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="code"></param>
        /// <param name="updatedCode"></param>
        /// <returns></returns>
        private static bool UpdatedCodeApplied(ref string code, string updatedCode)
        {
            bool result = updatedCode != code;

            if (result)
                code = updatedCode;

            return result;
        }
    }
}