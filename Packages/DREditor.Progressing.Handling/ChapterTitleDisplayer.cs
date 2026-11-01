//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using DREditor.Services;
using TMPro;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace DREditor.Progressing.Handling
{
    /// <summary>
    ///
    /// </summary>
	[ClassHeader("")]
    [MovedFrom(true, "DREditor.Progression.Handling", "DREditor.Progression.Handling")]
    public class ChapterTitleDisplayer : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected IntWithEvent currentChapterIndex = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected TextMeshProUGUI title = null;


        protected virtual void OnEnable() => currentChapterIndex.AddListener(UpdateTitle);

        protected virtual void OnDisable() => currentChapterIndex.RemoveListener(UpdateTitle);
        
        protected virtual void Start() => UpdateTitle();

        /// <summary>
        ///
        /// </summary>
        protected virtual void UpdateTitle(int index = 0) 
            => title.text = ServiceLocator.GetService<ProgressionManagerBase>().GetCurrentChapter().title;
    }
}