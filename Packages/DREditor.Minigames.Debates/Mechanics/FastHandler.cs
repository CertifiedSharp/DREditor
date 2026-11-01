//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace DREditor.Minigames.Debates.Mechanics
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(SpeedHandlerBase) + ", " + nameof(IPanelsResolvedListener) + ", " +
        nameof(IPanelsStoppedListener) + ", " + nameof(ISceneEventListener) +
        "\n\n")]
    public class FastHandler : SpeedHandlerBase
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected InputActionReference accelerate = null;


        protected override event Action<CallbackContext> StartInput
        {
            add => accelerate.action.started += value;

            remove => accelerate.action.started -= value;
        }

        protected override event Action<CallbackContext> StopInput
        {
            add => accelerate.action.canceled += value;

            remove => accelerate.action.canceled -= value;
        }


        /// <inheritdoc/>
        protected override void SetGameSpeed(float speed)
        {
            base.SetGameSpeed(speed);
#if DOTWEEN
            DG.Tweening.DOTween.timeScale = speed;
#endif
        }
    }
}