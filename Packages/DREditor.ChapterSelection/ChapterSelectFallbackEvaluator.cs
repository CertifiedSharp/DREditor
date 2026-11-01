//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

namespace DREditor.ChapterSelection
{
    /// <summary>
    ///
    /// </summary>
    public abstract class ChapterSelectFallbackEvaluator : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public abstract int GetMaxScene(ChapterSectionBase parentSection);

        /// <summary>
        ///
        /// </summary>
        /// <param name="sceneData"></param>
        /// <param name="parentSection"></param>
        /// <returns></returns>
        public abstract bool SceneIsValid(SceneDataProviderBase sceneData, ChapterSectionBase parentSection);
    }
}