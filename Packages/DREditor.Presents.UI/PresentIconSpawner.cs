//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

namespace DREditor.Presents.UI
{
    /// <summary>
    ///
    /// </summary>
	[ClassHeader("")]
    public class PresentIconSpawner : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected GameObject presentIconPrefab;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int spawnAmount = 32;

#if UNITY_EDITOR
        /// <summary>
        ///
        /// </summary>
        [ContextMenu("Spawn Icons")]
        public void SpawnIcons()
        {
            if (presentIconPrefab == null)
            {
                UnityEngine.Debug.LogWarning("ALERT: The prefab field must be filled out!");

                return;
            }
            
            for (int i = 0; i < spawnAmount; i++)
                UnityEditor.PrefabUtility.InstantiatePrefab(presentIconPrefab, transform);
        }
#endif
    }
}