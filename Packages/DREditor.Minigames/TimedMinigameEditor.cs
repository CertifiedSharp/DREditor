//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using UnityEngine;

namespace DREditor.Minigames
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class TimedMinigameEditor<T> : UnityEditor.Editor where T : ScriptableObject
    {
        /// <summary>
        ///
        /// </summary>
        protected TimedDifficultyEditor TimerDifficultyEditor;

        /// <summary>
        ///
        /// </summary>
        protected T builder;


        protected virtual void OnEnable()
        {
            TimerDifficultyEditor = new();

            builder = target as T;
        }
    }
}
#endif