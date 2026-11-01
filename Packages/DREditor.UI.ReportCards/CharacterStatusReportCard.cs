//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.DialogueMechanics.Investigation;
using UnityEngine;
using UnityEngine.Events;

namespace DREditor.UI.ReportCards
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(ReportCardComponent) +
        "\n\n")]
    public class CharacterStatusReportCard : ReportCardComponent
    {
        [Header("")]
        [SerializeField] protected UnityEvent onCharacterStatusAlive = null;
        
        [Header("")]
        [SerializeField] protected UnityEvent onCharacterStatusDead = null;


        /// <summary>
        ///
        /// </summary>
        protected CharacterDeathHandler characterDeathHandler = null;


        /// <summary>
        ///
        /// </summary>
        protected virtual void Start() => characterDeathHandler = FindAnyObjectByType<CharacterDeathHandler>();


        /// <inheritdoc/>
        protected override void UpdateCharacter()
        {
            base.UpdateCharacter();

            if (IsCharacterAlive())
                OnCharacterAlive();
            else
                OnCharacterDead();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool IsCharacterAlive()
        {
            if (characterDeathHandler == null)
                return true;
            else
                return !(characterDeathHandler != null && characterDeathHandler.IsCharacterDead(character.FirstName));
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnCharacterAlive() => onCharacterStatusAlive?.Invoke();

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnCharacterDead() => onCharacterStatusDead?.Invoke();
    }
}