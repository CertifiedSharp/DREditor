// Original Author: Henry (Meowpixel)
// Updated/Reworked by: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

namespace DREditor.Minigames.CA.Pages
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(PageHandlerBase) + ", " + nameof(IClosingArgumentResetable) +
        "\n\n")]
    public class MovePageHandler : ControlledComponent, IClosingArgumentResetable
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected InputActionReference shuffleLeft = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected InputActionReference shuffleRight = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent playerHasDied = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent trialTimeUp = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected GameObject pages;


        [Header("")]
        [SerializeField] protected UnityEvent OnPageTurn = null;


        /// <summary>
        ///
        /// </summary>
        protected int currentIndex = 0;

        /// <summary>
        ///
        /// </summary>
        protected int pageDiffCounter;

        /// <summary>
        ///
        /// </summary>
        protected Coroutine activeCoroutine;

        /// <summary>
        ///
        /// </summary>
        protected List<MovePageComponent> moveableComponents = new();


        /// <inheritdoc/>
        protected override void Awake()
        {
            base.Awake();

            moveableComponents = FindObjectsByType<MovePageComponent>().ToList();
        }

        /// <inheritdoc/>
        protected override void Start()
        {
            base.Start();

            moveableComponents.ForEach(component => component.Initialize());
        }

        /// <inheritdoc/>
        public override void ActivateControls()
        {
            shuffleLeft.action.performed += MovePagesLeft;

            shuffleRight.action.performed += MovePagesRight;
        }

        /// <inheritdoc/>
        public override void DeactivateControls()
        {
            shuffleLeft.action.performed -= MovePagesLeft;

            shuffleRight.action.performed -= MovePagesRight;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        protected void MovePagesLeft(InputAction.CallbackContext context)
        {
            if (currentIndex + pageDiffCounter == 0)
                return;

            MovePages(-1);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        protected void MovePagesRight(InputAction.CallbackContext context)
        {
            if (currentIndex + pageDiffCounter + 1 == pages.transform.childCount)
                return;

            MovePages(1);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="direction"></param>
        protected virtual void MovePages(int direction)
        {
            OnPageTurn?.Invoke();

            if (activeCoroutine != null)
                StopCoroutine(activeCoroutine);

            activeCoroutine = StartCoroutine(MovePagesRoutine(direction));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="pageDifference"></param>
        protected IEnumerator MovePagesRoutine(int pageDifference) //moves the pages and pagebar when the current page is changed
        {
            DisableInput();

            // Makes sure the current page (the one that will be shown to the player) is always correct,
            // no matter the order keys are pressed
            pageDiffCounter += pageDifference;

            moveableComponents.ForEach(component => component.UpdateEndPosition(pageDifference, currentIndex + pageDiffCounter));

            while (MovementsUnfinished())
            {
                moveableComponents.ForEach(component => component.MovePageUpdate());

                yield return null;
            }

            moveableComponents.ForEach(component => component.Initialize());

            currentIndex += pageDiffCounter;

            pageDiffCounter = 0;

            UpdatePageListeners(currentIndex);

            if (!playerHasDied.Value && !trialTimeUp.Value)
                EnableInput();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="currentPage"></param>
        protected virtual void UpdatePageListeners(int currentPage)
        {
            var pageListeners = transform.root.GetComponentsInChildren<IPageListener>();

            foreach (var listener in pageListeners)
                listener.OnPageUpdated(currentPage, pages.transform.childCount);
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool MovementsUnfinished()
        {
            foreach (var page in moveableComponents)
                if (page.MovementUnfinished())
                    return true;

            return false;
        }


        /// <inheritdoc/>
        public virtual void CAReset()
        {
            pageDiffCounter = 0;

            currentIndex = 0;

            if (activeCoroutine != null)
                StopCoroutine(activeCoroutine);
        }
    }
}