//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Minigames.Debates;
using TMPro;
using UnityEngine;

namespace DREditor.Minigames.RS
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(ISwordHitListener) +
        "\n\n")]
    public class DamagePhraseOnSwordHit : MonoBehaviour, ISwordHitListener
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int damageOnHit = 1;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected ScriptableObject shatterProcessOverride;


        /// <summary>
        ///
        /// </summary>
        protected PhraseTextBase phraseText;

        /// <summary>
        ///
        /// </summary>
        protected IPhraseShatterable shatter;

        /// <summary>
        ///
        /// </summary>
        protected Vector3 hitPoint;


        /// <inheritdoc/>
        public virtual void OnSwordHit(GameObject hitObject, Vector3 hitPoint)
        {
            this.hitPoint = hitPoint;

            if (HitObjectIsPhrase(hitObject) && PhraseIsSwipable())
                OnValidSwordHit();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="hitObject"></param>
        /// <returns></returns>
        protected virtual bool HitObjectIsPhrase(GameObject hitObject)
            => hitObject.transform.parent.TryGetComponent(out phraseText) && phraseText != null;

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool PhraseIsSwipable()
        {
            shatter = phraseText.GetComponent<IPhraseShatterable>();

            return shatter != null && !shatter.IsShattering();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnValidSwordHit()
        {
#if UNITY_EDITOR
            LogMesh();
#endif
            DamagePhrase();

            OnHitPhrase();

            CheckShatterText();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void LogMesh()
        {
            Mesh mesh = phraseText.GetComponentInChildren<TMP_Text>().mesh;

            UnityEngine.Debug.Log("REPORT:" +
                " Vert Length: " + mesh.vertices.Length +
                " Tri: " + mesh.triangles.Length +
                " UV: " + mesh.uv.Length +
                " UV2: " + mesh.uv2.Length +
                " Colors: " + mesh.colors.Length);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DamagePhrase()
        {
            if (phraseText.textPhrase is IPhraseHealth phraseHealth)
                phraseHealth.TakeDamage(damageOnHit);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnHitPhrase()
        {
            IPhraseHitListener[] hitComponents = phraseText.GetComponentsInChildren<IPhraseHitListener>();

            foreach (var component in hitComponents)
                component.OnHit();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void CheckShatterText()
        {
            if (PhraseShouldShatter())
                ShatterPhrase();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool PhraseShouldShatter()
            => phraseText.textPhrase is IPhraseHealth phraseHealth && phraseHealth.GetHealth() == 0;

        /// <summary>
        ///
        /// </summary>
        protected virtual void ShatterPhrase()
        {
            phraseText.StopAllCoroutines(); // Stops the phrases currently playing animations.

            phraseText.GetComponentInChildren<IShatterText>()?.OverrideProcess(shatterProcessOverride);

            shatter?.Shatter(hitPoint);
        }
    }
}