//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace DREditor.EventObjectServices
{
    /// <summary>
    ///
    /// </summary>
	[ClassHeader("")]
    public class ObjectActiveOnChapterIndex : SceneEventListener
    {
        /// <summary>
        ///
        /// </summary>
        [Space]

        [Space]

        [Header("")]

        [Tooltip("")]
        [SerializeField] protected IntWithEvent currentChapterIndex = null;

        /// <summary>
        ///
        /// </summary>
        [Space]

        [Tooltip("")]
        [SerializeField] protected bool setObjectsActiveTo = false;

        /// <summary>
        ///
        /// </summary>
        [Space]

        [FormerlySerializedAs("visibleChapterIndex")]
        [Tooltip("")]
        [SerializeField] protected List<int> visibleChapterIndexes = new();

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<GameObject> gameObjects = new();


        /// <inheritdoc/>
        public override void OnEventRaised()
        {
            base.OnEventRaised();

            CheckLoad();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void CheckLoad()
        {
            if (ChapterMatchesVisibilityIndex())
                SetGameObjectsActive();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool ChapterMatchesVisibilityIndex()
            => visibleChapterIndexes.Contains(currentChapterIndex.Value);

        /// <summary>
        ///
        /// </summary>
        protected virtual void SetGameObjectsActive()
        {
            foreach (var go in gameObjects)
                go.SetActive(setObjectsActiveTo);
        }
    }
}