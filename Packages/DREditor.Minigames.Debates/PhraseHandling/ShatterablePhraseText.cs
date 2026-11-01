//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if DOTWEEN
using CharTween;
#endif
using TMPro;
using UnityEngine;
using UnityEngine.Events;

namespace DREditor.Minigames.Debates.PhraseHandling
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(IPhraseShatterable) +
        "\n\n")]
    public class ShatterablePhraseText : MonoBehaviour, IPhraseShatterable
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool isPhraseText = true;


        /// <summary>
        ///
        /// </summary>
        public UnityEvent<IPhraseShatterable> onShattered;

        public UnityEvent<IPhraseShatterable> OnShattered { get => onShattered; set => onShattered = value; }


        /// <summary>
        ///
        /// </summary>
        protected bool shattering = false;

        /// <summary>
        ///
        /// </summary>
        protected IShatterText shatterText;

        /// <summary>
        ///
        /// </summary>
        protected IPhraseShatterListener[] listeners;


        protected virtual void Awake()
        {
            shatterText = GetComponentInChildren<IShatterText>();

            listeners = GetComponentsInChildren<IPhraseShatterListener>();
        }


        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual bool IsShattering() => shattering;


        /// <summary>
        ///
        /// </summary>
        /// <param name="hitPoint"></param>
        public virtual void Shatter(Vector3 hitPoint)
        {
            foreach (var component in listeners)
                component.OnShattered();

            if (shattering)
                return;

            hitPoint = GetComponentInChildren<TMP_Text>().transform.InverseTransformPoint(hitPoint);

            shattering = true;

            ShatterText(hitPoint);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="hitPoint"></param>
        protected virtual void ShatterText(Vector3 hitPoint)
        {
            shatterText.SetMesh();
#if DOTWEEN
            Destroy(InitializeComponent<CharTweener>());
#endif
            OnShattered?.Invoke(this);

            shatterText.Shatter(hitPoint);

            // Intended to be interaction box disabled when on white noise.
            if (!isPhraseText)
                InitializeComponent<BoxCollider>().enabled = false;
        }

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
        public virtual void Reset()
        {
            shatterText?.ResetMesh();

            shattering = false;
        }
    }
}