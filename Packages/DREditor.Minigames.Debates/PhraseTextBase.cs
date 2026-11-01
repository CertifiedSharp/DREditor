//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Cameras;
using UnityEngine;

namespace DREditor.Minigames.Debates
{
    /// <summary>
    ///
    /// </summary>
    public abstract class PhraseTextBase : MonoBehaviour, IPhraseResetter
    {
        /// <summary>
        ///
        /// </summary>
        [HideInInspector] public PhraseBase textPhrase;

        /// <summary>
        ///
        /// </summary>
        protected Transform cameraTransform;


        protected virtual void Awake() => InitializeComponents();

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeComponents() => SetCameraTransform(FindAnyObjectByType<DRTrialCamera>().transform);

        /// <summary>
        ///
        /// </summary>
        /// <param name="cameraTransform"></param>
        public virtual void SetCameraTransform(Transform cameraTransform) => this.cameraTransform = cameraTransform;

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <returns></returns>
        protected virtual T InitializeComponent<T>() where T : Component
        {
            if (!TryGetComponent<T>(out var result))
                result = GetComponentInChildren<T>();

            return result;
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="phrase"></param>
        /// <returns></returns>
        public virtual float Initialize<T>(T phrase) where T : PhraseBase
        {
            phrase.Initialize();

            textPhrase = phrase;

            return 0;
        }

        /// <summary>
        ///
        /// </summary>
        public abstract void Reset();
    }
}