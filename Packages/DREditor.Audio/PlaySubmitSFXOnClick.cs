//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Services;
using UnityEngine;
using UnityEngine.UI;

namespace DREditor.Audio
{
    /// <summary>
    ///
    /// </summary>
	[ClassHeader("")]
    [RequireComponent(typeof(Button))]
    public class PlaySubmitSFXOnClick : MonoBehaviour
    {
        protected virtual void OnEnable() 
            => GetComponent<Button>().onClick.AddListener(PlaySubmitSFX);

        protected virtual void OnDisable() 
            => GetComponent<Button>().onClick.RemoveListener(PlaySubmitSFX);

        protected virtual void PlaySubmitSFX() => ServiceLocator.GetService<SFXManagerBase>().PlaySubmit();
    }
}