//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Services;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace DREditor.RoomLoading
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
    public class TPFDRoomAnimation : MonoBehaviour, IPostTransitionRoutine
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected InputActionReference fastForward = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float speedUpRate = 2;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Animation roomAnimation;


        /// <summary>
        ///
        /// </summary>
        protected virtual void AddSpeedInput()
        {
            fastForward.action.started += SpeedUp;

            fastForward.action.canceled += SlowDown;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void RemoveSpeedInput()
        {
            fastForward.action.started -= SpeedUp;

            fastForward.action.canceled -= SlowDown;

            Time.timeScale = 1;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        protected virtual void SpeedUp(CallbackContext context) => Time.timeScale = speedUpRate;

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        protected virtual void SlowDown(CallbackContext context) => Time.timeScale = 1;


        /// <inheritdoc/>
        public virtual IEnumerator PostTransitionRoutine()
        {
            AddSpeedInput();

            roomAnimation.Play();

            yield return new WaitForSeconds(roomAnimation.clip.length);

            RemoveSpeedInput();

            yield break;
        }
    }
}