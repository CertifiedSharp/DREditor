//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Saving;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.UI.RegulationRules
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(IDRESaveable) + ", " + nameof(IDRELoadableManager) +
        "\n\n")]
    public class RegulationSaveHandler : MonoBehaviour, IDRESaveable, IDRELoadableManager
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected ScriptableObject regulationsDatabase = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected string saveKey = "ActiveRegulations";


        /// <summary>
        ///
        /// </summary>
        protected IRegulationsUIRepository regulationsRepository;


        /// <inheritdoc/>
        public virtual void Save(Dictionary<string, object> saveDatas)
        {
            if (!RepositoryValid())
                return;

            List<IRegulation> regulations = regulationsRepository?.GetRegulations();

            if (regulations == null)
                return;

            bool[] bools = new bool[regulations.Count];

            for (int i = 0; i < regulations.Count; i++)
                bools[i] = regulations[i].IsActiveRegulation();

            saveDatas.Add(saveKey, bools);
        }

        /// <inheritdoc/>
        public virtual void LoadManager(Dictionary<string, object> saveDatas, bool inChapterSelect)
        {
            if (!RepositoryValid())
                return;

            List<IRegulation> regulations = regulationsRepository?.GetRegulations();

            bool[] bools = saveDatas.Get<bool[]>(saveKey);

            if (bools != null)
                for (int i = 0; i < regulations.Count && i < bools.Length; i++)
                    regulations[i].SetRegulationState(bools[i]);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual bool RepositoryValid() => regulationsRepository != null || InitializeRepository();

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected virtual bool InitializeRepository()
        {
            if (regulationsDatabase is IRegulationsUIRepository repository)
                regulationsRepository = repository;
            else if (regulationsDatabase != null)
                UnityEngine.Debug.LogError($"ALERT: The database assigned must inherit from {nameof(IRegulationsUIRepository)}!");
            else
                UnityEngine.Debug.LogWarning($"ALERT: There is no regulations database assigned on game object {gameObject.name}! " +
                    $"Regulations will not save or load properly until this is corrected.");

            return regulationsDatabase is IRegulationsUIRepository;
        }
    }
}