//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using System.Linq;
using UnityEngine;

namespace DREditor.Saving
{
    /// <summary>
    ///
    /// </summary>
	[ClassHeader("")]
    public class PlayerDataClearHandler : SceneEventListener
    {
        /// <summary>
        ///
        /// </summary>
        protected override void OnEnable()
        {
            base.OnEnable();

            Response.AddListener(Clear);
        }

        /// <summary>
        ///
        /// </summary>
        protected override void OnDisable()
        {
            base.OnDisable();

            Response.RemoveListener(Clear);
        }

        /// <summary>
        ///
        /// </summary>
        public virtual void Clear()
        {
            IPlayerDataClearer[] clearers = FindObjectsByType<MonoBehaviour>().OfType<IPlayerDataClearer>().ToArray();

            foreach (var clear in clearers)
                clear.ClearData();

            UnityEngine.Debug.Log("Player Datas Cleared.");
        }
    }
}