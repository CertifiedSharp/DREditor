//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;
using UnityEngine.Serialization;

namespace DREditor.UI.UIControls
{
    /// <summary>
    ///
    /// </summary>
    [CreateAssetMenu(menuName = "DREditor/Controls UI/Control Panel", fileName = "Control Panel")]
    public class ControlsUIPanel : ScriptableObject, IControlsUIPanel
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Header("")]
        [Space]
        [Tooltip("")]
        [FormerlySerializedAs("PanelTitle")]
        [SerializeField] protected string panelTitle;

        /// <summary>
        ///
        /// </summary>
        [Space]

        [Tooltip("")]
        [FormerlySerializedAs("ControlRows")]
        [SerializeField] protected ControlPanelRow[] controlRows;

        /// <summary>
        ///
        /// </summary>
        [Space]

        [Tooltip("")]
        [FormerlySerializedAs("SystemDescription")]
        [TextArea(minLines: 30, maxLines: 30)]
        [SerializeField] protected string systemDescription;


        public string Title => panelTitle;

        public IControlRowData[] Rows => controlRows;

        public string SystemDescription => systemDescription;
    }
}