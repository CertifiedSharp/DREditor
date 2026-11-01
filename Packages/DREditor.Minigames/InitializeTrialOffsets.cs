//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Cameras;
using DREditor.Characters;
using DREditor.Processables;
using UnityEngine;
using UnityEngine.Serialization;

namespace DREditor.Minigames
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(InitializableBase) + ", " + nameof(IInitializable) +
        "\n\n")]
    public class InitializeTrialOffsets : InitializableBase
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [FormerlySerializedAs("database")]
        [SerializeField] protected CharacterDatabase characterDatabase = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int numberOfCharacters = 16;


        /// <inheritdoc/>
        protected override void Init()
        {
            var cameras = FindObjectsByType<DRTrialCamera>();

            foreach (var camera in cameras)
                SetCharacterHeightOffsets(camera);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="camera"></param>
        protected virtual void SetCharacterHeightOffsets(DRTrialCamera camera)
        {
            if (characterDatabase != null)
                foreach (Character character in characterDatabase.Characters)
                    if (character.TrialPosition < numberOfCharacters)
                        camera.CharHeightOffset[character.TrialPosition] = character.TrialHeight;
        }
    }
}