//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using DREditor.Characters;
using DREditor.Dialogues;
using DREditor.Dialogues.Events;
using DREditor.Editor.Toolbox;
using DREditor.Editor.Utility;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.Flairs.Editor
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public abstract class GenerateFlairDialogueOptionBase : IGenerateFlairDialogueSetting
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string characterDatabaseFolderPath = "Assets/Resources/Characters";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string targetDirectory = "Dialogues";


        /// <summary>
        ///
        /// </summary>
        protected CharacterDatabase cdb;

        /// <summary>
        ///
        /// </summary>
        protected List<IFlairEvent> events;

        /// <summary>
        ///
        /// </summary>
        protected Dictionary<Character, int> characterIndexes;


        protected GenerateFlairDialogueOptionBase() => targetDirectory = CustomEditorUtility.GetCurrentProjectWindowPath();


        /// <inheritdoc/>
        public virtual void ExecuteOption(List<IFlairEvent> events)
        {
            Initialize(events);

            GenerateDialogue();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="events"></param>
        protected virtual void Initialize(List<IFlairEvent> events)
        {
            cdb = BuilderUtility.SetAssetByType<CharacterDatabase>($"t:{nameof(CharacterDatabase)}", new[] { characterDatabaseFolderPath });

            this.events = events;

            characterIndexes = new Dictionary<Character, int>(cdb.Characters.Count);

            for (int i = 0; i < cdb.Characters.Count; i++)
                characterIndexes[cdb.Characters[i]] = i;
        }

        /// <summary>
        ///
        /// </summary>
        protected abstract void GenerateDialogue();

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected virtual T MakeInstance<T>() where T : ScriptableObject => (T)ScriptableObject.CreateInstance(typeof(T));

        /// <summary>
        ///
        /// </summary>
        /// <param name="character"></param>
        /// <param name="flair"></param>
        /// <param name="flairName"></param>
        /// <param name="expressionNum"></param>
        /// <returns></returns>
        protected virtual Line CharacterFlairLine(Character character, IFlairEvent flair, string flairName, int expressionNum = -1)
        {
            Expression expression = expressionNum >= 0 ? character.Expressions[expressionNum] : null;

            Line line = new(character, characterIndexes[character], expression, expressionNum + 1, 
                "Testing: " + character.FirstName + " " + flairName);

            if (flair is IDialogueEvent dialogueEvent)
                line.DiaEvents.Add(dialogueEvent);
            else
                UnityEngine.Debug.LogWarning($"ALERT: The sent flair event was not also inheriting {nameof(IDialogueEvent)}!");

            return line;
        }
    }
}
#endif