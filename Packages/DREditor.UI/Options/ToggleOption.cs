//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using DREditor.EventObjectServices;
using System;
using UnityEngine;

namespace DREditor.UI.Options
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    [CreateAssetMenu(menuName = "DREditor/Event Objects/EventOptions/Toggle", fileName = "New Toggle Option")]
    public class ToggleOption : OptionWithEvent<bool, BoolEvent> { }
}