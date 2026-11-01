//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections;
using UnityEngine;

namespace DREditor.Minigames.Debates.Mechanics
{
    /// <summary>
    ///
    /// </summary>
    public class ChildIndexOffsetter : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Space]
        [Tooltip("")]
        [SerializeField] protected Vector3 childSpacing = new(41.5f, -66.7f, 0);


        protected virtual void Awake() => InitializePosition();

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializePosition()
        {
            if (transform.GetSiblingIndex() == 0)
                return;

            StartCoroutine(Buffer());
        }

        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        protected virtual IEnumerator Buffer()
        {
            yield return null;

            var eldistPosition = transform.parent.GetChild(0).localPosition;

            transform.localPosition = eldistPosition;

            var newPosition = childSpacing * transform.GetSiblingIndex();

            transform.localPosition += newPosition;
        }
    }
}