//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Progressing;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace DREditor.RoomBuilding.Old
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    [CreateAssetMenu(menuName = "DREditor/Room Building/Old/Room Builder", fileName = "Room Builder")]
    public class RoomBuilder : ScriptableObject
    {
        [FormerlySerializedAs("Shell")]
        public List<RoomData> Shells = new();

        public List<RoomSection> Sections = new();


        public RoomSection GetSectionWithObjective(Chapter c, Objective o)
        {
            foreach (RoomSection section in Sections)
                if (section.Chapter == c && section.HasDataFor(o))
                    return section;

            return null;
        }
    }

    //private void OnEnable()
    //{
    //    try
    //    {
    //        ProgressionDatabase pDB = Resources.Load<ProgressionDatabase>("Progression/ProgressionDatabase");
    //        if (Sections.Count < pDB.Chapters.Count)
    //        {
    //            /* Intended to add a new section when a new chapter was added, to be done later I guess
    //            for(int i = 0; i < (pDB.Chapters.Count - Sections.Count); i++)
    //            {
    //                UnityEngine.Debug.LogWarning("An Additional Section was added to match the current # of chapters");
    //                //UnityEngine.Debug.LogWarning("Chapters Count: " + pDB.Chapters.Count);
    //                //UnityEngine.Debug.LogWarning("Sections Count: " + Sections.Count);
    //                Sections.Add(new RoomSection(pDB.Chapters[i]));
    //            }
    //            */
    //        }

    //    }
    //    catch
    //    {
    //        UnityEngine.Debug.LogError("Progression Asset is missing or in the wrong place! " +
    //            "Make sure it's in: Resources/Progression and named ProgressionDatabase");
    //    }
    //}

}
