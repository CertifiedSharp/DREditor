//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using UnityEngine;

namespace DREditor
{
    /// <summary>
    /// Settings for DREditor projects relating to programming.
    /// </summary>
    public class CodeSettings : ProjectSettingsData<CodeSettings>
    {
        /// <summary>
        /// When true, tells visual studio to enforce XML documentation rules in order to keep code clean.
        /// </summary>
        [Tooltip("When true, tells visual studio to enforce XML documentation rules in order to keep code clean.")]
        [SerializeField] protected bool enforceXMLDocumentation = false;


        /// <summary>
        /// When true, tells visual studio to enforce XML documentation rules in order to keep code clean.
        /// </summary>
        public virtual bool EnforceXMLDocumentation { get => enforceXMLDocumentation; }
    }
}