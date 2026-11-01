//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.UIHandling;
using System;
using UnityEngine.Events;

namespace DREditor.UI
{
    /// <summary>
    ///
    /// </summary>
    public class MenuState
    {
        /// <summary>
        ///
        /// </summary>
        public UnityEvent<IUIVisibility> MenuEvaluationFinished = new();


        /// <summary>
        ///
        /// </summary>
        public bool CanSelect { get; private set; } = true;

        /// <summary>
        ///
        /// </summary>
        public bool Changing { get; private set; } = false;


        /// <summary>
        ///
        /// </summary>
        public event Action<bool> OnCanSelectChanged;

        /// <summary>
        ///
        /// </summary>
        public event Action<bool> OnMenuStateChanged;


        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        public virtual void SetSelect(bool value)
        {
            if (CanSelect == value) 
                return;

            CanSelect = value;

            OnCanSelectChanged?.Invoke(value);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        public virtual void SetChanging(bool value)
        {
            if (Changing == value) 
                return;

            Changing = value;

            OnMenuStateChanged?.Invoke(value);
        }
    }
}