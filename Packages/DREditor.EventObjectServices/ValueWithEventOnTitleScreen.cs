//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using DREditor.Services;
using UnityEngine;
using UnityEngine.Events;

namespace DREditor.EventObjectServices
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <typeparam name="TY"></typeparam>
    public class ValueWithEventOnTitleScreen<T, TY> : MonoBehaviour, IToTitleScreen where TY : UnityEvent<T>, new()
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]

        [Tooltip("")]
        [SerializeField] protected ValueWithEvent<T, TY> valueWithEvent = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected T valueOnTitleScreen;


        public virtual void ToTitleScreen()
        {
            if (valueWithEvent != null)
                valueWithEvent.SetValue(valueOnTitleScreen);
        }
    }
}