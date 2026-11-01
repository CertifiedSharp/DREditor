//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Services;
using FMODUnity;
using UnityEngine;

namespace DREditor.Audio.FMOD
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(SoundPlayerDRE) +
        "\n\n")]
    public class SoundPlayerDREFMOD : SoundPlayerDRE
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField][EventRef] string clipSFX;


        /// <inheritdoc/>
        public override void PlayMusic()
        {
            if (ServiceLocator.HasService<MusicManagerBase>())
                ServiceLocator.GetService<MusicManagerBase>().PlayMusic(clipSFX, true);
        }

        /// <inheritdoc/>
        public override void PlaySFX()
        {
            if (ServiceLocator.HasService<SFXManagerBase>())
                ServiceLocator.GetService<SFXManagerBase>().PlaySFX(clipSFX);
        }

        /// <inheritdoc/>
        public override void PlayVoiceLine()
        {
            if (ServiceLocator.HasService<VOManagerBase>())
                ServiceLocator.GetService<VOManagerBase>().PlayVoiceLine(clipSFX);
        }
    }
}