//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Saving;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.UI.Buttons
{
    /// <summary>
    ///
    /// </summary>
	[ClassHeader("")]
    public class MoveToLoadedScene : MoveToScene, IDRELoadableManager
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected string sceneNameSaveKey = "SceneName";


        /// <summary>
        ///
        /// </summary>
        protected virtual void OnValidate() => SetSceneName("");

        /// <inheritdoc/>
        public virtual void LoadManager(Dictionary<string, object> saveDatas, bool inChapterSelect)
            => SetSceneName(saveDatas.Get<string>(sceneNameSaveKey));
    }
}