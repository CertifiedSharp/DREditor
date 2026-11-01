//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.Trials
{
    /// <summary>
    ///
    /// </summary>
	[ClassHeader("")]
    public class ChapterDeathHandler : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent isLoadingFile = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected IntWithEvent currentChapterIndex = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] List<DeadTrialChapter> deadTrialChapters = new();


        protected virtual void Start()
        {
            if (isLoadingFile.Value)
                StartCoroutine(WaitForFileToLoad());
            else
                InitializeDeadCharacters();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator WaitForFileToLoad()
        {
            yield return new WaitUntil(() => !isLoadingFile.Value);

            InitializeDeadCharacters();

            yield break;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeDeadCharacters()
        {
            foreach (var chapter in deadTrialChapters)
                if (chapter != null && chapter.GetChapterIndex() == currentChapterIndex.Value)
                    chapter.SetUpDeadCharacters();
        }
    }
}