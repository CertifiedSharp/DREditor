//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DREditor.UI.UIControls
{
    /// <summary>
    ///
    /// </summary>
    [CreateAssetMenu(menuName = "DREditor/Controls UI/Database", fileName = "ControlsDatabase")]
    public class ControlsDatabase : ScriptableObject, IControlsUIRepository
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Header("")]
        [Header("")]
        [Space]
        [Tooltip("")]
        [SerializeField] protected List<ControlPair> controls = new();


        /// <inheritdoc/>
        public virtual IControlsUIPanel GetPanel(string id)
        {
            var panels = controls.Where(pair => pair.name == id);

            if (panels.Count() == 0)
                return null;

            return panels.ElementAt(0).panel;
        }
    }
}