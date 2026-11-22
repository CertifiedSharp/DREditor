//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using UnityEngine;

namespace DREditor
{
    /// <summary>
    /// Displays a white divider line in the inspector for visual organization.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public sealed class DividerAttribute : PropertyAttribute { }
}