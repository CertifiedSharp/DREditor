//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Services;
using UnityEngine;

namespace DREditor.Audio
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(SoundPlayerDRE) +
        "\n\n")]
    public class SoundPlayerDREUnity : SoundPlayerDRE
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected AudioClip clipSFX;

        /// <inheritdoc/>
        public override void PlayMusic() => ServiceLocator.GetService<MusicManagerBase>().PlayMusic(clipSFX);

        /// <inheritdoc/>
        public override void PlaySFX() => ServiceLocator.GetService<SFXManagerBase>().PlaySFX(clipSFX);

        /// <inheritdoc/>
        public override void PlayVoiceLine() => ServiceLocator.GetService<VOManagerBase>().PlayVoiceLine(clipSFX);
    }
}