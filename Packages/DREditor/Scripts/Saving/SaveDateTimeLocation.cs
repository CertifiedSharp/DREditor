//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace DREditor.Saving
{
    /// <summary>
    ///
    /// </summary>
	[ClassHeader("")]
    public class SaveDateTimeLocation : MonoBehaviour, IDRESaveable
    {
        /// <inheritdoc/>
        public virtual void Save(Dictionary<string, object> saveData)
        {
            saveData.Set("SceneName", SceneManager.GetActiveScene().name);

            DateTime s = DateTime.Now;

            saveData.Set("Month", s.Month);

            saveData.Set("Day", s.Day);

            saveData.Set("Year", s.Year);

            saveData.Set("Hour", s.Hour);

            saveData.Set("Minute", s.Minute);

            saveData.Set("Second", s.Second);
        }
    }
}