//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.UI.ReportCards;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DREditor.FreeTimeReportCards
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(ReportCardControllableComponent) + ", " + nameof(FTEReportCardBase) +
        "\n\n")]
    public class FTEReportCard : FTEReportCardBase
    {
        /// <summary>
        ///
        /// </summary>
        protected IEnumerable<IUpdateFTEUITab> updateableTabs;

        /// <summary>
        ///
        /// </summary>
        protected IEnumerable<IDisplayFTEUITab> displayableTabs;

        /// <summary>
        ///
        /// </summary>
        protected IEnumerable<IResetFTEUITab> resetableTabs;


        /// <inheritdoc/>
        protected override void Start()
        {
            base.Start();

            var allComponents = GetComponentsInChildren<Component>();

            updateableTabs = allComponents.OfType<IUpdateFTEUITab>();

            displayableTabs = allComponents.OfType<IDisplayFTEUITab>();

            resetableTabs = allComponents.OfType<IResetFTEUITab>();
        }


        /// <inheritdoc/>
        protected override void UpdateUnlockedFTETabs()
        {
            base.UpdateUnlockedFTETabs();

            foreach (var tab in updateableTabs)
                tab?.UpdateUnlockedFTETabs(GetCharacterAffinity());
        }

        /// <inheritdoc/>
        protected override void DisplayFTETabData()
        {
            base.DisplayFTETabData();

            foreach (var tab in displayableTabs)
                tab?.DisplayFTETabData(fteIndex);
        }

        /// <inheritdoc/>
        public override void ResetComponent()
        {
            base.ResetComponent();

            foreach (var tab in resetableTabs)
                tab?.ResetComponent(!character.NotFTECharacter);
        }
    }
}