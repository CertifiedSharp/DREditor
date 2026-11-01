//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if FMOD_UNITY
using System.Linq;
using UnityEngine;

namespace DREditor.Audio.FMOD
{
    /// <summary>
    ///
    /// </summary>
	[ClassHeader("")]
    public class AudioVisualizerTwoDFMOD : AudioVisualizerThreeDFMOD
    {
        /// <summary>
        ///
        /// </summary>
        protected RectTransform[] rectBlocks;


        protected virtual void Awake() => rectBlocks = blocks.Select(b => b.GetComponent<RectTransform>()).ToArray();

        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        protected override void ResetVisualizerBlocks()
        {
            for (int i = 0; i < blocks.Length; ++i)
                rectBlocks[i].sizeDelta = new Vector2(rectBlocks[i].sizeDelta.x, 0);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="spectrum"></param>
        /// <param name="i"></param>
        protected override void CalculateBlockSizeDelta(float[][] spectrum, int i)
        {
            currentIndex = i;

            Vector2 currentBlockSize = CalculateBlockSizeBySpectrum(spectrum, rectBlocks[currentIndex].sizeDelta);

            SetBlockSizeDelta(currentBlockSize);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="currentBlockSize"></param>
        protected override void SetBlockSizeDelta(Vector3 currentBlockSize)
        {
            if (bgmSetting.Value <= 0)
                rectBlocks[currentIndex].sizeDelta = Vector2.zero;
            else
                rectBlocks[currentIndex].sizeDelta = currentBlockSize;
        }
    }
}
#endif