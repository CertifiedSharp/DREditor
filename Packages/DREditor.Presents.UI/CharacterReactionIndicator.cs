//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.UI;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

namespace DREditor.Presents.UI
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(SelectedItemDisplayer) +
        "\n\n")]
    [RequireComponent(typeof(Image))]
    public class CharacterReactionIndicator : SelectedItemDisplayer
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string characterName;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected PresentDatabase presentDatabase = null;

        
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected Image indicatorImage = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Sprite positiveArrow = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Sprite negativeArrow = null;


        /// <summary>
        ///
        /// </summary>
        protected Present currentPresent = null;

        /// <summary>
        ///
        /// </summary>
        protected PresentManager presentDataHandler = null;


        /// <summary>
        ///
        /// </summary>
        /// <param name="name"></param>
        public virtual void InitializeName(string name) => characterName = name;

        /// <inheritdoc/>
        public override void DisplaySelectedItem(int globalIndex)
        {
            if (presentDatabase.presents[globalIndex] is Present present)
                currentPresent = present;
            else
                currentPresent = null;

            DisplayReaction();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayReaction()
        {
            if (CurrentPresentValid())
                return;

            IReactionEntry reaction = currentPresent.attributes
                .OfType<StudentReactionCollection>()
                .FirstOrDefault()
                .GetReactionByName(characterName);

            if (reaction != null)
            {
                switch (reaction.ReactionIndex)
                {
                    case 0:
                        DisplayLove();
                        return;
                    case 1:
                        DisplayLike();
                        return;
                    case 2:
                        DisplayNeutral();
                        return;
                    case 3:
                        DisplayDislike();
                        return;
                    case 4:
                        DisplayHate();
                        return;
                }
            }
            else
            {
                UnityEngine.Debug.LogWarning("Couldn't find Student Reaction for: " + characterName +
                    ". Make sure that the present has a reaction for the character and that the " +
                    "characterName property on this script is spelled correctly!");

                OnPresentNotFound();
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool CurrentPresentValid()
        {
            if (presentDataHandler == null)
                presentDataHandler = FindAnyObjectByType<PresentManager>();

            bool result = presentDataHandler != null && !presentDataHandler.HadPresent(currentPresent.ID);

            if (result || currentPresent == null)
                OnPresentNotFound();

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnPresentNotFound() => DisplayNeutral();


        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayLove()
        {
            indicatorImage.sprite = positiveArrow;

            indicatorImage.enabled = true;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayLike() => DisplayLove();

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayNeutral() => indicatorImage.enabled = false;

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayDislike() => DisplayHate();

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayHate()
        {
            indicatorImage.sprite = negativeArrow;

            indicatorImage.enabled = true;
        }
    }
}