//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections;
using TMPro;
using UnityEngine;

namespace DREditor.Minigames.Debates.Cycling
{
    /// <summary>
    ///
    /// </summary>
    public abstract class BulletTextHandler : MonoBehaviour
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected TMP_Text bulletTMP;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float iterateFontSize = 1;


        protected virtual void Awake()
        {
            if (bulletTMP == null)
                bulletTMP = GetComponent<TMP_Text>();
        }

        /// <summary>
        ///
        /// </summary>
        protected abstract void SetBulletText();

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator SetFontRoutine()
        {
            if (bulletTMP.textInfo.lineCount > 1)
                UnityEngine.Debug.Log($"ANALYSIS: Bullet {bulletTMP.text}: Text Line Count is " + bulletTMP.textInfo.lineCount);

            bulletTMP.ForceMeshUpdate();

            while (bulletTMP.textInfo.lineCount > 1)
            {
                bulletTMP.fontSize -= iterateFontSize;

                yield return null;
            }
        }
    }
}