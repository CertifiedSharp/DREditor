//Author: Benjamin "Sweden" Jillson : sweden_gaming For Project Eden's Garden
using DREditor.Editor.Dialogues;
using DREditor.Minigames.RS.Managers;
using System.Collections.Generic;

namespace DREditor.Minigames.RS.Editor
{
    /// <summary>
    ///
    /// </summary>
    public class RSDebugToolEditor : TrialDialogueDebugToolEditor<RSPanel>
    {
        /// <summary>
        ///
        /// </summary>
        protected RSBuilder RSBuilder => dia as RSBuilder;

        /// <inheritdoc/>
        protected override void RandomizeCamAnims()
        {
            if (RSBuilder != null)
                for (int i = 0; i < RSBuilder.Lines.Count; i++)
                    RandomizeSectionAnims(RSBuilder.Lines[i]);
            else
                base.RandomizeCamAnims();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="section"></param>
        protected virtual void RandomizeSectionAnims(RSGroup section)
        {
            if (section == null)
                return;

            RandomizePanelList(section.Panels);

            RandomizePanelList(section.ResponsePanels);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="panels"></param>
        protected virtual void RandomizePanelList(List<RSPanel> panels)
        {
            if (panels == null) return;

            foreach (var panel in panels)
                RandomizePanelAnims(panel);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="panel"></param>
        protected virtual void RandomizePanelAnims(RSPanel panel)
        {
            if (panel == null)
                return;

            panel.CamAnimIdx = UnityEngine.Random.Range(0, cameraAnimDatabase.anims.Count);

            RandomizeMultiPanelAnims(panel);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="panel"></param>
        protected virtual void RandomizeMultiPanelAnims(RSPanel panel)
        {
            foreach (var multiPanel in panel.multiPanels)
                if (multiPanel != null)
                    multiPanel.CamAnimIdx = UnityEngine.Random.Range(0, cameraAnimDatabase.anims.Count);
        }

        /// <inheritdoc/>
        protected override void DebugSetTCOToChar()
        {
            if (RSBuilder != null)
                SetTCOToChar();
            else
                base.DebugSetTCOToChar();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void SetTCOToChar()
        {
            foreach (RSGroup section in RSBuilder.Lines)
                foreach (RSPanel panel in section.Panels)
                    if (panel.Speaker == debugSpeaker)
                        panel.Co = coDebug;
        }
    }
}