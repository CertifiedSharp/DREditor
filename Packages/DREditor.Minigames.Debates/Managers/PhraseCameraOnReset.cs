//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.Minigames.Debates.Managers
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(IPhraseResetter) +
        "\n\n")]
    public class PhraseCameraOnReset : MonoBehaviour, IPhraseResetter
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Transform phraseCameraTransform = null;


        /// <summary>
        ///
        /// </summary>
        protected IEnumerable<PhraseTextBase> phrasesToReset;


        protected virtual void Awake() => phrasesToReset = transform.root.GetComponentsInChildren<PhraseTextBase>();


        /// <inheritdoc/>
        public virtual void Reset()
        {
            if (phrasesToReset != null && phraseCameraTransform != null)
                foreach (var phrase in phrasesToReset)
                    phrase.SetCameraTransform(phraseCameraTransform);
        }
    }
}