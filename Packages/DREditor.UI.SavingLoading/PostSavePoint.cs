//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using DREditor.RoomLoading;
using System;
using UnityEngine;

namespace DREditor.UI.SavingLoading
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    [CreateAssetMenu(menuName = "DREditor/Progression/Post Save Point", fileName = "Post Save Point")]
    public class PostSavePoint : ScriptableObject
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public string objective;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public string scene;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public bool changeObjective;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public int addstate;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public BoolWithEvent mode;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public string dialogueEvent = "";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public RoomLoadOptionsAsset roomLoadingOptions;
    }
}