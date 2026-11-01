//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Cameras;
using DREditor.Characters;
using TMPro;
using UnityEngine;

namespace DREditor.Minigames.BS
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    ///
    /// </remarks>
	[ClassHeader("")]
    public class BlackenedNameScroller : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected CharacterDatabase characterDatabase = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected TextMeshProUGUI nameText = null;


        /// <summary>
        ///
        /// </summary>
        protected DRTrialCamera trialCamera = null;

        /// <summary>
        ///
        /// </summary>
        protected Vector3 defaultPosition = Vector3.zero;

        /// <summary>
        ///
        /// </summary>
        protected int seatPosition = -1;


        /// <summary>
        ///
        /// </summary>
        protected virtual void Start()
        {
            defaultPosition = nameText.rectTransform.position;

            trialCamera = FindAnyObjectByType<DRTrialCamera>();
        }

        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        public virtual void Animate() => DisplayCharacterName();

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayCharacterName()
        {
            SetSeatPosition();

            foreach (var character in characterDatabase.Characters)
                if (character.TrialPosition == seatPosition)
                    SetCharacterName(character);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual int SetSeatPosition() => seatPosition = Mathf.RoundToInt(trialCamera.SeatFocus);

        /// <summary>
        ///
        /// </summary>
        /// <param name="character"></param>
        protected virtual void SetCharacterName(Character character)
            => nameText.text = character.FirstName + " " + character.LastName;
    }
}