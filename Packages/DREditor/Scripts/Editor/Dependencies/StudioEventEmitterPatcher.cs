//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using System;
using System.IO;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace DREditor.Editor.Dependencies
{
    /// <summary>
    /// A static class for a menu item to fix a bug in the FMOD StudioEventEmitter script where it only plays the first music ever played.
    /// </summary>
    /// <remarks>
    /// This script can be removed in the future if we end up updating the DREditor FMOD version in the future.
    /// - Sweden.
    /// </remarks>
    public static class StudioEventEmitterPatcher
    {
        /// <summary>
        /// The StudioEventEmitter script file name.
        /// </summary>
        private const string EmitterFileName = "StudioEventEmitter.cs";

        /// <summary>
        /// The filter unity will use to find the StudioEventEmitter script file.
        /// </summary>
        private const string FindFileFilter = "t:MonoScript StudioEventEmitter";


        /// <summary>
        /// Patches a bug in the FMOD StudioEventEmitter script where it only plays the first music ever played.
        /// </summary>
        [MenuItem("DREditor/Dependencies/FMOD/Patch StudioEventEmitter", priority = 4)]
        public static void PatchStudioEventEmitter()
        {
            if (EmitterPathValid(out string path) &&
                File.ReadAllText(path) is string text &&
                TextCanBePatched(text, path, out Match clearCacheMatch))
                PatchTextAtPath(CreateUpdatedText(text, clearCacheMatch), path);
            else
                Debug.LogWarning("ALERT: Process aborted.");
        }

        /// <summary>
        /// Checks whether the path for the StudioEventEmitter script is valid, outputting the valid path if so.
        /// </summary>
        /// <param name="path">The valid path retrieved from the </param>
        /// <returns>True if the retrieved path for the StudioEventEmitter script is valid.</returns>
        private static bool EmitterPathValid(out string path)
        {
            path = GetEmitterPath();

            bool result = path != null;

            if (!result)
                Debug.LogError($"ALERT: {EmitterFileName} Could not be found within the project.");

            return result;
        }

        /// <summary>
        /// Retrieves the file path to the FMOD StudioEventEmitter script.
        /// </summary>
        /// <returns>The string path to the FMOD StudioEventEmitter script.</returns>
        private static string GetEmitterPath()
        {
            foreach (string guid in AssetDatabase.FindAssets(FindFileFilter))
                if (AssetDatabase.GUIDToAssetPath(guid) is string emitterPath &&
                    Path.GetFileName(emitterPath) == EmitterFileName)
                    return emitterPath;

            return null;
        }


        /// <summary>
        /// Checked whether the text should/can be patched.
        /// </summary>
        /// <param name="text">The text to patch to the file at the given path.</param>
        /// <param name="path">The path of the file to write the text to.</param>
        /// <param name="clearCacheMatch">The regex match to enable the insertion of </param>
        /// <returns>True if the text should/can be patched.</returns>
        private static bool TextCanBePatched(string text, string path, out Match clearCacheMatch)
        {
            clearCacheMatch = null;

            bool alreadyPatched = Regex.IsMatch(text, @"cachedParams\.Clear\(\);\s*if\s*\(\s*hasTriggered\s*\)\s*\{\s*Lookup\(\);\s*\}");

            if (ConditionMatch(
                condition: alreadyPatched,
                matches: true,
                displayMessage: $"REPORT: {path} has already been patched!",
                displayWith: Debug.Log))
                return false;

            Match playFunctionMatch = Regex.Match(text, @"public\s+void\s+Play\s*\(\s*\)\s*\{");

            if (ConditionMatch(
                condition: !playFunctionMatch.Success,
                matches: true,
                displayMessage: $"ALERT: Could not find the Play function at: {path}.",
                displayWith: Debug.LogError))
                return false;

            Regex clearCacheRegex = new(@"^([ \t]*)cachedParams\.Clear\(\);[ \t]*(\r?\n)", RegexOptions.Multiline);

            clearCacheMatch = clearCacheRegex.Match(text, playFunctionMatch.Index);

            if (ConditionMatch(
                condition: !clearCacheMatch.Success,
                matches: true,
                displayMessage: $"ALERT: Could not find cachedParams.Clear(); inside the Play function at: {path}.",
                displayWith: Debug.LogError))
                return false;

            return true;
        }

        /// <summary>
        /// Checks if a condition matches a given result. If so, displays a message using the action provided.
        /// </summary>
        /// <param name="condition">The condition to compare against.</param>
        /// <param name="matches">The value that the condition must match in order to display a message.</param>
        /// <param name="displayMessage">The message to display if the condition matches the given value.</param>
        /// <param name="displayWith">The function used to display the given message.</param>
        /// <returns>The value of condition.</returns>
        private static bool ConditionMatch(bool condition, bool matches, string displayMessage, Action<string> displayWith)
        {
            if (condition == matches)
                displayWith(displayMessage);

            return condition;
        }


        /// <summary>
        /// Rewrites the file with the patched text at the given path.
        /// </summary>
        /// <param name="text">The text to patch to the file at the given path.</param>
        /// <param name="path">The path of the file to write the text to.</param>
        private static void PatchTextAtPath(string text, string path)
        {
            File.WriteAllText(path, text);

            AssetDatabase.ImportAsset(path);

            Debug.Log($"REPORT: Successfully Patched Studio Event Emitter!");
        }

        /// <summary>
        /// Creates the updated patched text using the regex match to insert the patch at the correct code spot.
        /// </summary>
        /// <param name="text">Text to insert the update to.</param>
        /// <param name="clearCacheMatch">Regex match used to insert the patch at the correct code spot.</param>
        /// <returns>The text with the patched block inserted into it.</returns>
        private static string CreateUpdatedText(string text, Match clearCacheMatch)
        {
            string indent = clearCacheMatch.Groups[1].Value;

            string newLine = clearCacheMatch.Groups[2].Value;

            string patchedBlock =
                newLine +
                indent + "if (hasTriggered)" + newLine +
                indent + "{" + newLine +
                indent + "    Lookup();" + newLine +
                indent + "}" + newLine;

            int insertAt = clearCacheMatch.Index + clearCacheMatch.Length;

            return text.Insert(insertAt, patchedBlock);
        }
    }
}