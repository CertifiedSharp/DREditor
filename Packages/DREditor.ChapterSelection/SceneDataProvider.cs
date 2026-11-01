//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Saving;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.ChapterSelection
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    [CreateAssetMenu(menuName = "DREditor/Chapter Select/Scene Data Provider")]
    public class SceneDataProvider : SceneDataProviderBase
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public SerializableDictionary serializableDictionary = null;


        /// <inheritdoc/>
        public override object GetData() => serializableDictionary;
    }
}