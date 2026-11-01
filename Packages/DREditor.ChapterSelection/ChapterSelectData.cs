//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.ChapterSelection
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    [CreateAssetMenu(menuName = "DREditor/Chapter Select/Data", fileName = "Chapter Select Data")]
    public class ChapterSelectData : ChapterSelectCollection<SceneDataProviderBase>
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        public int chapter;


        /// <summary>
        ///
        /// </summary>
        /// <param name="queue"></param>
        /// <returns></returns>
        public virtual object GetChapterSelectData(Queue<int> queue)
        {
            int selectedScene = queue.Dequeue();

            int useIndex = GetIndexOfSelectScene(selectedScene);

            var data = chapterSelectDetails[useIndex];

            return data;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual int GetIndexOfSelectScene(int selectedEnd)
        {
            foreach (var sceneData in chapterSelectDetails)
                if (sceneData.selectScene == selectedEnd)
                    return chapterSelectDetails.IndexOf(sceneData);

            UnityEngine.Debug.LogWarning("Chapter Select: ALERT: " +
                "Using default end because it couldn't find a matching select scene!");

            return selectedEnd;
        }
    }
}