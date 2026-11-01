//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.DialogueMechanics.Investigation;
using UnityEngine;

namespace DREditor.UI.Menus
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(IUnlockableUIOption) +
        "\n\n")]
    public class TruthBulletUnlockableOption : MonoBehaviour, IUnlockableUIOption
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected TruthBulletDataHandler TruthBulletDataHandler = null;


        protected virtual void Awake()
        {
            if (TruthBulletDataHandler == null && 
                transform.root.GetComponentInChildren<TruthBulletDataHandler>() is TruthBulletDataHandler handler)
                TruthBulletDataHandler = handler;
        }

        /// <inheritdoc/>
        public virtual bool IsUnlocked() => TruthBulletDataHandler.foundBullets.Count != 0;
    }
}