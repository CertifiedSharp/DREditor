// Original Author: Henry (Meowpixel)
// Updated/Reworked by: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using DREditor.UIHandling.Reticles;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

namespace DREditor.Minigames.CA.Panels
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(HoverReticle) + ", " + nameof(Reticle) +
        "\n\n")]
    public class ClosingArgumentReticle : HoverReticle
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [FormerlySerializedAs("caInputAllowed")]
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inputAllowed = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int bufferFrames = 5;

        /// <summary>
        ///
        /// </summary>
        [Range(0, 1)]
        [Tooltip("")]
        [SerializeField] protected float dampenScaleFactor = 0.012f;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected Rigidbody2D rigidBody = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<Collider2D> currentColliders = new();


        /// <summary>
        ///
        /// </summary>
        protected Coroutine enterCoroutine = null;

        /// <summary>
        ///
        /// </summary>
        protected Coroutine exitCoroutine = null;


        protected virtual void OnEnable() => inputAllowed.AddListener(OnInputUpdated);

        protected virtual void OnDisable() => inputAllowed.RemoveListener(OnInputUpdated);

        /// <summary>
        ///
        /// </summary>
        /// <param name="canInput"></param>
        protected virtual void OnInputUpdated(bool canInput) => SetLock(!canInput);


        /// <inheritdoc/>
        protected override void Start()
        {
            base.Start();

            rigidBody = GetComponent<Rigidbody2D>();
        }


        protected virtual void OnTriggerEnter2D(Collider2D collision)
        {
            currentColliders.Add(collision);

            if (enterCoroutine != null)
                StopCoroutine(enterCoroutine);

            enterCoroutine = StartCoroutine(CheckEnter(collision));
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="collision"></param>
        protected virtual IEnumerator CheckEnter(Collider2D collision)
        {
            yield return new WaitForSeconds(Time.deltaTime * bufferFrames);

            if (!locked)
            {
                QuestionPanelBase foundQuestion = collision.gameObject.GetComponent<QuestionPanelBase>();

                object foundPanel = foundQuestion;

                if (foundPanel != null && foundPanel != hoveredObject)
                    HoverObject(foundQuestion.transform, foundQuestion);
            }
        }


        protected virtual void OnTriggerExit2D(Collider2D collision)
        {
            currentColliders.Remove(collision);

            if (exitCoroutine != null)
                StopCoroutine(exitCoroutine);

            exitCoroutine = StartCoroutine(CheckExit());
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator CheckExit()
        {
            yield return new WaitForSeconds(Time.deltaTime * bufferFrames);

            if (currentColliders.Count == 0)
                StopHoveringObject();
            else
                HoverObject(currentColliders[0].transform,
                    currentColliders[0].gameObject.GetComponent<QuestionPanelBase>());
            //For above,
            //if it's colliding with two at the same time and leaving the collision of one.
            //Otherwise the selection would be wrongly set to null.
        }


        /// <inheritdoc/>
        protected override Vector3 GrabMovement() => base.GrabMovement() * dampenScaleFactor;

        /// <inheritdoc/>
        protected override void ApplyCalculatedMovement(Vector3 calculatedMovement)
        {
            if (rigidBody != null)
                rigidBody.MovePosition(rigidBody.position + (Vector2)calculatedMovement);
            else
                base.ApplyCalculatedMovement(calculatedMovement);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="to"></param>
        public override void SetLock(bool to)
        {
            base.SetLock(to);

            if (!locked)
                gameObject.GetComponent<BoxCollider2D>().enabled = !to;
        }
    }
}