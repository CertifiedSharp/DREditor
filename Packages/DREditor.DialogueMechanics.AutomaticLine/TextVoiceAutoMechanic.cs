//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Audio;
using DREditor.Services;
using System.Collections;
using UnityEngine;

namespace DREditor.DialogueMechanics.AutomaticLine
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(TextBoxAutoMechanic) + ", " + nameof(AutoMechanic) + ", " + nameof(AutoMechanicBase) +
        "\n\n")]
    public class TextVoiceAutoMechanic : TextBoxAutoMechanic
    {
        /// <inheritdoc/>
        protected override IEnumerator AutoMechanicProcess()
        {
            yield return new WaitForSeconds(CalculateAutoWaitTime());

            if (ServiceLocator.GetService<VOManagerBase>().VoicePlaying())
                while (ServiceLocator.GetService<VOManagerBase>().VoicePlaying())
                    yield return null;

            EndAutoProcess();

            yield break;
        }
    }
}