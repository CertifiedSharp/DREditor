//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Processables;
using System;
using UnityEngine;

namespace DREditor.ChapterSelection
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public abstract class SceneDataProviderBase : ScriptableObject, IIdentifiable
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        public int selectScene;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int id = -1;

        public virtual int ID => id;


        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public abstract object GetData();
    }
}