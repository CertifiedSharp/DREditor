//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Input;
using DREditor.Services;
using UnityEngine;

namespace DREditor.UI.Buttons
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(AnyInputDetector) +
        "\n\n")]
    public class AnyKey : AnyInputDetector
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected MenuGroup groupOn = null;
        
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected MenuGroup groupTo = null;


        /// <summary>
        ///
        /// </summary>
        protected IInputManager inputManager;

        /// <summary>
        ///
        /// </summary>
        protected bool alreadyConfirmedKey = false;


        /// <summary>
        ///
        /// </summary>
        protected virtual void Start() => ServiceLocator.HasService(out inputManager);


        /// <inheritdoc/>
        public override void Subscribe()
        {
            base.Subscribe();

            alreadyConfirmedKey = false;
        }


        /// <inheritdoc/>
        protected override bool ValidKeyNoticed() => !alreadyConfirmedKey && base.ValidKeyNoticed();


        /// <inheritdoc/>
        protected override void OnValidInputFound()
        {
            inputManager?.SetSchemeByDevice(device);

            groupOn.ChangeGroup(groupTo);

            alreadyConfirmedKey = true;
        }


        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDestroy() => Unsubscribe();
    }
}