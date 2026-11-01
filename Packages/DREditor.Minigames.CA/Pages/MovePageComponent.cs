// Original Author: Henry (Meowpixel)
// Updated/Reworked by: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections;
using UnityEngine;

namespace DREditor.Minigames.CA.Pages
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(IClosingArgumentResetable) +
        "\n\n")]
    public class MovePageComponent : MonoBehaviour, IClosingArgumentResetable
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected GameObject pages;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float movementMultiplier = 80;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Vector3 movePages = new(-9.887051203f, 0.6740315305f, 0); //new(-2.793402478f, 0.192100486f, 0);


        /// <summary>
        ///
        /// </summary>
        protected Vector3 pageEndPosition = Vector3.zero;


        /// <summary>
        ///
        /// </summary>
        /// <param name="pageEndPosition"></param>
        protected virtual void SetPageEndPosition(Vector3 pageEndPosition) => this.pageEndPosition = pageEndPosition;


        /// <summary>
        ///
        /// </summary>
        public virtual void Initialize() => SetPageEndPosition(pages.transform.position);

        /// <summary>
        ///
        /// </summary>
        /// <param name="pageDifference"></param>
        /// <param name="updatedIndex"></param>
        public virtual void UpdateEndPosition(int pageDifference, int updatedIndex)
        {
            SetPageEndPosition(new Vector3(
                movePages.x * pageDifference + pageEndPosition.x,
                movePages.y * pageDifference + pageEndPosition.y,
                movePages.z));
        }

        /// <summary>
        ///
        /// </summary>
        public virtual void MovePageUpdate()
        {
            pages.transform.position = Vector3.
                    MoveTowards(pages.transform.position, pageEndPosition, movementMultiplier * Time.deltaTime);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual bool MovementUnfinished() => pages.transform.position != pageEndPosition;


        /// <summary>
        ///
        /// </summary>
        [ContextMenu("Set Page Bar")]
        public void SetPageBar()
        {
            movePages = new(-2.793402478f, 0.192100486f, 0);

            movementMultiplier = 20;
        }

        /// <inheritdoc/>
        public virtual void CAReset() => StartCoroutine(BufferResetInitialization());

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator BufferResetInitialization()
        {
            yield return null;

            Initialize();
        }
    }
}