//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if FMOD_UNITY
using DREditor.EventObjects;
using DREditor.Services;
using FMOD;
using FMODUnity;
using System;
using System.Collections;
using System.Runtime.InteropServices;
using UnityEngine;

namespace DREditor.Audio.FMOD
{
    /// <summary>
    ///
    /// </summary>
	[ClassHeader("")]
    public class AudioVisualizerThreeDFMOD : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected bool canDisplay = false;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected ValueWithEvent<float, FloatEvent> bgmSetting = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected Transform[] blocks;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected float heightScale = 5;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] int expScale = 3;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] int sectionAdder = 2;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] int sectionDivider = 2;


        /// <summary>
        ///
        /// </summary>
        protected DSP fft;

        /// <summary>
        ///
        /// </summary>
        protected float height;

        /// <summary>
        ///
        /// </summary>
        protected int currentIndex = 0;


        protected static readonly float DecibelMinVolume = -80.0f;

        protected static readonly float DecibelMaxVolume = 0.0f;

        protected static readonly float Amplitude = 20.0f;

        protected static readonly int WindowSize = 16;


        /// <summary>
        ///
        /// </summary>
        protected virtual void OnEnable() => ServiceLocator.GetService<MusicManagerBase>().OnMusicChange += PrepareEmitterData;

        /// <summary>
        ///
        /// </summary>
        /// <param name="emitter"></param>
        protected virtual void PrepareEmitterData(StudioEventEmitter emitter) => StartCoroutine(PrepareEmitterDataRoutine(emitter));

        /// <summary>
        ///
        /// </summary>
        /// <param name="emitter"></param>
        protected virtual IEnumerator PrepareEmitterDataRoutine(StudioEventEmitter emitter)
        {
            if (ResetBlocksOnEmitterNull(emitter))
                yield break;

            yield return new WaitUntil(() => RuntimeManager.IsInitialized);

            yield return new WaitUntil(() => emitter.IsPlaying());

            canDisplay = false;

            InitializeEmitterData(emitter);

            canDisplay = true;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="emitter"></param>
        protected virtual void InitializeEmitterData(StudioEventEmitter emitter)
        {
            RuntimeManager.CoreSystem.createDSPByType(DSP_TYPE.FFT, out fft);

            fft.setParameterInt((int)DSP_FFT.WINDOWTYPE, (int)DSP_FFT_WINDOW.HANNING);

            fft.setParameterInt((int)DSP_FFT.WINDOWSIZE, WindowSize);

            UnityEngine.Debug.Log(emitter.EventInstance.getChannelGroup(out ChannelGroup channelGroup).ToString());

            UnityEngine.Debug.Log(fft.getParameterInt((int)DSP_FFT.WINDOWSIZE, out int value));

            UnityEngine.Debug.Log(value);

            RESULT rESULT = fft.getInfo(out _, out _, out _, out _, out _);

            UnityEngine.Debug.Log(rESULT.ToString());

            UnityEngine.Debug.Log(channelGroup.addDSP(CHANNELCONTROL_DSP_INDEX.HEAD, fft).ToString());
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="emitter"></param>
        /// <returns></returns>
        protected bool ResetBlocksOnEmitterNull(StudioEventEmitter emitter)
        {
            if (emitter == null)
                ResetVisualizerBlocks();

            return emitter == null;
        }



        /// <summary>
        ///
        /// </summary>
        protected virtual void LateUpdate() => EvaluateVisualizeAudio();

        /// <summary>
        ///
        /// </summary>
        protected virtual void EvaluateVisualizeAudio()
        {
            if (canDisplay)
                VisualizeAudio();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void VisualizeAudio()
        {
            fft.getParameterData((int)DSP_FFT.SPECTRUMDATA, out IntPtr unmanagedData, out _);

            try
            {
                DSP_PARAMETER_FFT fftData = (DSP_PARAMETER_FFT)Marshal.PtrToStructure(unmanagedData, typeof(DSP_PARAMETER_FFT));

                if (fftData.numchannels > 0)
                    for (int i = 0; i < blocks.Length; ++i)
                        CalculateBlockSizeDelta(fftData.spectrum, i);
            }
            catch { }
        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="spectrum"></param>
        /// <param name="currentBlockSize"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        protected virtual Vector3 CalculateBlockSizeBySpectrum(float[][] spectrum, Vector3 currentBlockSize)
        {
            int spectrumSection = CalculateSpectrumSection();

            float decibelLevel = LinearToDecibel(spectrum[0][spectrumSection]);

            height = (-1 * DecibelMinVolume) + decibelLevel;

            height *= heightScale;

            currentBlockSize.y = Mathf.Lerp(currentBlockSize.y, height, 0.1f);

            return currentBlockSize;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual int CalculateSpectrumSection() => expScale * (currentIndex + sectionAdder) / sectionDivider;

        /// <summary>
        ///
        /// </summary>
        /// <param name="linear"></param>
        /// <returns></returns>
        protected float LinearToDecibel(float linear) => Mathf.Clamp(Mathf.Log10(linear) * Amplitude, DecibelMinVolume, DecibelMaxVolume);

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDisable()
        {
            if (ServiceLocator.HasService<MusicManagerBase>())
                ServiceLocator.GetService<MusicManagerBase>().OnMusicChange -= PrepareEmitterData;
        }



        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        protected virtual void ResetVisualizerBlocks()
        {
            for (int i = 0; i < blocks.Length; ++i)
                blocks[i].transform.localScale = new Vector3(blocks[i].transform.localScale.x, 0);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="spectrum"></param>
        /// <param name="i"></param>
        protected virtual void CalculateBlockSizeDelta(float[][] spectrum, int i)
        {
            currentIndex = i;

            Vector3 currentBlockSize = CalculateBlockSizeBySpectrum(spectrum, blocks[currentIndex].transform.localScale);

            SetBlockSizeDelta(currentBlockSize);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="currentBlockSize"></param>
        protected virtual void SetBlockSizeDelta(Vector3 currentBlockSize)
        {
            if (bgmSetting.Value.Equals(0))
                blocks[currentIndex].transform.localScale = Vector3.zero;
            else
                blocks[currentIndex].transform.localScale = currentBlockSize;
        }
    }
}
#endif