//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Characters;
using DREditor.DialogueEventHandling;
using DREditor.Dialogues;
using DREditor.Dialogues.Events;
using DREditor.EventObjects;
using DREditor.Presents;
using DREditor.Services;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DREditor.FreeTime
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(ServiceHolder<FTEManager>) + " for type: " + nameof(FTEManager) +
        "\n\n")]
    public class FTEManager : ServiceHolder<FTEManager>, IDialogueHolder
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]

        [Space]

        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected ReactionDatabase reactionDatabase = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected FTEDatabase fteDatabase = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected CharacterDatabase characterDatabase = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SelectableObjectWithEvent lastActor = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected string callEvaluateReaction = "EvaluateReaction";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string callEvaluateFTE = "EvaluateFTE";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string callCompleteFTE = "CompleteFTE";

        /// <summary>
        ///
        /// </summary>
        [Space]

        [Tooltip("")]
        [SerializeField] protected DialogueEventHandler moveToNextDayHandler = null;


        /// <summary>
        ///
        /// </summary>
        protected IFTEDataHandler fteData = null;


        #region Initialization

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnEnable()
        {
            DialogueEventSystem.StartListening(callEvaluateReaction, EvaluateReaction);

            DialogueEventSystem.StartListening(callEvaluateFTE, EvaluateFTE);

            DialogueEventSystem.StartListening(callCompleteFTE, CompleteFTE);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDisable()
        {
            DialogueEventSystem.StopListening(callEvaluateReaction, EvaluateReaction);

            DialogueEventSystem.StopListening(callEvaluateFTE, EvaluateFTE);

            DialogueEventSystem.StopListening(callCompleteFTE, CompleteFTE);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void Start()
        {
            fteData = GetComponent<IFTEDataHandler>();

            fteData ??= GetComponentInChildren<IFTEDataHandler>();
        }

        #endregion

        #region Evaluate Present Reaction Event

        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        protected virtual void EvaluateReaction(object o = null)
        {
            var startingObject = ServiceLocator.GetService<DialogueObjectManager>().GetStartObject();

            if (startingObject == null)
                startingObject = lastActor.Value;

            string displayName = startingObject.DisplayName;
            
            Character character = characterDatabase.GetCharacterByContains(displayName);

            IReactionEntry studentReaction =
                ((Present)o)
                .attributes
                .OfType<StudentReactionCollection>()
                .FirstOrDefault()
                .studentReactions
                .Find(studentReaction => studentReaction.Character == character);

            if (studentReaction != null)
                PlayReactionFromFeeling(character, studentReaction.ReactionIndex);
            else if (character != null)
                UnityEngine.Debug.LogError($"ALERT: {nameof(FTEManager)}: Couldn't find a reaction for {character.FirstName}.");
            else
                UnityEngine.Debug.LogError($"ALERT: {nameof(FTEManager)}: The evaluated character for the present reaction was null!");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="character"></param>
        /// <param name="level"></param>
        protected virtual void PlayReactionFromFeeling(Character character, int level)
        {
            List<DialogueCore> reactionPool = reactionDatabase.GetReactionPool(character, level);

            if (reactionPool.Count != 0)
                PlayPresentReaction(reactionPool);
            else
                UnityEngine.Debug.LogError($"ALERT: {nameof(FTEManager)}: Reaction pool for {character.FirstName} ({level}) is empty.");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="reactionPool"></param>
        protected virtual void PlayPresentReaction(List<DialogueCore> reactionPool)
        {
            int index = UnityEngine.Random.Range(0, reactionPool.Count);

            if (reactionPool[index] != null)
                PlayMidDialogue((Dialogue)reactionPool[index]);
            else
                UnityEngine.Debug.LogError($"ALERT: {nameof(FTEManager)}: Reaction Evaluation at {index} was null!");
        }

        #endregion

        #region Evaluate FTE Event

        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        protected virtual void EvaluateFTE(object o = null)
        {
            var startingObject = ServiceLocator.GetService<DialogueObjectManager>().GetStartObject();

            if (startingObject == null)
                startingObject = lastActor.Value;

            string displayName = startingObject.DisplayName;

            Character character = characterDatabase.GetCharacterByContains(displayName);

            Dialogue dialogue = (Dialogue)ServiceLocator.GetService<IDialogueManager>().GetReader().GetCurrentDialogue();

            int level = reactionDatabase.GetReactionKeyFromDialogue(dialogue);

            int percentChance = 0;

            switch (level)
            {
                case 0:
                case 1:
                    PlayFTE(character);
                    return;

                case 2:
                    percentChance = 50;
                    break;

                case 3:
                    percentChance = 20;
                    break;

                case 4:
                    MoveToNextDay();
                    return;
            }

            EvaluatePlayFTE(percentChance, character);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="percentChance"></param>
        /// <param name="character"></param>
        protected virtual void EvaluatePlayFTE(int percentChance, Character character)
        {
            int num = UnityEngine.Random.Range(1, 100);

            if (num <= percentChance && 
                fteData?.GetLevelOfCharacter(character.FirstName) - 1 < character.Get<SimpleFTEEntryCollection>()?.Count)
                PlayFTE(character);
            else
                MoveToNextDay();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="character"></param>
        protected virtual void PlayFTE(Character character)
        {
            Dialogue dialogue = (Dialogue)fteDatabase.GetFTE(character, fteData.GetAffinity(character.FirstName));

            if (dialogue != null)
                PlayMidDialogue(dialogue);
            else
            {
                UnityEngine.Debug.LogWarning($"ALERT: {nameof(FTEManager)}: The FTE Dialogue is null! Moving to next day.");

                MoveToNextDay();
            }
        }

        #endregion

        #region Complete FTE Event

        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        protected virtual void CompleteFTE(object o = null)
        {
            Dialogue currentDialogue = (Dialogue)ServiceLocator.GetService<IDialogueManager>().GetReader().GetCurrentDialogue();

            Character character = fteDatabase.GetCharacterFromDialogue(currentDialogue);

            fteData?.LevelUpCharacter(character.FirstName);

            MoveToNextDay();
        }

        #endregion

        #region Core Supporting Functions

        /// <summary>
        ///
        /// </summary>
        protected virtual void MoveToNextDay()
        {
            if (moveToNextDayHandler != null)
                moveToNextDayHandler.TriggerEvent();
            else
                UnityEngine.Debug.LogWarning($"ALERT: {nameof(FTEManager)}: Event handler for moving to the next day was not found!");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="dialogue"></param>
        protected virtual void PlayMidDialogue(Dialogue dialogue)
        {
            Dialogue[] dialogues = new Dialogue[1] { dialogue };

            ServiceLocator.GetService<IDialogueManager>().GetReader().MidDialogueEnterance(dialogues);
        }

        #endregion


        /// <inheritdoc/>
        public virtual bool CheckIfSaveDialogue() =>
            reactionDatabase != null && fteDatabase != null &&
            (reactionDatabase.CheckIfSaveDialogue() || fteDatabase.CheckIfSaveDialogue());
    }
}