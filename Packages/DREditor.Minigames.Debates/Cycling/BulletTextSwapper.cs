//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.EventObjects;
using DREditor.Investigations;
using DREditor.UIHandling;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.Minigames.Debates.Cycling
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks>
    ///
    /// </remarks>
    [ClassHeader(nameof(IBulletSendable) + ", " + nameof(ISceneEventListener) + 
        "\n\n")]
    public class BulletTextSwapper : BulletTextHandler, IBulletSendable, ISceneEventListener
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected SceneEvent setBulletText = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected EvaluateCondition setBulletTextAllowed = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected StringWithEvent bulletText = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected IntWithEvent currentBulletIndex = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float bulletTextFontSize = 36;


        /// <summary>
        ///
        /// </summary>
        protected List<TruthBullet> truthBullets = new();


        /// <inheritdoc/>
        protected override void Awake()
        {
            base.Awake();

            bulletTextFontSize = bulletTMP.fontSize;
        }

        protected virtual void OnEnable() => bulletText.AddListener(OnBulletTextUpdated);

        protected virtual void OnDisable() => bulletText.RemoveListener(OnBulletTextUpdated);


        /// <inheritdoc/>
        public virtual void OnSentBullets(List<TruthBullet> bullets)
        {
            truthBullets = bullets;

            OnEventRaised();

            setBulletText.RegisterListener(this);
        }

        /// <inheritdoc/>
        public virtual void OnEventRaised()
        {
            if (setBulletTextAllowed == null || setBulletTextAllowed.Evaluate())
            {
                SetBulletText();

                StartCoroutine(SetFontRoutine());
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected override void SetBulletText()
        {
            var bullet = truthBullets[currentBulletIndex.Value];

            if (bulletTextFontSize != 0)
                bulletTMP.fontSize = bulletTextFontSize;

            if (bullet != null)
                bulletText.SetValue(bullet.DisplayName);
        }


        protected virtual void OnBulletTextUpdated(string updatedText) => bulletTMP.text = updatedText;

        protected virtual void OnDestroy()
        {
            setBulletText.UnregisterListener(this);

            bulletText.SetValue(string.Empty);
        }
    }
}