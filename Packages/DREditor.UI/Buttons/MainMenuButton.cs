//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DREditor.UI.Buttons
{
    /// <summary>
    ///
    /// </summary>
    public abstract class MainMenuButton : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected bool useButton = true;

        
        /// <summary>
        ///
        /// </summary>
        protected Button button;


        protected virtual void Awake()
        {
            TryGetComponent(out button);

            if (button == null && useButton)
                UnityEngine.Debug.LogError($"ALERT: {gameObject.name} does not have a button but you are using the use button setting! " +
                    "Either add a button with this component or turn of the use button option!");
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnEnable() => SubscribeToButton(true, OnButtonClicked);

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDisable() => SubscribeToButton(false, OnButtonClicked);

        /// <summary>
        ///
        /// </summary>
        /// <param name="subscribing"></param>
        /// <param name="unityAction"></param>
        protected virtual void SubscribeToButton(bool subscribing, UnityAction unityAction)
        {
            if (button != null && useButton)
                if (subscribing)
                    button.onClick.AddListener(unityAction);
                else
                    button.onClick.RemoveListener(unityAction);
        }


        /// <summary>
        ///
        /// </summary>
        public abstract void OnButtonClicked();
    }
}