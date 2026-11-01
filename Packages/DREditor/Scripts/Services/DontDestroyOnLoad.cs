//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

namespace DREditor.Services
{
    /// <summary>
    ///
    /// </summary>
	[ClassHeader("")]
    public class DontDestroyOnLoad : MonoBehaviour
    {
        protected virtual void Awake() => DontDestroyOnLoad(this);
    }
}