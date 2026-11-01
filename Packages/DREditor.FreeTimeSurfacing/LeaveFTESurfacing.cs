//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Audio;
using DREditor.EventObjects;
using DREditor.Fading;
using DREditor.RoomLoading;
using DREditor.Services;
using System.Collections;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using static UnityEngine.InputSystem.InputAction;

namespace DREditor.FreeTimeSurfacing
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(ControlledComponent) +
        "\n\n")]
    public class LeaveFTESurfacing : ControlledComponent
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected InputActionReference cancel = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected ScriptableObject leaveRoomOptions = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string returnScene = "";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float fadeOutDuration = 0.5f;


        /// <summary>
        ///
        /// </summary>
        protected bool leaving = false;


        /// <inheritdoc/>
        protected override void Awake()
        {
            base.Awake();

            StoreLastPlayedMusic();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void StoreLastPlayedMusic()
        {
            if (!ServiceLocator.HasService<string>())
                ServiceLocator.Register(ServiceLocator.GetService<MusicManagerBase>().GetLastPlayedMusic());
        }

        /// <inheritdoc/>
        public override void ActivateControls() => cancel.action.started += Cancel;

        /// <inheritdoc/>
        public override void DeactivateControls() => cancel.action.started -= Cancel;

        /// <summary>
        ///
        /// </summary>
        /// <param name="ctx"></param>
        protected virtual void Cancel(CallbackContext ctx)
        {
            if (AlreadyLeaving())
                return;

            DisableInput();

            StartCoroutine(LeaveRoutine());
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool AlreadyLeaving()
        {
            bool result = leaving || string.IsNullOrEmpty(returnScene);

            if (!result)
                leaving = true;

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator LeaveRoutine()
        {
            yield return FadeOutScreen();

            LeaveFTESurface();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator FadeOutScreen()
        {
            if (ServiceLocator.HasService(out GlobalFade fader))
                yield return fader.FadeToRoutine(fadeOutDuration, false, true);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void LeaveFTESurface()
        {
            ServiceLocator.GetService<RoomChangerBase>().SetOptions(leaveRoomOptions);

            SceneManager.sceneLoaded += ReturnMusic;

            UnityEngine.Debug.LogWarning("Changing Scene");

            SceneManager.LoadScene(returnScene);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="scene"></param>
        /// <param name="mode"></param>
        protected virtual void ReturnMusic(Scene scene, LoadSceneMode mode)
        {
            SceneManager.sceneLoaded -= ReturnMusic;

            RestoreStoredMusic();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void RestoreStoredMusic()
        {
            if (ServiceLocator.HasService(out string storedMusic))
                ServiceLocator.GetService<MusicManagerBase>().PlayMusic(storedMusic);
            else
                UnityEngine.Debug.LogWarning("ALERT: There was no music stored in the service registry!");
        }
    }
}