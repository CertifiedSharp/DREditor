//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using DREditor.Services;

namespace DREditor.Audio
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(SceneEventListener) +
        "\n\n")]
    public class SelectSFXOnSceneEvent : SceneEventListener
    {
        /// <inheritdoc/>
        public override void OnEventRaised()
        {
            base.OnEventRaised();

            ServiceLocator.GetService<SFXManagerBase>().PlaySelect();
        }
    }
}