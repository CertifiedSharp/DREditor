//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Services;
using DREditor.Talkables;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DREditor.Actors
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DialogueObjectManager) + ", " + nameof(IObjectPooler<Talkable>) +
        "\n\n")]
    public class ActorManager : DialogueObjectManager, IObjectPooler<Talkable>
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string tutorialCharacterFirstName = "Tutorial";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool logActorsNotFound = false;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected List<Talkable> actors = new();


        /// <summary>
        ///
        /// </summary>
        protected IDialogueManager dialogueManager = null;


        /// <summary>
        ///
        /// </summary>
        protected virtual void OnEnable()
        {
            if (gameObject != null)
            {
                RegisterService<DialogueObjectManager>(this);

                RegisterService<IObjectPooler<Talkable>>(this);
            }

            dialogueManager = ServiceLocator.GetService<IDialogueManager>();

            if (dialogueManager != null)
                dialogueManager.GetEnder().OnDialogueResolving += SetStartObjectNull;
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="service"></param>
        protected virtual void RegisterService<T>(T service) where T : class
        {
            if (!ServiceLocator.HasService<T>())
                ServiceLocator.Register(service);
            else
                Destroy(gameObject);
        }


        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDisable()
        {
            if (dialogueManager != null)
                dialogueManager.GetEnder().OnDialogueResolving -= SetStartObjectNull;
        }


        /// <inheritdoc/>
        public virtual void RegisterObject(Talkable obj)
        {
            if (obj is Talkable talkable)
            {
                if (actors.Contains(talkable))
                    return;

                actors.Add(talkable);

                talkable.SubscribeToStartConversation(Evaluate);
            }
            else
                UnityEngine.Debug.LogWarning("Object Tried To register to a Pooler that doesn't use it's type");
        }

        /// <inheritdoc/>
        public virtual void UnregisterObject(Talkable obj)
        {
            actors.Remove(obj);

            obj.UnsubscribeToStartConversation(Evaluate);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="talkable"></param>
        /// <param name="Conversation"></param>
        protected virtual void Evaluate(Talkable talkable, Dialogue[] Conversation)
        {
            startObject = talkable;

            if (ServiceLocator.HasService<IDialogueManager>())
            {
                if (Conversation[0] != null)
                    ServiceLocator.GetService<IDialogueManager>().StartDialogue(talkable, Conversation);
                else
                    ServiceLocator.GetService<IDialogueManager>().DebugNoCharacterDialogueCall(talkable);
            }
            else
                UnityEngine.Debug.LogWarning($"ALERT: {ToString()}: ActorManager could not get {nameof(IDialogueManager)}!");
        }


        /// <inheritdoc/>
        public virtual Talkable GetObjectByName(string name)
        {
            var actors = GetRegisteredObjects().OfType<Talkable>()
                .Where(talkable => talkable.gameObject.name == name);

            if (actors.Any())
                return actors.First();
            else if (name != tutorialCharacterFirstName && logActorsNotFound)
                UnityEngine.Debug.Log($"REPORT: {ToString()}: Couldn't find actor: {name}.");

            return null;
        }

        /// <inheritdoc/>
        public virtual List<Talkable> GetRegisteredObjects() => actors;
    }
}