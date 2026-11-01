//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DREditor.Saving.Handlers
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(SceneEventListener) + ", " + nameof(IDRESaveable) + ", " + nameof(IDRELoadableManager) +
        "\n\n")]
    public class ModeHandler : SceneEventListener, IDRESaveable, IDRELoadableManager
    {
        /// <summary>
        ///
        /// </summary>
        [Space]

        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected string saveKey = "mode";
        
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent startingMode = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] List<BoolWithEvent> modes = new();


        /// <inheritdoc/>
        protected override void OnEnable()
        {
            base.OnEnable();

            Response.AddListener(StartNewGameFile);

            foreach (BoolWithEvent mode in modes)
                mode.AddListener((modeValue) => EvaluateMode(mode, modeValue));
        }


        /// <inheritdoc/>
        protected override void OnDisable()
        {
            base.OnDisable();

            Response.RemoveListener(StartNewGameFile);

            foreach (BoolWithEvent mode in modes)
                mode.RemoveListener((modeValue) => EvaluateMode(mode, modeValue));
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void StartNewGameFile() => startingMode.SetValue(true);

        /// <summary>
        ///
        /// </summary>
        /// <param name="boolEvent"></param>
        /// <param name="boolEventsValue"></param>
        protected virtual void EvaluateMode(BoolWithEvent boolEvent, bool boolEventsValue)
        {
            if (boolEventsValue)
                foreach (BoolWithEvent mode in modes)
                    if (mode != boolEvent)
                        mode.SetValue(false);
        }


        /// <inheritdoc/>
        public virtual void Save(Dictionary<string, object> saveData)
            => saveData.Set(saveKey, GetCurrentMode());

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual int GetCurrentMode()
        {
            BoolWithEvent currentMode = modes.Where(mode => mode.Value).First();

            return modes.IndexOf(currentMode);
        }

        /// <inheritdoc/>
        public virtual void LoadManager(Dictionary<string, object> saveDatas, bool inChapterSelect)
            => modes[saveDatas.Get<int>(saveKey)].SetValue(true);
    }
}