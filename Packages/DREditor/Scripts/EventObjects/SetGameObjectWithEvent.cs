//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

namespace DREditor.EventObjects
{
    /// <summary>
    ///
    /// </summary>
    public class SetGameObjectWithEvent : MonoBehaviour
    {
        [Header("")]
        [Space]
        [Tooltip("")]
        [SerializeField] protected GameObjectWithEvent gameObjectWithEvent = null;

        [Tooltip("")]
        [SerializeField] protected bool setOnlyOnEmpty = true;


        /// <summary>
        ///
        /// </summary>
        protected GameObject original = null;


        protected virtual void Start()
        {
            if (!setOnlyOnEmpty || gameObjectWithEvent.Value == null)
                SetThisGameObject();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void SetThisGameObject()
        {
            if (gameObjectWithEvent.Value != null)
                original = gameObjectWithEvent.Value;

            gameObjectWithEvent.SetValue(gameObject);
        }


        protected virtual void OnDestroy()
        {
            if (!setOnlyOnEmpty && original != null)
                gameObjectWithEvent.SetValue(original);
        }
    }
}