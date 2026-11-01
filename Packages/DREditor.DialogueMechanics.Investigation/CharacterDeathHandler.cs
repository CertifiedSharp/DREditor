//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.DialogueEventHandling;
using DREditor.Saving;
using DREditor.Utility;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.DialogueMechanics.Investigation
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DialogueEventHandler) + ", " + nameof(IDRESaveable) + ", " + 
        nameof(IDRELoadableManager) + ", " + nameof(IPlayerDataClearer) +
        "\n\n")]
    public class CharacterDeathHandler : DialogueEventHandler, IDRESaveable, IDRELoadableManager, IPlayerDataClearer
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string saveKey = "deadList";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<string> deadCharacters = new();


        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        protected override void OnDialogueEventTriggered(object o = null)
        {
            if (o is string firstName)
                KillCharacter(firstName);
            else
                UnityEngine.Debug.LogWarning($"ALERT: The handler listening to the {dialogueEventString} event did not send data of the proper type!" +
                    $"\nANALYSIS: Data must be of type: string. Data Sent: {o.GetType()}");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="firstName"></param>
        public virtual void KillCharacter(string firstName)
        {
            if (!IsCharacterDead(firstName))
                AddToTheDead(firstName);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="firstName"></param>
        protected virtual void AddToTheDead(string firstName) => deadCharacters.Add(firstName);

        /// <summary>
        ///
        /// </summary>
        /// <param name="firstName"></param>
        /// <returns></returns>
        public virtual bool IsCharacterDead(string firstName) => deadCharacters.Contains(firstName);


        /// <inheritdoc/>
        public virtual void Save(Dictionary<string, object> saveData) => saveData.Set(saveKey, deadCharacters);

        /// <inheritdoc/>
        public virtual void LoadManager(Dictionary<string, object> saveDatas, bool inChapterSelect)
            => deadCharacters = saveDatas.Get<List<string>>(saveKey).DeepCopy();


        /// <inheritdoc/>
        public virtual void ClearData() => deadCharacters.Clear();


        [ContextMenu("Set Default Event")]
        public void SetDefaultEvent() => SetDefaultEventString("KillCharacter");

        protected virtual void Reset() => SetDefaultEventString("KillCharacter");
    }
}