//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues.Events;
using DREditor.Processables;
using DREditor.Services;
using System;
using UnityEngine;

namespace DREditor.ChapterSelection
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class UnlockChapterSelectScene : IDialogueEvent
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [ReadOnly]
        [Tooltip("")]
        [SerializeField] protected string eventString = "UnlockChapterSelectScene";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneDataProviderBase unlockScene;


        /// <inheritdoc/>
        public virtual void TriggerDialogueEvent()
        {
            DialogueEventSystem.TriggerEventIfListening(eventString, unlockScene);

            UnlockIdentifiableScene();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void UnlockIdentifiableScene()
        {
            if (unlockScene is IIdentifiable identifiable && ServiceLocator.HasService(out IChapterSelectData data))
                data?.Add(identifiable.ID);
        }
    }
}