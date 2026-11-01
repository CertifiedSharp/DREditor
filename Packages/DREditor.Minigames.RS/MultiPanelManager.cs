//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Cameras;
using DREditor.Characters;
using DREditor.Processables;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.UI;

namespace DREditor.Minigames.RS
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(InitializableBase) +
        "\n\n")]
    public class MultiPanelManager : InitializableBase
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [FormerlySerializedAs("database")]
        [SerializeField] protected CharacterDatabase characterDatabase = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int numberOfCharacters = 16;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<DRTrialCamera> drCameras = new();

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<RawImage> panelWindows = new();

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<RenderTexture> cameraTextures = new();


        /// <summary>
        ///
        /// </summary>
        protected Canvas canvas = null;

        /// <summary>
        ///
        /// </summary>
        protected List<UnityEngine.Camera> cameras = new();

        /// <summary>
        ///
        /// </summary>
        protected List<ICameraEnabler> cameraEnablers = new();


        /// <inheritdoc/>
        protected override void Awake()
        {
            base.Awake();

            canvas = GetComponentInChildren<Canvas>();
        }

        protected virtual void OnDisable()
        {
            foreach (var camera in cameras)
                if (camera != null)
                    camera.targetTexture = null;

            if (canvas != null)
                canvas.enabled = false;
        }


        /// <inheritdoc/>
        protected override void Init()
        {
            SetMainDRCam();

            InitializeCameras();

            SetCameraTextures();

            EnableCanvas();

            EnableCameras();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void SetMainDRCam()
        {
            var cameras = FindObjectsByType<DRTrialCamera>();

            foreach (var camera in cameras)
            {
                if (!drCameras.Contains(camera))
                {
                    drCameras.Insert(0, camera);

                    return;
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeCameras()
        {
            foreach (var drCamera in drCameras)
            {
                if (drCamera != null && drCamera.GetComponentInChildren<UnityEngine.Camera>() is UnityEngine.Camera camera)
                {
                    cameras.Add(camera);

                    cameraEnablers.Add(camera.GetComponent<ICameraEnabler>());

                    SetCharacterHeightOffsets(drCamera);
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="camera"></param>
        protected virtual void SetCharacterHeightOffsets(DRTrialCamera camera)
        {
            if (characterDatabase != null)
                foreach (Character character in characterDatabase.Characters)
                    if (character.TrialPosition < numberOfCharacters)
                        camera.CharHeightOffset[character.TrialPosition] = character.TrialHeight;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void SetCameraTextures()
        {
            for (int i = 0; i < cameras.Count && i < panelWindows.Count && i < cameraTextures.Count; i++)
            {
                cameras[i].targetTexture = cameraTextures[i];

                panelWindows[i].texture = cameraTextures[i];
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void EnableCanvas()
        {
            if (canvas != null)
                canvas.enabled = true;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void EnableCameras()
        {
            foreach (var camera in cameraEnablers)
                camera?.InvokeCameraEnabled(true);
        }
    }
}