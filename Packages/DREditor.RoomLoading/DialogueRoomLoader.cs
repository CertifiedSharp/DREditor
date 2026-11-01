//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.Loading;
using DREditor.EventObjects;
using DREditor.FPC;
using DREditor.Saving;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DREditor.RoomLoading
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(RoomLoadComponent) + ", " + 
        nameof(IDRELoadable) + ", " + nameof(IRoomLoadRoutine) + ", " + nameof(IRoomLoadResolver) +
        "\n\n")]
    public class DialogueRoomLoader : RoomLoadComponent, IDRELoadable, IRoomLoadRoutine, IRoomLoadResolver
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inDialogue = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inThreeDMode = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent hasMenuAccess = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inTrialMode = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent isLoadingFile = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent loadDialogueFound = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent dialogueLoaded = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEvent LoadTrialIndex = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string dialogueNameSaveKey = "DialogueName";


        /// <summary>
        ///
        /// </summary>
        protected Dictionary<string, object> saveDatas = new();


        /// <inheritdoc/>
        public virtual void Load(Dictionary<string, object> saveDatas) => this.saveDatas = saveDatas;


        /// <inheritdoc/>
        public virtual IEnumerator OnRoomLoadRoutine() => CheckLoadDialoguesRoutine();

        /// <summary>
        ///
        /// </summary>
        public virtual IEnumerator CheckLoadDialoguesRoutine()
        {
            if (!SaveFileIsInDialogue())
                yield break;

            LookForSaveDialogue();

            yield return WaitForSaveDialogueFound();

            SpawnSavedTrialMinigame();

            yield return WaitForDialogueLoadersCompleted();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void LookForSaveDialogue()
        {
            UnityEngine.Debug.Log($"REPORT: {ToString()}: Started Looking for Dialogue: " + saveDatas.Get<string>(dialogueNameSaveKey));

            var dialogueHolders = FindObjectsByType<MonoBehaviour>().OfType<IDialogueHolder>();

            foreach (IDialogueHolder holder in dialogueHolders)
                holder.CheckIfSaveDialogue();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void SpawnSavedTrialMinigame()
        {
            if (inTrialMode.Value)
                LoadTrialIndex.Raise();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator WaitForSaveDialogueFound()
        {
            yield return new WaitUntil(() => loadDialogueFound.Value);

            loadDialogueFound.SetValue(false);

            UnityEngine.Debug.Log($"REPORT: {ToString()}: Found Dialogue");
        }

        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        protected virtual IEnumerator WaitForDialogueLoadersCompleted()
        {
            var loaders = FindObjectsByType<MonoBehaviour>().OfType<IDialogueLoader>();

            foreach (var loader in loaders)
                yield return loader.LoadDialogueProcess(saveDatas);

            yield return new WaitUntil(() => dialogueLoaded.Value);

            dialogueLoaded.SetValue(false);

            UnityEngine.Debug.Log($"REPORT: {ToString()}: Dialogue has finished loading.");
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual bool SaveFileIsInDialogue() => isLoadingFile.Value && saveDatas.Get<string>(dialogueNameSaveKey) != "";


        /// <inheritdoc/>
        public virtual void OnRoomLoadResolve()
        {
            if (isLoadingFile.Value && !SaveFileIsInDialogue())
                WhenLoadingSaveFileWithoutDialogue();

            if (!inDialogue.Value && SaveFileIsInDialogue())
                inDialogue.SetValue(true);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void WhenLoadingSaveFileWithoutDialogue()
        {
            if (!inTrialMode.Value)
                hasMenuAccess.SetValue(true);

            if (inThreeDMode.Value)
                FindAnyObjectByType<MovePlayer>().enabled = true;
        }
    }
}