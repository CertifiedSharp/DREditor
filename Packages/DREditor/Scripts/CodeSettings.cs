//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using UnityEngine;

namespace DREditor
{
    /// <summary>
    ///
    /// </summary>
    public class CodeSettings : ProjectSettingsData<CodeSettings>
    {
        [Tooltip("")]
        [SerializeField] protected bool enforceXMLDocumentation = false;


        /// <summary>
        ///
        /// </summary>
        public virtual bool EnforceXMLDocumentation { get => enforceXMLDocumentation; }
    }
}