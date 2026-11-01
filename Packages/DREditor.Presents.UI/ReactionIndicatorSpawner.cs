//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Characters;
using UnityEngine;

namespace DREditor.Presents.UI
{
    /// <summary>
    ///
    /// </summary>
	[ClassHeader("")]
    public class ReactionIndicatorSpawner : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected CharacterDatabase characterDatabase;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected GameObject reactionIndicatorPrefab;


        /// <summary>
        ///
        /// </summary>
        [ContextMenu("Spawn Indicators")]
        public void SpawnIndicators()
        {
            if (characterDatabase == null || reactionIndicatorPrefab == null)
            {
                UnityEngine.Debug.LogWarning("ALERT: You the character database and the reaction indicator fields must be filled out!");

                return;
            }

            var reactionableCharacters = characterDatabase.GetFTECharacterList();

            foreach (var character in reactionableCharacters)
                if (character != null)
                    SpawnCharacterIndicator(character);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="character"></param>
        protected virtual void SpawnCharacterIndicator(Character character)
        {
            var indicator = Instantiate(reactionIndicatorPrefab, transform);

            if (indicator.TryGetComponent(out CharacterReactionIndicator validIndicator))
                validIndicator.InitializeName(character.FirstName);
        }
    }
}