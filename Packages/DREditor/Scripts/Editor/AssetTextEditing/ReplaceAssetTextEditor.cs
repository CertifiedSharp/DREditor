//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Utility;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using UnityEditor;
using UnityEngine;

namespace DREditor.Editor.AssetTextEditing
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
    public class ReplaceAssetTextEditor : ScriptableWizard
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] string lookFor = "ns: , asm: Assembly-CSharp";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] string replaceWith = "ns: DREditor.CustomEvents, asm: DREditor.CustomEvents";


        /// <summary>
        ///
        /// </summary>
        private readonly List<string> OldEventStructs = new()
        {
            "SCGValue:",
            "cTPTuple:",
            "CCFValue:",
            "cISTuple:",
            "cITuple:",
            "CEValue:",
            "FBValue:",
            "SVValue:",
            "oTuple:",
        };


        [MenuItem("DREditor/Convert/Replace Dialogue Asset Text Editor", false, 4)]
        public static void ShowWindow()
            => DisplayWizard<ReplaceAssetTextEditor>("Replace Dialogue Asset Text Editor", "Convert", "Update Reworked Dialogue Events");


        protected virtual void OnWizardCreate() =>
            AssetEditorUtility.UpdateAllAssetsOfType<DialogueCore>(updateProcess: ReplaceWhatsLookedFor);

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        protected virtual void ReplaceWhatsLookedFor<T>(T obj) where T : UnityEngine.Object =>
            ReplaceAssetTextUtility.ReplaceAssetText(obj, ReplaceWhatsLookedFor);

        /// <summary>
        ///
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        protected virtual string ReplaceWhatsLookedFor(string content) => content.Replace(lookFor, replaceWith);


        protected virtual void OnWizardOtherButton() =>
            AssetEditorUtility.UpdateAllAssetsOfType<DialogueCore>(updateProcess: ReplaceOldEventStructs);

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="obj"></param>
        protected virtual void ReplaceOldEventStructs<T>(T obj) where T : UnityEngine.Object
        {
            ReplaceAssetTextUtility.ReplaceAssetText(obj, RemoveOldStructsInText);

            ReplaceAssetTextUtility.ReplaceAssetText(obj, RemoveRogueDataInText);

            ReplaceAssetTextUtility.ReplaceAssetText(obj, RemoveAnimationEvents);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        protected virtual string RemoveOldStructsInText(string content) => content.ReplaceMany(OldEventStructs, string.Empty);

        /// <summary>
        ///
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        protected virtual string RemoveRogueDataInText(string content)
        {
            string pattern = @"(?<prefix>type:\s*\{class:\s*[^,]+,\s*ns:\s*DREditor\.Dialogues\.Events,\s*asm:\s*DREditor\}" + 
                @"\s*\r?\n\s*data:\s*\r?\n(?:\s*_[^:\r\n]+:\s*[^\r\n]+\r?\n)?\s*)data:";

            return Regex.Replace(content, pattern, "${prefix}");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="content"></param>
        /// <returns></returns>
        protected virtual string RemoveAnimationEvents(string content)
        {
            string pattern = @"(^\s*type:\s*\{class:\s*AnimationEvent,[^\r\n]+\r?\n\s*data:\r?\n\s*done:\s*\d+\r?\n)\s*a:\r?\n\s*(EventName:)";

            string replacement = "$1        $2";

            return Regex.Replace(content, pattern, replacement, RegexOptions.Multiline);
        }
    }
}