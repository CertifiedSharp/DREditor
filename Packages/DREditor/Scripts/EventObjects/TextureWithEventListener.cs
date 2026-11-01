//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using UnityEngine;
using UnityEngine.Events;

namespace DREditor.EventObjects
{
    /// <summary>
    ///
    /// </summary>
    public class TextureWithEventListener : MonoBehaviour
    {
        [Header("")]
        [Space]
        [Tooltip("")]
        [SerializeField] protected TextureWithEvent textureWithEvent = null;

        [Header("")]
        [SerializeField] protected UnityEvent<Texture2D> onTextureChanged = null;

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnEnable() => textureWithEvent.AddListener(OnEventChanged);

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDisable() => textureWithEvent.RemoveListener(OnEventChanged);

        /// <summary>
        ///
        /// </summary>
        /// <param name="changedValue"></param>
        protected virtual void OnEventChanged(Texture2D changedValue) => onTextureChanged?.Invoke(changedValue);
    }
}