//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using System.Text.RegularExpressions;
using System;
using UnityEngine;

namespace DREditor
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public abstract class ProjectSettingsData<T> : ScriptableObject where T : ProjectSettingsData<T>
    {
        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        public static System.Func<T> OnRequestInstance { get; set; }

        /// <summary>
        ///
        /// </summary>
        public static T Instance => OnRequestInstance?.Invoke();


        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        public virtual string SettingsPath => "Project/DREditor/" + GetClassLabel();

        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        public virtual string MenuLabel => null;

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual string GetClassLabel() => Regex.Replace(GetType().Name, "([A-Z])", " $1").Trim().Replace("Settings", "");
    }
}