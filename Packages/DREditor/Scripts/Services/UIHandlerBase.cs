//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;
using UnityEngine.Events;

namespace DREditor.Services
{
    /// <summary>
    ///
    /// </summary>
    public abstract class UIHandlerBase : ServiceHolder<UIHandlerBase>
    {
        /// <summary>
        ///
        /// </summary>
        [Header(nameof(UIHandlerBase) + ", " + nameof(ServiceHolder<UIHandlerBase>) +
            "\n\n")]
        [Header(nameof(UIHandlerBase) + " Settings")]
        [Space]
        [Tooltip("")]
        public GameObject current;


        [Header("")]
        public UnityEvent OnChange;


        /// <summary>
        ///
        /// </summary>
        protected bool movingToTitleScreen = false;


        /// <summary>
        ///
        /// </summary>
        public bool MovingToTitleScreen
        {
            get => movingToTitleScreen;

            set => movingToTitleScreen = value;
        }


        /// <summary>
        ///
        /// </summary>
        public abstract void CallToTitle();


        /// <summary>
        ///
        /// </summary>
        /// <param name="current"></param>
        public virtual void SetCurrent(GameObject current) => this.current = current;
    }
}