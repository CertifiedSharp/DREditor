//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues.Events;
using DREditor.Investigations;
using DREditor.Services;
using DREditor.UIHandling;
using UnityEngine;
using UnityEngine.UI;

namespace DREditor.DialogueMechanics.Investigation
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(UIVisibilityHandler) + ", " + nameof(IToTitleScreen) +
        "\n\n")]
    public class DisplayTruthBulletGet : UIVisibilityHandler, IToTitleScreen
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected Image bulletGetImage = null;


        /// <summary>
        ///
        /// </summary>
        protected bool uiVisible = false;


        /// <summary>
        ///
        /// </summary>
        protected virtual void OnEnable()
        {
            // These Event Names are not fields because the Event Data class uses these specific strings
            DialogueEventSystem.StartListening("TruthBulletGet", TruthBulletGet);

            DialogueEventSystem.StartListening("TruthBulletGetEnd", TruthBulletGetEnd);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDisable()
        {
            // These Event Names are not fields because the Event Data class uses these specific strings
            DialogueEventSystem.StopListening("TruthBulletGet", TruthBulletGet);

            DialogueEventSystem.StopListening("TruthBulletGetEnd", TruthBulletGetEnd);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        public virtual void TruthBulletGet(object o = null)
        {
            if (!uiVisible)
                CheckTruthBulletGet((TruthBulletGet)o);
            else
                TruthBulletGetEnd();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="truthBulletData"></param>
        protected virtual void CheckTruthBulletGet(TruthBulletGet truthBulletData)
        {
            if (truthBulletData.truthBullet != null)
                DisplayTruthBullet(truthBulletData.truthBullet.Image);
            else
                UnityEngine.Debug.LogError($"{ToString()}: Don't make a TruthBulletGet Event " +
                    $"without referencing a Truth Bullet you moron.");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="bulletSprite"></param>
        protected virtual void DisplayTruthBullet(Sprite bulletSprite)
        {
            uiVisible = true;

            bulletGetImage.sprite = bulletSprite;

            visibleUI.Show();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        protected virtual void TruthBulletGetEnd(object o = null)
        {
            visibleUI.Hide();

            uiVisible = false;
        }

        /// <inheritdoc/>
        public virtual void ToTitleScreen() => TruthBulletGetEnd();
    }
}