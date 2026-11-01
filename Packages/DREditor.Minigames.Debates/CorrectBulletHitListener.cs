//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Linq;
using UnityEngine;
using UnityEngine.Events;

namespace DREditor.Minigames.Debates
{
    /// <summary>
    ///
    /// </summary>
	[ClassHeader("")]
    public class CorrectBulletHitListener : MonoBehaviour
    {
        [Header("")]
        [SerializeField] protected UnityEvent<PhraseBase> onBulletHit = null;


        /// <summary>
        ///
        /// </summary>
        protected IBulletEvaluatorManager bulletEvaluatorManager = null;


        protected virtual void Awake()
            => bulletEvaluatorManager = transform.root.GetComponentsInChildren<Component>().OfType<IBulletEvaluatorManager>().FirstOrDefault();

        protected virtual void OnEnable() => bulletEvaluatorManager.OnCorrectPhraseHit.AddListener(OnCorrectBulletHit);

        protected virtual void OnDisable() => bulletEvaluatorManager.OnCorrectPhraseHit.RemoveListener(OnCorrectBulletHit);

        /// <summary>
        ///
        /// </summary>
        /// <param name="textPhrase"></param>
        protected virtual void OnCorrectBulletHit(PhraseBase textPhrase) => onBulletHit?.Invoke(textPhrase);
    }
}