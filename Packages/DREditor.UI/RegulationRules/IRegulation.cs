//Completely Reworked Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
//Originally done by Willy Bee
using DREditor.Processables;
using UnityEngine;

namespace DREditor.UI.RegulationRules
{
    /// <summary>
    ///
    /// </summary>
    public interface IRegulation : IEventInitializable
    {
        /// <summary>
        ///
        /// </summary>
        Sprite Image { get; }

        /// <summary>
        ///
        /// </summary>
        string Text { get; }


        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        bool IsActiveRegulation();

        /// <summary>
        ///
        /// </summary>
        /// <param name="active"></param>
        void SetRegulationState(bool active);
    }
}