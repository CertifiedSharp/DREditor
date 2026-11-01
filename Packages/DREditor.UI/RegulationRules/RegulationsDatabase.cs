//Completely Reworked Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
//Originally done by Willy Bee
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DREditor.UI.RegulationRules
{
    /// <summary>
    ///
    /// </summary>
    [System.Serializable]
    [CreateAssetMenu(menuName = "DREditor/Regulations/Database", fileName = "RegulationsDatabase")]
    public class RegulationsDatabase : ScriptableObject, IRegulationsUIRepository
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Space]
        [Tooltip("")]
        [SerializeField] protected List<Regulation> allRegulations = new();


        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual List<IRegulation> GetRegulations() => allRegulations.Cast<IRegulation>().ToList();

        /// <summary>
        ///
        /// </summary>
        /// <param name="regulation"></param>
        public virtual void AddRegulation(IRegulation regulation) => allRegulations.Add(regulation as Regulation);

        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        public virtual void ClearEmptyValues() => allRegulations.RemoveAll(item => item == null);



        /// <inheritdoc/>
        public virtual void Initialize() => GetRegulations().ForEach(regulation => regulation.Initialize());

        /// <inheritdoc/>
        public virtual void Resolve() => GetRegulations().ForEach(regulation => regulation.Resolve());

        protected virtual void OnValidate() => ClearEmptyValues();
    }
}