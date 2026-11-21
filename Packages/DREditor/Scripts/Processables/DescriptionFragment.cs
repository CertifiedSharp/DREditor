//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;
using UnityEngine.Serialization;

namespace DREditor.Processables
{
    /// <summary>
    /// Represents a portion of a description, or a fragment of a larger description.
    /// </summary>
    [System.Serializable]
    public class DescriptionFragment : IDescriptionFragment
    {
        /// <summary>
        /// The display name or identifier for the description.
        /// </summary>
        [FormerlySerializedAs("Title")]
        [Header("Represents a portion of a description, or a fragment of a larger description. \n")]
        [Tooltip("The display name or identifier for the description.")]
        [SerializeField] protected string displayName;

        /// <summary>
        /// The fragment of a description.
        /// </summary>
        [FormerlySerializedAs("Description")]
        [TextArea]
        [Tooltip("The fragment of a description.")]
        [SerializeField] protected string description;


        /// <summary>
        /// The display name or identifier for the description.
        /// </summary>
        public virtual string DisplayName { get => displayName; set => displayName = value; }

        /// <summary>
        /// The fragment of a description.
        /// </summary>
        public virtual string Description { get => description; set => description = value; }
    }
}