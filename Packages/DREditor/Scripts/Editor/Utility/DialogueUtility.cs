//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Characters;
using DREditor.Dialogues;
using DREditor.Editor;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace DREditor.Editor.Utility
{
    /// <summary>
    ///
    /// </summary>
    public static class DialogueUtility
    {
        /// <summary>
        ///
        /// </summary>
        [MenuItem("DREditor/Dialogue/Spawn Relevant Characters")]
        public static void SpawnRelevantCharacters()
        {
            List<Character> characters = GetCharactersInSelectedDialogues();

            if (characters.Count == 0)
            {
                UnityEngine.Debug.LogWarning("ALERT: Could not spawn relevant characters " +
                    "as none could be found in the dialogue asset(s).");

                return;
            }

            foreach (var character in characters)
            {
                if (character.ActorPrefab != null)
                {
                    GameObject gameObject = MonoBehaviour.Instantiate(character.ActorPrefab);

                    gameObject.name = gameObject.name.Replace("(Clone)", "");
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        private static List<Character> GetCharactersInSelectedDialogues()
        {
            List<Character> characters = new();

            foreach (var obj in Selection.objects)
                if (obj is Dialogue dialogue)
                    characters.AddRange(dialogue.GetCharactersInDialogue());
                else
                    continue;

            return characters;
        }


        /// <summary>
        ///
        /// </summary>
        [MenuItem("DREditor/Dialogue/Find Empty Scene Events In Dialogue")]
        public static void FindDialogueWithText()
        {
            var assets = BuilderUtility.FindAssetsByType<DialogueCore>();

            bool foundOne = false;

            for (int i = 0; i < assets.Count; i++)
                for (int x = 0; x < assets[i].GetLines<Line>().Count; x++)
                    if (assets[i].GetLines<Line>()[x].HasEmptySceneEvents())
                    {
                        foundOne = true;

                        UnityEngine.Debug.Log("REPORT: Empty Scene Event Found On: " + assets[i].name + " on line: " + x);
                    }

            if (!foundOne)
                UnityEngine.Debug.LogWarning("ALERT: Couldn't find a trial dialogue that fit.");
        }
    }
}