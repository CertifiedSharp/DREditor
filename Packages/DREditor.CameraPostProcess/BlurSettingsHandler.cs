//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.CustomSettings;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace DREditor.CamerasPostProcess
{
    /// <summary>
    ///
    /// </summary>
	[ClassHeader("")]
    public class BlurSettingsHandler : MonoBehaviour, ICustomGameSettingsHandler
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string gameSettingID = "BlurCameraQueueManager";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BlurCameraRenderManager blurCamManager;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BlurRenderQueueSettings renderQueueSettings;


        /// <inheritdoc/>
        public virtual CustomGameSettings Save() => new(gameSettingID, CustomGameSettings.JObjectSerialize(renderQueueSettings));

        /// <inheritdoc/>
        public virtual void Load(List<CustomGameSettings> customSettings)
        {
            CustomGameSettings settings = customSettings.Where(x => x.ID == gameSettingID).ToList()[0];

            BlurRenderQueueSettings loadedBlurSettings = 
                CustomGameSettings.JObjectDeserialize<BlurRenderQueueSettings>(settings.data);

            LoadBlurSettings(loadedBlurSettings);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="settings"></param>
        protected virtual void LoadBlurSettings(BlurRenderQueueSettings settings)
        {
            renderQueueSettings.PopulateData(settings);

            blurCamManager.SetLowSpecCamRender(settings.useRenderQueue);
        }
    }
}