//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace DREditor.Progressing
{
    /// <summary>
    ///
    /// </summary>
    [System.Serializable]
    [CreateAssetMenu(menuName = "DREditor/Progression/Database", fileName = "ProgressionDatabase")]
    [MovedFrom(true, "DREditor.Progression")]
    public class ProgressionDatabase : ScriptableObject
    {
        public Chapter FTEChapter = null;
        public List<Chapter> Chapters = new();
        
        public int GetChapterIndex(Chapter c)
        {
            for (int i = 0; i < Chapters.Count; i++)
                if (Chapters[i] == c)
                    return i;

            return -1;
        }

        public Objective GetObjective(string o)
        {
            foreach(Chapter c in Chapters)
            {
                Objective ob = c.GetObjective(o);

                if (ob != null) 
                    return ob;
            }
            
            if (FTEChapter != null)
            {
                return FTEChapter.GetObjective(o);
            }

            return null;
        }


        public virtual int GetObjectiveIndex(int chapterIndex, Objective objective)
        {
            Chapter chapter = Chapters[chapterIndex];

            for (int i = 0; i < chapter.Objectives.Count; i++)
            {
                var obj = chapter.Objectives[i];

                if (obj.Description == objective.Description)
                    return i;
            }

            return -1;
        }

        // To be renamed probably.
        public virtual int GetObjectiveInt(int chapterIndex, Objective objective, int loadedProgressionIndex)
        {
            int objectiveIndex = GetObjectiveIndex(chapterIndex, objective);

            if (objectiveIndex >= 0)
                return objectiveIndex;

            int index = 0;

            try
            {
                index = loadedProgressionIndex;
            }
            catch
            {
                UnityEngine.Debug.LogWarning("ALERT: It seems there's faulty past progression info!");
            }

            return index;
        }


        public virtual string[] GetAllChapterNames() => Chapters.Select(chapter => chapter.title).ToArray();

        /// <summary>
        ///
        /// </summary>
        /// <param name="chapterIndex"></param>
        /// <returns></returns>
        public virtual string[] GetObjectiveNames(int chapterIndex)
        {
            if (chapterIndex < 0 || chapterIndex >= Chapters.Count)
                return null;

            var currentChapter = Chapters[chapterIndex];

            return currentChapter.Objectives
                .Select(objective => objective.Description)
                .ToArray();
        }
    }
}
