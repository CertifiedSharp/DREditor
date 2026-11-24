//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using DREditor.Characters;
using System.Collections.Generic;

namespace DREditor.Dialogues
{
    /// <summary>
    ///
    /// </summary>
    public static class DialogueExtensions
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="dialogue"></param>
        /// <returns></returns>
        public static List<Character> GetCharactersInDialogue(this Dialogue dialogue)
        {
            List<Character> characters = new();

            ScanCharactersInDialogue(characters, dialogue);

            return characters;
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="characters"></param>
        /// <param name="dialogue"></param>
        static void ScanCharactersInDialogue<T>(List<Character> characters, DialogueBase<T> dialogue)
            where T : Line
        {
            foreach (Line line in dialogue.Lines)
                if (!characters.Contains(line.Speaker))
                    characters.Add(line.Speaker);

            if (dialogue.GetEndEvent<DirectTo>() != null &&
                dialogue.GetEndEvent<DirectTo>().NewDialogue != null &&
                dialogue != dialogue.GetEndEvent<DirectTo>().NewDialogue &&
                dialogue.GetEndEvent<DirectTo>().NewDialogue.IsValid(out DialogueBase<T> validResult))
                ScanCharactersInDialogue(characters, validResult);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="dialogue"></param>
        /// <param name="result"></param>
        /// <returns></returns>
        public static bool IsValid<T>(this DialogueCore dialogue, out DialogueBase<T> result)
        {
            result = null;

            if (dialogue is DialogueBase<T> validResult)
                result = validResult;

            return result != null;
        }
    }
}
