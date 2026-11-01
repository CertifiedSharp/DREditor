//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Kill/Cure: Beautiful Vice
using UnityEngine;

namespace DREditor.Flairs
{
    /// <summary>
    ///
    /// </summary>
    [CreateAssetMenu(menuName = "DREditor/Flairs/Flair", fileName = "Flair")]
    public class Flair : ScriptableObject, IFlair
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected GameObject prefab;


        /// <inheritdoc/>
        public virtual GameObject GetPrefab() => prefab;

#if UNITY_EDITOR
        /// <summary>
        ///
        /// </summary>
        /// <param name="prefab"></param>
        public virtual void SetPrefab(GameObject prefab) => this.prefab = prefab;
#endif
    }
}