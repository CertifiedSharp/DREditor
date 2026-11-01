//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using System.Collections;
using UnityEngine;

namespace DREditor.Minigames.Debates
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(SceneEventListener) +
        "\n\n")]
    public class BulletModeUpdater : SceneEventListener
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float bufferDuration = 0.5f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEvent displayBulletMode = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float postDisplayDuration = 0.5f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent canFire = null;


        /// <inheritdoc/>
        protected override void OnResponseInvoked()
        {
            base.OnResponseInvoked();

            StartCoroutine(BulletMode());
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator BulletMode()
        {
            yield return new WaitForSecondsRealtime(bufferDuration);

            displayBulletMode.Raise();

            yield return new WaitForSecondsRealtime(postDisplayDuration);

            canFire.SetValue(true);
        }
    }
}