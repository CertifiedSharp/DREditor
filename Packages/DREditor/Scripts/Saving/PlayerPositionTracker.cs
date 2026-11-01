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
    public class PlayerPositionTracker : MonoBehaviour, IDRESaveable, IDRELoadable
    {
        /// <summary>
        ///
        /// </summary>
        [Divider]
        [Tooltip("")]
        [SerializeField] string saveKeyPosition = "Position";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] string saveKeyRotation = "Rotation";


        /// <inheritdoc/>
        public virtual void Save(Dictionary<string, object> saveData)
        {
            saveData.Set(saveKeyPosition, transform.position);

            saveData.Set(saveKeyRotation, transform.eulerAngles);
        }

        /// <inheritdoc/>
        public virtual void Load(Dictionary<string, object> saveDatas)
        {
            if (!saveDatas.ContainsKey(saveKeyPosition))
                return;

            try
            {
                transform.position = saveDatas.Get<Vector3>(saveKeyPosition);

                transform.eulerAngles = saveDatas.Get<Vector3>(saveKeyRotation);
            }
            catch
            {
                UnityEngine.Debug.LogWarning($"{ToString()}: ALERT: Skipping loading position for tracked object: " + gameObject.name);
            }
        }
    }
}