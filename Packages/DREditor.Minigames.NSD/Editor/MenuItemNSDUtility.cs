//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using DREditor.Dialogues;
using DREditor.Minigames.Debates;
using System.Collections.Generic;
using UnityEditor;

namespace DREditor.Minigames.NSD.Editor
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
    public static class MenuItemNSDUtility
    {
        /// <summary>
        ///
        /// </summary>
        [MenuItem("DREditor/Import/Dialogues/Fuck Up Dialogue")]
        public static void ImportFUDialogue()
        {
            DebateBuilder<Panel> builder = null;

            List<DialogueCore> fuckUpDialogues = new();

            foreach (var selection in Selection.objects)
                if (selection is DebateBuilder<Panel> debate)
                    builder = debate;
                else if (selection is DialogueCore dialogue)
                    fuckUpDialogues.Add(dialogue);

            UnityEngine.Debug.Log("Count: " + fuckUpDialogues.Count);

            if (builder != null)
            {
                ImportFUDialogueToAsset(builder, fuckUpDialogues);

                EditorUtility.SetDirty(builder);

                UnityEngine.Debug.Log("Import Complete");
            }
            else
                UnityEngine.Debug.LogWarning("Couldn't find builder to import to.");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="fuckUpDialogues"></param>
        static void ImportFUDialogueToAsset(DebateBuilder<Panel> builder, List<DialogueCore> fuckUpDialogues)
        {
            for (int i = 0; i < fuckUpDialogues.Count; i++)
                foreach (var panel in builder.Lines)
                    if (ImportedFUDialogue(panel, fuckUpDialogues[i]))
                        break;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="panel"></param>
        /// <param name="fuckUpDialogue"></param>
        /// <returns></returns>
        static bool ImportedFUDialogue(Panel panel, DialogueCore fuckUpDialogue)
        {
            foreach (var basePhrase in panel.Phrases)
            {
                if (basePhrase is Phrase phrase && IsMissingFUDialogue(basePhrase))
                {
                    phrase.SetFuckUpDialogue(fuckUpDialogue);

                    UnityEngine.Debug.Log("Adding: " + fuckUpDialogue.name);

                    return true;
                }
            }

            return false;
        }

        /// <summary>
        ///
        /// </summary>
        [MenuItem("DREditor/NSD/Clear Selected Debates all FU Dialogue")]
        public static void ClearFUDialogue()
        {
            foreach (var selection in Selection.objects)
            {
                if (selection is DebateBuilder<Panel> builder)
                {
                    foreach (var panel in builder.Lines)
                        foreach (var basePhrase in panel.Phrases)
                            if (ContainsFUDialogue(basePhrase) && basePhrase is Phrase phrase)
                                phrase.SetFuckUpDialogue(null);

                    EditorUtility.SetDirty(builder);
                }
            }

            UnityEngine.Debug.Log("Clear Complete");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="basePhrase"></param>
        /// <returns></returns>
        static bool ContainsFUDialogue(PhraseBase basePhrase)
            => basePhrase is Phrase phrase && phrase.fuckUpDialogue != null && IsPhraseHitable(phrase);

        /// <summary>
        ///
        /// </summary>
        /// <param name="basePhrase"></param>
        /// <returns></returns>
        static bool IsMissingFUDialogue(PhraseBase basePhrase)
            => basePhrase is Phrase phrase && phrase.fuckUpDialogue == null && IsPhraseHitable(phrase);

        /// <summary>
        ///
        /// </summary>
        /// <param name="phrase"></param>
        /// <returns></returns>
        public static bool IsPhraseHitable(PhraseBase phrase) => phrase.Has<PartlyHitModifier>() || phrase.Has<InputModifier>();
    }
}
#endif