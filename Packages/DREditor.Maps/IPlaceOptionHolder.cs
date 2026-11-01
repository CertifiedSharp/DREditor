//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.Maps
{
    /// <summary>
    ///
    /// </summary>
    public interface IPlaceOptionHolder
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="shiftNum"></param>
        /// <param name="placeNames"></param>
        void PopulatePlaceUIText(int shiftNum, List<string> placeNames);

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        PlaceManager GetCurrentlyHoveredPlace();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        int GetMaxShiftIndex();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        GameObject GetTopOption();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        GameObject GetBottomOption();

        /// <summary>
        ///
        /// </summary>
        void EnableSelectablesOnValidText();

        /// <summary>
        ///
        /// </summary>
        void DisableSelectableOptions();
    }
}