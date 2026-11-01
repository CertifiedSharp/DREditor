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
    [CreateAssetMenu(menuName = "DREditor/Chapter Select/Database", fileName = "Chapter Select Database")]
    public class ChapterSelectDatabase : ChapterSelectCollection<ChapterSelectData>
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="queue"></param>
        /// <returns></returns>
        public virtual object GetChapterSelectData(Queue<int> queue)
        {
            int selectedChapter = queue.Dequeue();

            var data = chapterSelectDetails[selectedChapter].GetChapterSelectData(queue);

            return data;
        }
    }
}