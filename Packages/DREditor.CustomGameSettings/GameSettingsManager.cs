//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.SaveManaging;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DREditor.CustomSettings
{
    /// <summary>
    ///
    /// </summary>
	[ClassHeader("")]
    public class GameSettingsManager : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected string gameSettingsFileName = "GameSettings";


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected GameSettingsObject defaultGameSettings = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected GameSettings currentGameSettings;


        /// <summary>
        ///
        /// </summary>
        protected List<ICustomGameSettingsHandler> gameSettingsHandlers = new();


        /// <summary>
        ///
        /// </summary>
        protected virtual void Start()
        {
            gameSettingsHandlers = FindObjectsByType<MonoBehaviour>().OfType<ICustomGameSettingsHandler>().ToList();

            Load(GetData());
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="data"></param>
        protected virtual void Load(GameSettings data)
        {
            if (data == null)
            {
                UnityEngine.Debug.LogError($"{nameof(GameSettingsManager)}: Game Settings Data was Null!");

                return;
            }

            UnityEngine.Debug.Log($"{nameof(GameSettingsManager)}: Loading Game Settings");

            currentGameSettings = data;

            Application.targetFrameRate = currentGameSettings.frameRate;

            QualitySettings.vSyncCount = currentGameSettings.vSyncCount;

            foreach (var handler in gameSettingsHandlers)
                handler.Load(currentGameSettings.customSettings);

            UnityEngine.Debug.Log($"{nameof(GameSettingsManager)}: Game Settings Successfully Loaded");
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual GameSettings GetData()
        {
            GameSettings settings;

            try
            {
                settings = SaveSystem.ReadFromJSON<GameSettings>(gameSettingsFileName);

                if (settings == null)
                {
                    UnityEngine.Debug.Log($"ANALYSIS: {nameof(GameSettingsManager)}: " + 
                        $"Game didn't have a Game Settings file, creating one.");

                    settings = new GameSettings();

                    SaveSystem.SaveToJSON(settings, gameSettingsFileName);
                }

                if (settings.customSettings.Count != gameSettingsHandlers.Count)
                {
                    UnityEngine.Debug.Log($"ANALYSIS: {nameof(GameSettingsManager)}: " +
                        $"Game had inconsistent Game Settings; recreating file.");

                    settings.SetCustomSettings(CreateGameSettings().customSettings);

                    SaveSystem.SaveToJSON(settings, gameSettingsFileName);
                }
            }
            catch (Exception e)
            {
                UnityEngine.Debug.LogWarning($"ALERT: {nameof(GameSettingsManager)}: " +
                    $"An unknown error occurred while loading the game settings file: " 
                    + e.ToString());

                settings = defaultGameSettings != null ? defaultGameSettings.ToGameSettings() : CreateGameSettings();
            }

            return settings;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual GameSettings CreateGameSettings()
        {
            GameSettings gameSettings = new();

            foreach (var handler in gameSettingsHandlers)
                gameSettings.customSettings.Add(handler.Save());

            return gameSettings;
        }


        /// <summary>
        ///
        /// </summary>
        [BehaviourButton]
        public void ForceApplyData() => Load(currentGameSettings);
    }
}