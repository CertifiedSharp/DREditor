//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.Loading;
using DREditor.EventObjects;
using DREditor.LocalDialogues;
using DREditor.Services;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace DREditor.Talkables
{
    /// <summary>
    ///
    /// </summary>
    public abstract class Talkable : SelectableObject, IDialogueProgression, IDialogueHolder, ISkipConversation
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public bool identified = true;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string unidentifiedDisplayName = "???";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [FormerlySerializedAs("RoomConvo")]
        public List<LocalDialogue> conversations = new();


        /// <summary>
        ///
        /// </summary>
        public event Action<Talkable, Dialogue[]> StartConversation;


        /// <summary>
        ///
        /// </summary>
        /// <param name="action"></param>
        public virtual void SubscribeToStartConversation(Action<Talkable, Dialogue[]> action) => StartConversation += action;

        /// <summary>
        ///
        /// </summary>
        /// <param name="action"></param>
        public virtual void UnsubscribeToStartConversation(Action<Talkable, Dialogue[]> action) => StartConversation -= action;

        /// <summary>
        ///
        /// </summary>
        /// <param name="conversation"></param>
        protected virtual void InvokeConversation(Dialogue[] conversation) => StartConversation?.Invoke(this, conversation);


        /// <summary>
        ///
        /// </summary>
        protected virtual void Start() => Register(this);

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pooledObject"></param>
        protected virtual void Register<T>(T pooledObject) where T : SelectableObject
        {
            ServiceLocator.GetService<IObjectPooler<SelectableObject>>()?.RegisterObject(pooledObject);

            if (ServiceLocator.HasService<IObjectPooler<T>>())
                ServiceLocator.GetService<IObjectPooler<T>>()?.RegisterObject(pooledObject);
            else
                StartCoroutine(WaitForObjectPooler(pooledObject));
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pooledObject"></param>
        /// <returns></returns>
        protected virtual IEnumerator WaitForObjectPooler<T>(T pooledObject)
        {
            UnityEngine.Debug.LogWarning($"ALERT: Pooler for type: {typeof(T)} does not exist!");

            UnityEngine.Debug.Log($" {typeof(T)} Started Waiting for: " + transform.parent.name);

            yield return new WaitUntil(() => ServiceLocator.HasService<IObjectPooler<T>>());

            ServiceLocator.GetService<IObjectPooler<T>>()?.RegisterObject(pooledObject);

            UnityEngine.Debug.Log("Successfully waited for: " + transform.parent.name);
        }


        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDestroy() => Unregister(this);

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="pooledObject"></param>
        protected virtual void Unregister<T>(T pooledObject) where T : SelectableObject
        {
            if (ServiceLocator.HasService<IObjectPooler<SelectableObject>>(out var service))
                service.UnregisterObject(pooledObject);

            ServiceLocator.GetService<IObjectPooler<T>>()?.UnregisterObject(pooledObject);
        }


        /// <inheritdoc/>
        public override string DisplayName => identified ? base.DisplayName : unidentifiedDisplayName;


        /// <inheritdoc/>
        public virtual bool CheckIfSaveDialogue()
        {
            foreach (LocalDialogue localDialogue in conversations)
                if (ServiceLocator.GetService<IDialogueManager>()
                    .GetManagerComponent<DialogueLoadIdentifier>()
                    .CheckDialogue<Line>(localDialogue.dialogue)
                    ||
                    ServiceLocator.GetService<IDialogueManager>()
                    .GetManagerComponent<DialogueLoadIdentifier>()
                    .CheckDialogue<Line>(localDialogue.dialogueIfFlagNotMet))
                    return true;

            return false;
        }

        /// <inheritdoc/>
        public virtual void SkipConversation(int conversationIndex) => conversations[conversationIndex].SetTriggered();

        /// <inheritdoc/>
        public virtual bool AllDialogueTriggered() => conversations.AllDialogueTriggered();

        /// <inheritdoc/>
        public virtual bool HasUnTriggeredRequiredFlags() => conversations.HasUnTriggeredRequiredFlags();
    }
}