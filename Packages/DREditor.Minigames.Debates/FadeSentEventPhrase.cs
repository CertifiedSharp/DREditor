//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if DOTWEEN
using DG.Tweening;
#endif
using TMPro;
using UnityEngine;

namespace DREditor.Minigames.Debates
{
    /// <summary>
    ///
    /// </summary>
	[ClassHeader("")]
    public class FadeSentEventPhrase : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float duration = 1;


        /// <summary>
        ///
        /// </summary>
        public virtual void FadePhrase(PhraseTextBase aimedText)
        {
            if (aimedText == null || (aimedText.TryGetComponent<IPhraseShatterable>(out var shat) && shat.IsShattering())) 
                return;

            if (aimedText.GetComponentInChildren<BoxCollider>() is var collider)
                collider.enabled = false;

            if (aimedText.GetComponentInChildren<TMP_Text>() is var tmpText)
#if DOTWEEN
                tmpText.DOColor(new Color(tmpText.color.r, tmpText.color.g, tmpText.color.b, 0), duration);
#else
                tmpText.color = new Color(tmpText.color.r, tmpText.color.g, tmpText.color.b, 0);
#endif
        }
    }
}