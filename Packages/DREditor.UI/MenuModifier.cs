//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

namespace DREditor.UI
{
    /// <summary>
    ///
    /// </summary>
    [RequireComponent(typeof(MenuGroupBase))]
    public class MenuModifier : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        protected MenuGroupBase menu = null;


        /// <summary>
        ///
        /// </summary>
        protected virtual void Awake()
        {
            if (!TryGetComponent(out menu) && !TryGetComponentInParent(out menu))
                UnityEngine.Debug.LogError($"{gameObject.name}: Menu modifier is missing a menu to modify!");
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="menu"></param>
        /// <returns></returns>
        protected virtual bool TryGetComponentInParent<T>(out T menu) where T : Component
        {
            menu = null;

            Transform currentTransform = transform;

            while (currentTransform != null && !currentTransform.TryGetComponent(out menu))
                currentTransform = currentTransform.parent;

            return menu != null;
        }
    }
}