//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;
using UnityEngine.Serialization;

namespace DREditor.Processables
{
    [System.Serializable]
    public class DescriptionFragment : IDescriptionFragment
    {
        /// <summary>
        ///
        /// </summary>
        [FormerlySerializedAs("Title")]
        [SerializeField] protected string displayName;

        /// <summary>
        ///
        /// </summary>
        [FormerlySerializedAs("Description")]
        [TextArea]
        [SerializeField] protected string description;


        /// <summary>
        ///
        /// </summary>
        public virtual string DisplayName { get => displayName; set => displayName = value; }

        /// <inheritdoc/>
        public virtual string Description { get => description; set => description = value; }
    }
}