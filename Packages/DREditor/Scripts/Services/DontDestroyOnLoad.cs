//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

namespace DREditor.Services
{
    /// <summary>
    /// Script to set it's GameObject to always carry over between scenes.
    /// </summary>
    public class DontDestroyOnLoad : MonoBehaviour
    {
        protected virtual void Awake() => DontDestroyOnLoad(this);
    }
}