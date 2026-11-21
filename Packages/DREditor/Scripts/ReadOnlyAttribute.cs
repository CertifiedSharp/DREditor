//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using UnityEngine;

namespace DREditor
{
    /// <summary>
    /// Disables the ability to edit this field in the inspector.
    /// </summary>
    [AttributeUsage(AttributeTargets.Field, AllowMultiple = true)]
    public sealed class ReadOnlyAttribute : PropertyAttribute { }
}