//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Services;
using UnityEngine;

namespace DREditor.Audio
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(IToTitleScreen) +
        "\n\n")]
    public class StopEnvAudioToTitle : MonoBehaviour, IToTitleScreen
    {
        /// <inheritdoc/>
        public virtual void ToTitleScreen() => ServiceLocator.GetService<EnvironmentSFXManangerBase>().StopEnv();
    }
}