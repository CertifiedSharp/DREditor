//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;
using UnityEngine.Events;

namespace DREditor.Trials
{
    /// <summary>
    ///
    /// </summary>
    [System.Serializable]
    [CreateAssetMenu(menuName = "DREditor/Trials/Minigame Type", fileName = "MinigameType")]
    public class MinigameType : ScriptableObject
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        public string TypeName;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public int TypeNumber;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public string ControlsKey;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public GameObject Prefab;


        [Header("")]
        public UnityEvent<ScriptableObject> manager;


        /// <summary>
        ///
        /// </summary>
        /// <param name="TypeNumber"></param>
        public virtual void SetTypeNumber(int TypeNumber) => this.TypeNumber = TypeNumber;
    }
}