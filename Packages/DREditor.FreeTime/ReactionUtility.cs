//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using UnityEditor;

namespace DREditor.FreeTime
{
    /// <summary>
    ///
    /// </summary>
    public static class ReactionUtility
    {
        /// <summary>
        ///
        /// </summary>
        [MenuItem("DREditor/FTE/Generate Base Reaction Data")]
        public static void GenerateBaseReactionData()
        {
            UnityEngine.Debug.LogWarning("ALERT: This tool has not been implemented as there will be a " +
                "PresentReactionLevel Rework in the future and this class will depend on that process." +
                "\nContact Sweden for more information.");

            //database.characterReactions.Clear();

            //List<Character> characters = characterDatabase.GetFTECharacterList();

            //List<PresentReactionLevel> levels = Enum.GetValues(typeof(PresentReactionLevel)).Cast<PresentReactionLevel>().ToList();

            //foreach (Character character in characters)
            //{
            //    CharacterReaction reaction = new(character.FirstName, character);

            //    database.characterReactions.Add(reaction);

            //    foreach(PresentReactionLevel level in levels)
            //    {
            //        PresentReaction presReact = new(level.ToString(), level);

            //        reaction.Reactions.Add(presReact);
            //    }
            //}
        }
    }
}
#endif