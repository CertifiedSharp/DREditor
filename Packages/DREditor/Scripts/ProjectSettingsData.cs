//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using System.Text.RegularExpressions;
using System;
using UnityEngine;

namespace DREditor
{
    /// <summary>
    /// The base class for creating project settings in DREditor.
    /// </summary>
    /// <typeparam name="T">Type of data class containing the settings for this asset.</typeparam>
    [Serializable]
    public abstract class ProjectSettingsData<T> : ScriptableObject where T : ProjectSettingsData<T>
    {
        /// <summary>
        /// The function used to request the active project settings.
        /// </summary>
        public static System.Func<T> OnRequestInstance { get; set; }

        /// <summary>
        /// Requests the active instance of the project settings.
        /// </summary>
        public static T Instance => OnRequestInstance?.Invoke();


        /// <summary>
        /// Creates the Settings path. Uses the Class label as the default.
        /// </summary>
        public virtual string SettingsPath => "Project/DREditor/" + GetClassLabel();

        /// <summary>
        /// The label to use if using the class label is not desired.
        /// </summary>
        public virtual string MenuLabel => null;

        /// <summary>
        /// Creates the label for the class to display in the Project Settings window.
        /// </summary>
        /// <returns>The string label for the class to display in the Project Settings window.</returns>
        /// <remarks>I might be able to set this to NicifyObjectName. Remind me if so.</remarks>
        public virtual string GetClassLabel() => Regex.Replace(GetType().Name, "([A-Z])", " $1").Trim().Replace("Settings", "");
    }
}