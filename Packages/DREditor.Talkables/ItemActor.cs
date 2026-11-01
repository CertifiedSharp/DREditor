//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.Events;
using DREditor.LocalDialogues;
using DREditor.Services;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.Talkables
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(Talkable) +
        "\n\n")]
    public class ItemActor : Talkable
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string toggleBlurEventString = "ToggleBlur";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public bool isStaticItem = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<GameObject> observeAddOnObjects = new();


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Tooltip("")]
        [SerializeField] protected int defaultLayerIndex = 0;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int itemLayerIndex = 11;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int addOnLayerIndex = 16;


        /// <inheritdoc/>
        protected override void Start()
        {
            base.Start();

            LocalDialogueHandlerBase.BookOfTriggers.Add(gameObject.name, this);
        }

        /// <inheritdoc/>
        protected override void OnDestroy()
        {
            LocalDialogueHandlerBase.BookOfTriggers.Remove(gameObject.name);

            base.OnDestroy();
        }


        /// <summary>
        ///
        /// </summary>
        public virtual void ClearItemData()
        {
            if (!isStaticItem)
            {
                conversations.Clear();

                selectable = true;
            }
        }


        /// <inheritdoc/>
        public override void SelectObject()
        {
            DialogueEventSystem.TriggerEvent(toggleBlurEventString, false);

            Dialogue[] conversation = new Dialogue[1];

            Dialogue dialogue = EvaluateDialogueWhereTriggeredFlagIsItem();

            conversation[0] = dialogue != null ?
                dialogue : ServiceLocator.GetService<LocalDialogueHandlerBase>().EvaluateDialogue(conversations);

            InvokeConversation(conversation);
        }

        /// <summary>
        ///
        /// </summary>
        /// <remarks>
        ///
        /// </remarks>
        /// <returns></returns>
        protected virtual Dialogue EvaluateDialogueWhereTriggeredFlagIsItem()
        {
            foreach (LocalDialogue localDialogue in conversations)
                if (!localDialogue.triggered &&
                    localDialogue.requiredProgressionFlag != "" &&
                    ServiceLocator.GetService<IObjectPooler<Talkable>>() is IObjectPooler<Talkable> pooler &&
                    pooler.GetObjectByName(localDialogue.requiredProgressionFlag) is Talkable talkable)
                    return GetDialogueThroughEvaluation(localDialogue, talkable.conversations.ContainsTriggeredDialogue());

            return null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="localDialogue"></param>
        /// <param name="evaluation"></param>
        /// <returns></returns>
        protected virtual Dialogue GetDialogueThroughEvaluation(LocalDialogue localDialogue, bool evaluation)
        {
            if (evaluation)
                localDialogue.SetTriggered();

            return evaluation ? localDialogue.dialogue : localDialogue.dialogueIfFlagNotMet;
        }


        /// <inheritdoc/>
        public override void SetSelectability(bool selectable)
        {
            // Added 1/1/2025 as a way to disable items if they have no usable dialogue. 
            // however, below code might have an adverse reaction and functionality to disable
            // items if there's no usuable dialogue should be added during the next Rework.
            //if (RoomConvo.Count == 0)
            //{
            //    to = false;
            //}
            // Revisited this during the 2025 Rework 10/16/2025 and I'm going to leave this for another rework.

            if (selectable == base.selectable && gameObject.layer == (selectable ? itemLayerIndex : defaultLayerIndex))
                return;

            gameObject.layer = selectable ? itemLayerIndex : defaultLayerIndex;

            foreach (Transform child in gameObject.transform)
                child.gameObject.layer = selectable ? itemLayerIndex : defaultLayerIndex;

            foreach (GameObject observeAddOnObject in observeAddOnObjects)
                observeAddOnObject.layer = selectable ? addOnLayerIndex : defaultLayerIndex;

            base.SetSelectability(selectable);
        }

        /// <summary>
        ///
        /// </summary>
        [ContextMenu("Create Head, Body, and Set Layer")]
        public virtual void EditorInstantiation()
        {
            DisplayName = gameObject.name;

            gameObject.layer = itemLayerIndex;

            gameObject.AddComponent<BoxCollider>();

            GameObject head = CreateChild("_Head");

            GameObject body = CreateChild("_Body");

            SetHeadAndBody(head.transform, body.transform);

            body.transform.position = new Vector3(0, 0, -2);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="suffix"></param>
        /// <returns></returns>
        protected virtual GameObject CreateChild(string suffix)
        {
            string objectName = gameObject.name.Replace(" ", "_");

            GameObject child = new(objectName + suffix);

            child.transform.parent = gameObject.transform;

            return child;
        }
    }
}