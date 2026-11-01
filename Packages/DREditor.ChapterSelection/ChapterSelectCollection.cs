//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace DREditor.ChapterSelection
{
    /// <summary>
    /// 
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class ChapterSelectCollection<T> : ChapterSelectCollectionBase, IEnumerable<T>
    {
        /// <summary>
        /// 
        /// </summary>
        [FormerlySerializedAs("ChapterScenes")]
        [FormerlySerializedAs("ChapterDatas")]
        [Tooltip("")]
        [SerializeField] protected List<T> chapterSelectDetails = new();


        /// <summary>
        /// 
        /// </summary>
        public virtual List<T> Details => chapterSelectDetails;


        /// <summary>
        /// 
        /// </summary>
        /// <param name="i"></param>
        /// <returns></returns>
        public virtual T this[int i] => chapterSelectDetails[i];


        public override IList GetCollection() => Details;


        public virtual IEnumerator<T> GetEnumerator()
        {
            foreach (var detail in chapterSelectDetails)
                yield return detail;
        }

        IEnumerator IEnumerable.GetEnumerator() => GetEnumerator();
    }
}