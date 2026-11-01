// Original Author: Henry (Meowpixel)
// Updated/Reworked by: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using UnityEngine;

namespace DREditor.Minigames.CA.Pages
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(MovePageComponent) +
        "\n\n")]
    public class MoveMainPagesComponent : MovePageComponent
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool debugMissing = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float scaleMultiplier = 20f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Vector3 focusScale = new(0.65f, 0.65f, 1);

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Vector3 unfocusScale = new(0.4f, 0.4f, 1);

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Vector4 visibleShadow = new(0, 0, 0, 1);

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Vector4 invisibleShadow = new(0, 0, 0, 0);

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int shadowParentIndex = 0;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int shadowChildIndex = 1;


        /// <summary>
        ///
        /// </summary>
        protected Transform currentPage = null;

        /// <summary>
        ///
        /// </summary>
        protected SpriteRenderer currentPageShadow;

        /// <summary>
        ///
        /// </summary>
        protected Transform toBePage;

        /// <summary>
        ///
        /// </summary>
        protected SpriteRenderer toBePageShadow;


        /// <inheritdoc/>
        public override void Initialize()
        {
            base.Initialize();

            if (currentPageShadow != null)
                currentPageShadow.color = visibleShadow;

            if (toBePageShadow != null)
                toBePageShadow.color = invisibleShadow;
        }

        /// <inheritdoc/>
        public override void UpdateEndPosition(int pageDifference, int updatedIndex)
        {
            base.UpdateEndPosition(pageDifference, updatedIndex);

            currentPage = pages.transform.GetChild(updatedIndex - pageDifference);

            toBePage = pages.transform.GetChild(updatedIndex);

            currentPageShadow = currentPage.GetChild(shadowParentIndex).GetChild(shadowChildIndex).GetComponent<SpriteRenderer>();

            toBePageShadow = toBePage.GetChild(shadowParentIndex).GetChild(shadowChildIndex).GetComponent<SpriteRenderer>();
        }

        /// <inheritdoc/>
        public override bool MovementUnfinished() => base.MovementUnfinished() || currentPage.localScale != unfocusScale;

        /// <inheritdoc/>
        public override void MovePageUpdate()
        {
            base.MovePageUpdate();

            try
            {
                MoveScale();

                MoveShadow();
            }
            catch (NullReferenceException exception)
            {
                if (debugMissing)
                    UnityEngine.Debug.LogException(exception);
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void MoveScale()
        {
            currentPage.localScale = Vector3.MoveTowards(
                currentPage.localScale,
                unfocusScale,
                scaleMultiplier * Time.deltaTime);

            toBePage.localScale = Vector3.MoveTowards(
                toBePage.localScale,
                focusScale,
                scaleMultiplier * Time.deltaTime);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void MoveShadow()
        {
            currentPageShadow.color = Vector4.MoveTowards(
                currentPageShadow.color,
                visibleShadow,
                scaleMultiplier * Time.deltaTime);

            toBePageShadow.color = Vector4.MoveTowards(
                toBePageShadow.color,
                invisibleShadow,
                scaleMultiplier * Time.deltaTime);
        }
    }
}