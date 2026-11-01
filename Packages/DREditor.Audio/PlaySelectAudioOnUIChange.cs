//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Services;
using UnityEngine;

namespace DREditor.Audio
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
    [DefaultExecutionOrder(1)]
	[ClassHeader("")]
    public class PlaySelectAudioOnUIChange : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        protected virtual void OnEnable()
        {
            if (ServiceLocator.HasService<UIHandlerBase>())
                ServiceLocator.GetService<UIHandlerBase>().OnChange.AddListener(PlaySelectSound);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDisable()
        {
            if (ServiceLocator.HasService<UIHandlerBase>())
                ServiceLocator.GetService<UIHandlerBase>().OnChange.RemoveListener(PlaySelectSound);
        }

        /// <summary>
        ///
        /// </summary>
        public virtual void PlaySelectSound()
        {
            if (ServiceLocator.HasService<SFXManagerBase>())
                ServiceLocator.GetService<SFXManagerBase>().PlaySelect();
        }
    }
}