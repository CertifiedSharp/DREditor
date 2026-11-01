//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using UnityEngine;
using UnityEngine.Rendering;

namespace DREditor.Minigames.Debates.Mechanics
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(ControlledComponent) + ", " + nameof(IPanelsResolvedListener) + ", " +
        nameof(IPanelsStoppedListener) + ", " + nameof(ISceneEventListener) +
        "\n\n")]
    public class DebateVolumeManager : ControlledComponent, IPanelsResolvedListener,
        IPanelsStoppedListener, ISceneEventListener
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEvent onPhraseShot = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Volume volume = null;


        /// <summary>
        ///
        /// </summary>
        protected IShatterGraphic shatter;


        /// <inheritdoc/>
        protected override void Awake()
        {
            base.Awake();

            shatter = transform.root.GetComponentInChildren<IShatterGraphic>();
        }

        /// <inheritdoc/>
        protected override void OnEnable()
        {
            base.OnEnable();

            shatter?.OnPostShatter?.AddListener(DisableVolume);
        }

        /// <inheritdoc/>
        protected override void OnDisable()
        {
            base.OnDisable();

            shatter?.OnPostShatter?.RemoveListener(DisableVolume);

            DisableVolume();
        }

        /// <inheritdoc/>
        public override void ActivateControls()
        {
            if (onPhraseShot != null)
                onPhraseShot.RegisterListener(this);
        }

        /// <inheritdoc/>
        public override void DeactivateControls()
        {
            if (onPhraseShot != null)
                onPhraseShot.UnregisterListener(this);

            EnableVolume();
        }


        /// <summary>
        ///
        /// </summary>
        protected virtual void EnableVolume()
        {
            if (volume != null)
                volume.enabled = true;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisableVolume()
        {
            if (volume != null)
                volume.enabled = false;
        }


        /// <inheritdoc/>
        public virtual void OnPanelsStopped() => DisableVolume();

        /// <inheritdoc/>
        public virtual void OnPanelsResolved() => EnableVolume();

        /// <inheritdoc/>
        public virtual void OnEventRaised() => EnableVolume();
    }
}