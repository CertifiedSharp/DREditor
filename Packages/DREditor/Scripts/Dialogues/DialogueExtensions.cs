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

            if (dialogue.DirectTo != null &&
                dialogue.DirectTo.NewDialogue != null &&
                dialogue != dialogue.DirectTo.NewDialogue)
                ScanCharactersInDialogue(characters, dialogue.DirectTo.NewDialogue);
        }
    }
}
