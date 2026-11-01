//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Editor.Dialogues;
using DREditor.Editor.Utility;

namespace DREditor.Minigames.Debates.Editor
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class MultiPanelLineEditor<T> : PanelLineEditor<T> where T : Panel, new()
    {
        /// <summary>
        ///
        /// </summary>
        protected readonly MultiPanelEditor multiPanelEditor = new();


        protected override void DisplayLowerLine()
        {
            base.DisplayLowerLine();

            if (Lines[i] is IMultiPanelHolder panel)
                panel.ShowEditorPanels = multiPanelEditor.EditMultiPanels(panel.ShowEditorPanels, panel.GetMultiPanels(),
                dia, "Additional Panel Movements");
        }

        protected override void DisplayCharacterPanel() => 
            CustomLayoutUtility.DisplayVertical(base.DisplayCharacterPanel, DisplayMultiPanelNumber);

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayMultiPanelNumber()
        {
            IMultiPanelHolder mtl = (IMultiPanelHolder)Lines[i];

            mtl.PanelNumber = HandyFields.IntField("Panel Num: ", mtl.PanelNumber, 50);
        }
    }
}