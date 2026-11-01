//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using UnityEngine;

namespace DREditor.EventObjectServices
{
    /// <summary>
    ///
    /// </summary>
    [RequireComponent(typeof(CharacterController))]
	[ClassHeader("")]
    public class ResetCharacterControllerOnEvent : SceneEventListener
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected CharacterController characterController = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Vector3 resetPosition = new(0, 1.5f, 0);


        /// <inheritdoc/>
        public override void OnEventRaised()
        {
            base.OnEventRaised();

            ResetCharacterControllerPosition();
        }

        /// <summary>
        ///
        /// </summary>
        protected void ResetCharacterControllerPosition()
        {
            characterController.enabled = false;

            characterController.gameObject.transform.position = resetPosition;

            characterController.enabled = true;
        }
    }
}