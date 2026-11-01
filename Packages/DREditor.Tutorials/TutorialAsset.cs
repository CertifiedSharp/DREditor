//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace DREditor.Tutorials
{
    /// <summary>
    ///
    /// </summary>
    [CreateAssetMenu(menuName = "DREditor/Tutorials/Tutorial Asset", fileName = "Tutorial Asset")]
    [Serializable]
    public class TutorialAsset : ScriptableObject
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        public string title = "";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [FormerlySerializedAs("title")]
        public Sprite titleSprite = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public List<TutorialPage> pages = new();


        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual int GetPageCount() => pages.Count;

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public bool OnlySinglePage() => GetPageCount() == 1;
    }
}