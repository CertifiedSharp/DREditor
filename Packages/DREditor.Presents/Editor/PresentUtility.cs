//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using DREditor.Editor.Utility;
using DREditor.Services;
using UnityEditor;
using UnityEngine;

namespace DREditor.Presents.Editor
{
    /// <summary>
    ///
    /// </summary>
    public static class PresentUtility
    {
        /// <summary>
        ///
        /// </summary>
        [MenuItem("DREditor/FTE/Give All Presents")]
        public static void DebugGiveAllPresents()
        {
            if (!Application.isPlaying)
            {
                UnityEngine.Debug.LogWarning("ANALYSIS: You must be in play mode to use this command.");

                return;
            }

            if (!ServiceLocator.HasService(out IPresentManager presentManager))
            {
                UnityEngine.Debug.LogWarning($"ALERT: There was no service with the type {nameof(IPresentManager)}, aborting command.");

                return;
            }

            PresentDatabase presentDatabase = BuilderUtility.FindAssetByType<PresentDatabase>();

            foreach (Present present in presentDatabase.presents)
                presentManager?.AddPresent(present.ID);
        }
    }
}
#endif