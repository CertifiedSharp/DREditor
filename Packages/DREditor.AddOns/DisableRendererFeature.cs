//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
// Leothedev helped.
using UnityEngine;
using UnityEngine.Rendering.Universal;

namespace DREditor.AddOns
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
    public class DisableRendererFeature : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]

        [Tooltip("")]
        [SerializeField] protected UniversalRendererData rendererData = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string featureName = "SSAO";


        /// <summary>
        ///
        /// </summary>
        protected virtual void Start() => ChangeFeature(featureName, false);

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDisable() => ChangeFeature(featureName, true);

        /// <summary>
        ///
        /// </summary>
        /// <param name="featureName"></param>
        /// <param name="enabledState"></param>
        protected virtual void ChangeFeature(string featureName, bool enabledState) => GetFeature(featureName).SetActive(enabledState);

        /// <summary>
        ///
        /// </summary>
        /// <param name="featureName"></param>
        /// <returns></returns>
        protected virtual ScriptableRendererFeature GetFeature(string featureName)
        {
            foreach (var feature in rendererData.rendererFeatures)
                if (feature != null && feature.name == featureName)
                    return feature;

            return null;
        }
    }
}