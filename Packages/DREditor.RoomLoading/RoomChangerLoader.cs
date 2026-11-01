//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Cameras;
using DREditor.EventObjects;
using DREditor.Gates;
using DREditor.Saving;
using DREditor.Services;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.RoomLoading
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(IDRELoadableManager) +
        "\n\n")]
    public class RoomChangerLoader : MonoBehaviour, IDRESaveable, IDRELoadableManager
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected GateDatabase gateDatabase = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected TransformWithEvent playerSpawnPoint = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected TransformWithEvent mouseAbs = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected string positionSaveKey = "Position";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string rotationSaveKey = "Rotation";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string mouseabsSaveKey = "MouseAbs";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string sceneNameSaveKey = "SceneName";


        /// <inheritdoc/>
        public virtual void LoadManager(Dictionary<string, object> saveDatas, bool inChapterSelect)
        {
            var position = saveDatas.Get<Vector3>(positionSaveKey);

            var rotation = Quaternion.Euler(saveDatas.Get<Vector3>(rotationSaveKey));

            PosRot spawnPointData = new(position, rotation);

            playerSpawnPoint.SetValue(spawnPointData);

            mouseAbs.SetValue(new(saveDatas.Get<Vector2>(mouseabsSaveKey), Quaternion.Euler(Vector3.zero)));

            ServiceLocator.GetService<RoomChangerBase>().SetDestination(gateDatabase.GetArea(saveDatas.Get<string>(sceneNameSaveKey)));
        }

        /// <inheritdoc/>
        public virtual void Save(Dictionary<string, object> saveDatas)
        {
            var smoothMouse = FindAnyObjectByType<SmoothMouseLook>();

            if (smoothMouse != null)
                saveDatas.Set(mouseabsSaveKey, smoothMouse.GetAbsolute());
        }
    }
}