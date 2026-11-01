//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.EventObjects
{
    /// <summary>
    ///
    /// </summary>
    public class SelectableObjectGroup : MonoBehaviour
    {
        [Header("")]
        [Space]
        [Tooltip("")]
        [SerializeField] protected List<SelectableObject> items = new();

        [Tooltip("")]
        [SerializeField] protected List<Collider> inverseColliders = new();


        /// <summary>
        ///
        /// </summary>
        /// <param name="to"></param>
        public virtual void EnableItems(bool to)
        {
            foreach (SelectableObject selectableObject in items)
            {
                selectableObject.EnableCollision(to);

                selectableObject.SetSelectability(to);
            }

            foreach (Collider collider in inverseColliders)
                collider.enabled = !to;
        }
    }
}