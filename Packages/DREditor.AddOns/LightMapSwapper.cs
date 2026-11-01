//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;

namespace DREditor.AddOns
{
    /// <summary>
    ///
    /// </summary>
    public class LightMapSwapper : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Texture2D lightMap = null;


        protected virtual void Start() => SetLightMap();

        /// <summary>
        ///
        /// </summary>
        protected virtual void SetLightMap()
        {
            LightmapData data = new()
            {
                lightmapColor = lightMap
            };

            LightmapSettings.lightmaps = new LightmapData[] { data };
        }
    }
}