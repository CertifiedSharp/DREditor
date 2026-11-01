//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using DREditor.EventObjectServices;
#if FMOD_UNITY
using FMOD.Studio;
using FMODUnity;
#endif
using UnityEngine;
#if !FMOD_UNITY
using UnityEngine.Audio;
#endif
namespace DREditor.Audio
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(ValueWithEventListener<ValueWithEvent<float, FloatEvent>, float, FloatEvent>) +
        "\n\n")]
    public class OptionVolume : ValueWithEventListener<ValueWithEvent<float, FloatEvent>, float, FloatEvent>
    {
#if FMOD_UNITY
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string masterBusString = "Bus:/Master/";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string busName = "Music"; // SFX // Dialogue


        /// <summary>
        ///
        /// </summary>
        protected Bus fmodBus;


        /// <summary>
        ///
        /// </summary>
        protected virtual void Start() => fmodBus = RuntimeManager.GetBus(masterBusString + busName);


        /// <inheritdoc/>
        protected override void OnValueChanged(float updatedValue)
        {
            base.OnValueChanged(updatedValue);

            fmodBus.setVolume(updatedValue);
        }
#else
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected AudioMixer mixer = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string mixerName = "Music";


        protected override void OnValueChanged(float updatedValue)
        {
            base.OnValueChanged(updatedValue);

            mixer.SetFloat(mixerName, updatedValue);
        }
#endif
    }
}