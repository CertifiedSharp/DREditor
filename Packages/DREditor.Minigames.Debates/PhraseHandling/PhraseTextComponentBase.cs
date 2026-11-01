//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Processables;
using TMPro;
using UnityEngine;

namespace DREditor.Minigames.Debates.PhraseHandling
{
    /// <summary>
    ///
    /// </summary>
    public abstract class PhraseTextComponentBase : MonoBehaviour, IPhraseHandlerComponent, IPrioritizable
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int priority = 0;


        /// <summary>
        ///
        /// </summary>
        protected TMP_Text tmpText;


        public int Priority { get => priority; set => priority = value; }


        protected virtual void Awake() => InitializeComponents();

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeComponents() => tmpText = InitializeComponent<TMP_Text>();

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


        /// <inheritdoc/>
        public abstract void StartPhrase<T>(T phrase) where T : PhraseBase;
    }
}