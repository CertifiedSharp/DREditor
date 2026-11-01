//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.EventObjects;
using DREditor.Services;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.InputSystem.InputAction;

namespace DREditor.DialogueMechanics.AutomaticLine
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(IToTitleScreen) +
        "\n\n")]
    public class AutoMechanicToggler : MonoBehaviour, IToTitleScreen
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected InputActionReference auto = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected AutoMechanicBase AutoMechanic = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent inDialogue = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent isLeaving = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent isUIToggled = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent canToggleAutoMode = null;


        protected virtual void Awake() => EnableToggleAbility();

        /// <summary>
        ///
        /// </summary>
        protected virtual void EnableToggleAbility() => canToggleAutoMode.SetValue(true);

        protected virtual void OnEnable() => auto.action.performed += AttemptToggleAutoMode;

        protected virtual void OnDisable() => auto.action.performed -= AttemptToggleAutoMode;

        /// <summary>
        ///
        /// </summary>
        /// <param name="context"></param>
        protected virtual void AttemptToggleAutoMode(CallbackContext context)
        {
            if (GetComponentInParent<IDialogueManager>() is var dialogueManager &&
                dialogueManager != ServiceLocator.GetService<IDialogueManager>())
                return;

            if (!inDialogue.Value || isLeaving.Value || !isUIToggled.Value || !canToggleAutoMode.Value)
                return;
            
            if (auto.action.triggered)
                AutoMechanic.ToggleAutoMode();
        }


        /// <inheritdoc/>
        public virtual void ToTitleScreen() => EnableToggleAbility();
    }
}