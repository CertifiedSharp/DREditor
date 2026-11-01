//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Characters;
using DREditor.DialogueEventHandling;
using DREditor.Dialogues.Events;
using DREditor.EventObjects;
using DREditor.Services;
using DREditor.Talkables;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DREditor.Actors.Events
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DialogueEventHandler) +
        "\n\n")]
    public class MoveCharacterEvent : DialogueEventHandler
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected CharacterDatabase characterDatabase = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent isLoadingFile = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inThreeDMode = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string mainCameraTag = "MainCamera";


        /// <summary>
        ///
        /// </summary>
        protected MoveCharacter moveCharacterTuple;


        /// <summary>
        ///
        /// </summary>
        protected virtual void Awake() => SetDefaultEventString("MoveCharacter");

        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        protected override void OnDialogueEventTriggered(object o = null)
        {
            moveCharacterTuple = (MoveCharacter)o;

            if (moveCharacterTuple.all)
                MoveAllCharacters();
            else
                MoveSingleCharacter();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void MoveAllCharacters()
        {
            IEnumerable<Actor> actors = ServiceLocator.GetService<IObjectPooler<Talkable>>().GetRegisteredObjects().OfType<Actor>();

            foreach (Actor actor in actors)
                SetCharacterPositionAndRotation(actor.transform);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void MoveSingleCharacter()
        {
            string characterFirstName = characterDatabase.Characters[moveCharacterTuple.charNum].FirstName;

            GameObject actorParent = ServiceLocator.GetService<IObjectPooler<Talkable>>()
                .GetObjectByName(characterFirstName).gameObject;
            
            SetCharacterPositionAndRotation(actorParent.transform);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="transform"></param>
        protected virtual void SetCharacterPositionAndRotation(Transform transform)
        {
            SetActorLocalPosition(transform);

            if (!isLoadingFile.Value)
                SetActorRotation(transform);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="transform"></param>
        protected virtual void SetActorLocalPosition(Transform transform) => transform.localPosition = moveCharacterTuple.position;

        /// <summary>
        ///
        /// </summary>
        /// <param name="transform"></param>
        protected virtual void SetActorRotation(Transform transform)
        {
            if (inThreeDMode.Value)
                SetRotationToLookAtPlayer(transform);
            else
                transform.localEulerAngles = moveCharacterTuple.rotation;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="transform"></param>
        protected virtual void SetRotationToLookAtPlayer(Transform transform)
        {
            Transform playerCamera = GameObject.FindGameObjectWithTag(mainCameraTag).transform;
            
            Vector3 direction = playerCamera.position - transform.position;

            Quaternion lookRotation = Quaternion.LookRotation(direction);

            transform.rotation = lookRotation;

            transform.rotation = Quaternion.Euler(0, transform.rotation.eulerAngles.y, 0);
        }
    }
}