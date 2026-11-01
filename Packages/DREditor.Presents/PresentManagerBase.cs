//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Processables;
using DREditor.Services;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DREditor.Presents
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class PresentManagerBase<T> : ServiceHolder<IPresentManager>, IPresentManager where T : PresentSaveBase, IIdentifiable
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected List<T> presentDatas = new();


        public virtual int Count { get => presentDatas.Count; }


        public abstract void AddPresent(int id, int amt = 1);

        public virtual T1 Get<T1>(int id) where T1 : IIdentifiable
            => presentDatas.OfType<T1>().ToList().Find(i => i.ID == id);

        public virtual bool HadPresent(int id) => presentDatas.Any(i => i.ID == id);
    }
}