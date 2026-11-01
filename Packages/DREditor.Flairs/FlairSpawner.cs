//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Kill/Cure: Beautiful Vice
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.Flairs
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(FlairSpawnerBase) +
        "\n\n")]
    public class FlairSpawner : FlairSpawnerBase
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<FlairStrategyBase> strategies = new();


        /// <inheritdoc/>
        public override void Spawn(IFlairEvent eventInstance)
        {
            foreach (var strategy in strategies)
                if (strategy.TryToSpawn(eventInstance))
                    break;
        }
    }
}