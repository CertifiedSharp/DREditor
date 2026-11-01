//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.Saving
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(IDRESaveable) + ", " + nameof(IDRELoadable) +
        "\n\n")]
    public class CameraPositionSaver : MonoBehaviour, IDRESaveable, IDRELoadable
    {
        /// <summary>
        ///
        /// </summary>
        [Divider]
        [Tooltip("")]
        [SerializeField] protected UnityEngine.Camera trackedCamera = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string saveKeyPosition = "MainPosition";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string saveKeyRotation = "MainRotation";


        /// <inheritdoc/>
        public virtual void Save(Dictionary<string, object> saveData)
        {
            saveData.Set(saveKeyPosition, trackedCamera.transform.position);

            saveData.Set(saveKeyRotation, trackedCamera.transform.eulerAngles);
        }

        /// <inheritdoc/>
        public virtual void Load(Dictionary<string, object> saveDatas)
        {
            if (!saveDatas.ContainsKey(saveKeyPosition))
                return;

            try
            {
                trackedCamera.transform.position = saveDatas.Get<Vector3>(saveKeyPosition);

                trackedCamera.transform.eulerAngles = saveDatas.Get<Vector3>(saveKeyRotation);
            }
            catch
            {
                UnityEngine.Debug.Log($"{ToString()}: Skipping loading position for tracked camera: " + trackedCamera.gameObject.name);
            }
        }
    }
}