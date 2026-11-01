//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using UnityEngine;
using UnityEngine.UI;

namespace DREditor.EventObjects
{
    /// <summary>
    ///
    /// </summary>
    public class RawImageEventListener : TextureWithEventListener
    {
        [Header("")]
        [Space]
        [Tooltip("")]
        [SerializeField] protected bool setOnAwake = true;

        [Tooltip("")]
        [SerializeField] protected RawImage rawImage = null;


        protected virtual void Awake()
        {
            if (setOnAwake)
                OnEventChanged(textureWithEvent.Value);
        }


        protected override void OnEventChanged(Texture2D changedValue)
        {
            base.OnEventChanged(changedValue);

            if (changedValue != null)
                rawImage.texture = changedValue;
        }
    }
}