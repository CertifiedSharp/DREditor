//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Saving;
using System;
using System.Collections.Generic;
using System.Globalization;
using TMPro;
using UnityEngine;

namespace DREditor.UI.SavingLoading
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(SaveDataDisplayerBase) +
        "\n\n")]
    public class SaveDateDisplayer : SaveDataDisplayerBase
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool debugLogs = false;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected TextMeshProUGUI date = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected TextMeshProUGUI timeOfSave = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected string dateFormat = "MM/dd/yyyy";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string timeFormat = "HH:mm:ss";


        /// <summary>
        ///
        /// </summary>
        protected Dictionary<string, object> saveDatas;

        /// <summary>
        ///
        /// </summary>
        protected readonly string yearKey = "Year";

        /// <summary>
        ///
        /// </summary>
        protected readonly string monthKey = "Month";

        /// <summary>
        ///
        /// </summary>
        protected readonly string dayKey = "Day";

        /// <summary>
        ///
        /// </summary>
        protected readonly string hourKey = "Hour";

        /// <summary>
        ///
        /// </summary>
        protected readonly string minuteKey = "Minute";

        /// <summary>
        ///
        /// </summary>
        protected readonly string secondKey = "Second";


        /// <inheritdoc/>
        public override void Display(Dictionary<string, object> saveDatas, int id)
        {
            this.saveDatas = saveDatas;

            DateTime dateTime = GetSaveFileDateTime();

            date.text = dateTime.ToString(dateFormat, CultureInfo.InvariantCulture);

            timeOfSave.text = dateTime.ToString(timeFormat, CultureInfo.InvariantCulture);

            this.saveDatas = null;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual DateTime GetSaveFileDateTime()
        {
            if (debugLogs)
                LogDateTime();

            return new DateTime(
                saveDatas.Get<int>(yearKey),
                saveDatas.Get<int>(monthKey),
                saveDatas.Get<int>(dayKey),
                saveDatas.Get<int>(hourKey), 
                saveDatas.Get<int>(minuteKey), 
                saveDatas.Get<int>(secondKey));
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void LogDateTime()
        {
            UnityEngine.Debug.Log(saveDatas.Values.Count);

            UnityEngine.Debug.Log(
                saveDatas.Get<int>(yearKey) 
                + " " + saveDatas.Get<int>(monthKey)
                + " " + saveDatas.Get<int>(dayKey)
                + " " + saveDatas.Get<int>(hourKey)
                + " " + saveDatas.Get<int>(minuteKey)
                + " " + saveDatas.Get<int>(secondKey));
        }

        /// <inheritdoc/>
        public override void OnSaveNotFound(int id)
        {
            date.text = "";

            timeOfSave.text = "";
        }
    }
}