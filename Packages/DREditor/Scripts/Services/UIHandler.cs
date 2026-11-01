//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

namespace DREditor.Services
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(UIHandlerBase) + ", " + nameof(ServiceHolder<UIHandlerBase>) +
        "\n\n")]
    public class UIHandler : UIHandlerBase
    {
        /// <summary>
        /// 
        /// </summary>
        protected virtual void Update() => EvaluateSelectedObject();

        /// <summary>
        ///
        /// </summary>
        protected virtual void EvaluateSelectedObject()
        {
            if (DetectedSelectionChange())
                OnDetectedSelectionChange();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool DetectedSelectionChange()
        {
            bool hasEventSystem = EventSystem.current != null;

            if (EventSystem.current == null || !EventSystem.current.enabled)
                return false;

            bool selectedGameObjectIsValid = EventSystem.current.currentSelectedGameObject != null;

            bool currentObjectIsNotSelectedObject = current != EventSystem.current.currentSelectedGameObject;

            bool detectedSelectionChange = hasEventSystem && selectedGameObjectIsValid && currentObjectIsNotSelectedObject;

            return detectedSelectionChange;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDetectedSelectionChange()
        {
            OnChange?.Invoke();

            current = EventSystem.current.currentSelectedGameObject;
        }

        /// <summary>
        ///
        /// </summary>
        public override void CallToTitle() => CallToTitleScreenInterfaces();

        /// <summary>
        ///
        /// </summary>
        protected void CallToTitleScreenInterfaces()
        {
            var listeners = FindObjectsByType<MonoBehaviour>().OfType<IToTitleScreen>();

            foreach (var listener in listeners)
                listener.ToTitleScreen();
        }
    }
}