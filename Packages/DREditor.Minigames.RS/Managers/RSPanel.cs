//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Minigames.Debates;
using System;
using System.Collections.Generic;

namespace DREditor.Minigames.RS.Managers
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class RSPanel : Panel, IMultiPanelHolder
    {
        /// <summary>
        ///
        /// </summary>
        public bool showEditorPanels = false;

        /// <summary>
        ///
        /// </summary>
        public int panelNum = 0;

        /// <summary>
        ///
        /// </summary>
        public List<MultiPanel> multiPanels = new();


        public bool ShowEditorPanels { get => showEditorPanels; set => showEditorPanels = value; }

        public int PanelNumber { get => panelNum; set => panelNum = value; }


        /// <inheritdoc/>
        public virtual List<MultiPanel> GetMultiPanels() => multiPanels;
    }
}