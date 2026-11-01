//Completely Reworked Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
//Originally done by Willy Bee
using DREditor.EventObjects;
using UnityEngine;
using UnityEngine.Serialization;

namespace DREditor.UI.RegulationRules
{
    /// <summary>
    ///
    /// </summary>
    [System.Serializable]
    [CreateAssetMenu(menuName = "DREditor/Regulations/Regulation", fileName = "Regulation")]
    public class Regulation : ScriptableObject, IRegulation
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Space]
        [Tooltip("")]
        [SerializeField] protected ActiveBySceneEvent stateData;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [FormerlySerializedAs("regulationImg")]
        [SerializeField] protected Sprite image;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [TextArea]
        [SerializeField] protected string text;


        public Sprite Image => image;

        public string Text => text;


        /// <inheritdoc/>
        public virtual void Initialize()
        {
            if (stateData != null)
                stateData.Initialize();
        }

        /// <inheritdoc/>
        public virtual void Resolve()
        {
            if (stateData != null)
                stateData.Resolve();
        }

        /// <inheritdoc/>
        public virtual bool IsActiveRegulation() => stateData == null || stateData.IsActive;

        /// <inheritdoc/>
        public virtual void SetRegulationState(bool active)
        {
            if (stateData != null)
                stateData.Load(active);
        }
        

#if UNITY_EDITOR

        /// <summary>
        ///
        /// </summary>
        /// <param name="text"></param>
        public virtual void SetText(string text) => this.text = text;

        /// <summary>
        ///
        /// </summary>
        /// <param name="text"></param>
        public virtual void SetImage(Sprite image) => this.image = image;

#endif
    }
}