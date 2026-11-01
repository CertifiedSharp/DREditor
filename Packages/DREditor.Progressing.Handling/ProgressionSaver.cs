//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using DREditor.Saving;
using DREditor.Services;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting.APIUpdating;

namespace DREditor.Progressing.Handling
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(IDRESaveable) +
        "\n\n")]
    [MovedFrom(true, "DREditor.Progression.Handling", "DREditor.Progression.Handling")]
    public class ProgressionSaver : MonoBehaviour, IDRESaveable
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected IntWithEvent currentChapterIndex = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected string chapterKey = "chapter";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string objectiveKey = "CurrentObjective";


        /// <inheritdoc/>
        public virtual void Save(Dictionary<string, object> saveData)
        {
            saveData.Set(chapterKey, currentChapterIndex.Value);

            saveData.Set(objectiveKey, ServiceLocator.GetService<ProgressionManagerBase>().currentObjective);
        }
    }
}