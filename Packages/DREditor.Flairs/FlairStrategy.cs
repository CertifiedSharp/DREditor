//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Kill/Cure: Beautiful Vice
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.Flairs
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(FlairStrategyBase) + ", " + nameof(FlairSpawnerBase) +
        "\n\n")]
    public class FlairStrategy : FlairStrategyBase
    {
        /// <summary>
        ///
        /// </summary>
        [SerializeReference, SelectType]
        [Tooltip("")]
        [SerializeField] protected IFlairSpawnStep spawnStep;

        /// <summary>
        ///
        /// </summary>
        [SerializeReference, SelectType]
        [Tooltip("")]
        [SerializeField] protected List<IFlairStep> steps = new();


        /// <inheritdoc/>
        protected override void OnSpawnInvoked()
        {
            GameObject spawnedFlair = spawnStep?.SpawnFlair(eventInstance);

            eventInstance.ApplySettings(spawnedFlair);

            InvokeSteps(spawnedFlair);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="spawnedFlair"></param>
        protected virtual void InvokeSteps(GameObject spawnedFlair)
        {
            foreach (var step in steps)
                step.ApplyStep(eventInstance, spawnedFlair);
        }
    }
}