//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DREditor.States.Editor
{
    /// <summary>
    ///
    /// </summary>
    public static class StateAssetUtility
    {
        /// <summary>
        ///
        /// </summary>
        [MenuItem("DREditor/States/Update State Listeners")]
        public static void UpdateAll()
        {
            var all = MonoBehaviour.FindObjectsByType<MonoBehaviour>(FindObjectsInactive.Include).OfType<IStateList>();

            if (all.Count() == 0)
                UnityEngine.Debug.LogWarning("Couldn't Find any behaviours inheriting: " + nameof(IStateList));

            foreach (var item in all)
                item.OnInitializeStateList();
        }
    }
}
#endif