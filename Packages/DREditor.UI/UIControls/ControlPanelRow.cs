//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;

namespace DREditor.UI.UIControls
{
    /// <summary>
    ///
    /// </summary>
    [System.Serializable]
    public class ControlPanelRow : IControlRowData
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [FormerlySerializedAs("Left")]
        [SerializeField] protected Sprite left;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [FormerlySerializedAs("Center")]
        [SerializeField] protected Sprite center;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [FormerlySerializedAs("Right")]
        [SerializeField] protected Sprite right;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected InputActionReference actionReference;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string controlKey;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [FormerlySerializedAs("Description")]
        [SerializeField] protected string controlName;


        public virtual Sprite Left => left;

        public virtual Sprite Center => center;

        public virtual Sprite Right => right;

        public virtual InputActionReference ActionReference => actionReference;

        public virtual string ControlKey => controlKey;

        public virtual string ControlName => controlName;
    }
}