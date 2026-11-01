//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.ChapterSelection
{
    /// <summary>
    /// 
    /// </summary>
    public abstract class ChapterSelectCollectionBase : ScriptableObject
    {
        /// <summary>
        /// 
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="collection"></param>
        /// <returns></returns>
        public virtual bool TryGetCollection<T>(out List<T> collection)
        {
            collection = GetCollection() is List<T> castedCollection ? castedCollection : null;

            bool result = collection != null;

            if (!result)
                Debug.LogWarning($"ALERT: {name}'s collection is not of type: {typeof(T).Name}!");

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public abstract IList GetCollection();


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual bool IsEmpty() => CollectionCountEmpty() || ChildrenAreEmpty();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual bool ChildrenAreEmpty()
        {
            IList collection = GetCollection();

            foreach (var item in collection)
                if (item is ChapterSelectCollectionBase childCollection && childCollection.IsEmpty())
                    return true;

            return false;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        private bool CollectionCountEmpty() => GetDataCount() == 0;


        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual int GetDataCount() => GetCollection().Count;
    }
}