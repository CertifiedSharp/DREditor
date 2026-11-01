//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;
using UnityEngine.InputSystem;

namespace DREditor.UI.UIControls
{
    /// <summary>
    ///
    /// </summary>
    public interface IControlRowData
    {
        /// <summary>
        ///
        /// </summary>
        Sprite Left { get; }

        /// <summary>
        ///
        /// </summary>
        Sprite Center { get; }

        /// <summary>
        ///
        /// </summary>
        Sprite Right { get; }

        /// <summary>
        /// 
        /// </summary>
        InputActionReference ActionReference { get; }

        /// <summary>
        ///
        /// </summary>
        string ControlKey { get; }

        /// <summary>
        ///
        /// </summary>
        string ControlName { get; }
    }
}