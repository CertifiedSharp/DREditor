//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Processables;
using UnityEngine;

namespace DREditor.EventObjects
{
    /// <summary>
    ///
    /// </summary>
    public abstract class SelectableObject : MonoBehaviour, INameDisplayable, ISelectableObject
    {
        [Tooltip("")]
        [SerializeField] protected string displayName = "";

        [Tooltip("")]
        public Transform head;

        [Tooltip("")]
        public Transform body;

        [Tooltip("")]
        public bool selectable = true;


        /// <summary>
        ///
        /// </summary>
        protected Collider selectableCollider = null;


        /// <summary>
        ///
        /// </summary>
        /// <param name="selectable"></param>
        /// <remarks>
        ///
        /// </remarks>
        public virtual void SetSelectability(bool selectable) => this.selectable = selectable;

        

        /// <summary>
        ///
        /// </summary>
        /// <param name="newHead"></param>
        /// <param name="newBody"></param>
        public virtual void SetHeadAndBody(Transform newHead, Transform newBody)
        {
            head = newHead;

            body = newBody;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="enable"></param>
        public virtual void EnableCollision(bool enable)
        {
            if (selectableCollider == null && TryGetComponent(out selectableCollider))
                selectableCollider = GetComponentInChildren<Collider>();

            if (selectableCollider != null)
                selectableCollider.enabled = enable;
        }


        public abstract void SelectObject();

        /// <inheritdoc/>
        public virtual string DisplayName
        {
            get => displayName;

            set => displayName = value;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public virtual string GetSaveDisplayName() => displayName;
    }
}