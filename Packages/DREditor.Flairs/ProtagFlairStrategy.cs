//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Kill/Cure: Beautiful Vice
using DREditor.Dialogues;
using DREditor.EventObjects;
using DREditor.Services;
using System.Collections;
using UnityEngine;

namespace DREditor.Flairs
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(FlairStrategy) + ", " + nameof(FlairStrategyBase) + ", " + nameof(FlairSpawnerBase) +
        "\n\n")]
    public class ProtagFlairStrategy : FlairStrategy
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected StringWithEvent protagFirstName = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected BoolWithEvent protagUIVisible = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float protagWindowShowDuration = 0.5f;


        /// <inheritdoc/>
        protected override bool ShouldBeUsed() => base.ShouldBeUsed() && ProtagIsSpeaking();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        /// <remarks></remarks>
        protected virtual bool ProtagIsSpeaking() =>
            ServiceLocator.GetService<IDialogueManager>().GetReader().GetCurrentLine().Speaker.FirstName == protagFirstName.Value;


        /// <inheritdoc/>
        protected override void OnSpawnInvoked()
        {
            if (protagUIVisible.Value)
                base.OnSpawnInvoked();
            else
                StartCoroutine(BufferSpawning());
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator BufferSpawning()
        {
            yield return new WaitUntil(() => protagUIVisible.Value);

            yield return new WaitForSeconds(protagWindowShowDuration);

            base.OnSpawnInvoked();
        }
    }
}