//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Characters;
using DREditor.Dialogues;
using DREditor.Dialogues.Loading;
using DREditor.Services;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.FreeTime
{
    /// <summary>
    ///
    /// </summary>
    [System.Serializable]
    [CreateAssetMenu(menuName = "DREditor/FTE/Reaction Database", fileName = "Reaction Database")]
    public class ReactionDatabase : ScriptableObject, IDialogueHolder
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        public List<CharacterReaction> characterReactions = new();

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int badReactionIndex = 4;


        /// <summary>
        ///
        /// </summary>
        /// <param name="character"></param>
        /// <param name="level"></param>
        /// <returns></returns>
        public virtual List<DialogueCore> GetReactionPool(Character character, int level)
        {
            var pool = characterReactions.Find(characterReaction => characterReaction.character == character)
                .Reactions.Find(presentReaction => presentReaction.reactionKeyIndex == level)
                .reactions;

            List<DialogueCore> dialogues = new();

            foreach (var reaction in pool)
                dialogues.Add(reaction);

            return dialogues;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dialogue"></param>
        /// <returns></returns>
        public virtual int GetReactionKeyFromDialogue(Dialogue dialogue)
        {
            CharacterReaction charReact = GetReactionFromDialogue(dialogue);

            PresentReaction reaction = charReact.GetReactionFromDialogue(dialogue);

            if (reaction == null)
            {
                UnityEngine.Debug.LogWarning(nameof(ReactionDatabase) + ": " +
                "The reation level couldn't be found for dialogue: " +
                dialogue.name + " and will return a bad reaction level.");

                return badReactionIndex;
            }

            return reaction.reactionKeyIndex;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dialogue"></param>
        /// <returns></returns>
        protected virtual CharacterReaction GetReactionFromDialogue(Dialogue dialogue)
        {
            foreach (CharacterReaction reactions in characterReactions)
            {
                PresentReaction presentReactionAsset = reactions.Reactions
                    .Find(presentReactions => presentReactions.reactions.Contains(dialogue));

                if (presentReactionAsset != null)
                    return reactions;
            }

            UnityEngine.Debug.LogWarning(nameof(ReactionDatabase) + ": The character reaction couldn't be found for dialogue: " +
                dialogue.name + " and will return a null reaction.");

            return null;
        }


        /// <inheritdoc/>
        public virtual bool CheckIfSaveDialogue()
        {
            foreach (var reaction in characterReactions)
                foreach (var presentReaction in reaction.Reactions)
                    if (ServiceLocator.GetService<IDialogueManager>()
                        .GetManagerComponent<DialogueLoadIdentifier>()
                        .CheckDialogues<Line>(presentReaction.GetReactions()))
                        return true;

            return false;
        }
    }
}