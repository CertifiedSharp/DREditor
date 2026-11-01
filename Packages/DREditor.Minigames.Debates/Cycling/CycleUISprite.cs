//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using UnityEngine;
using UnityEngine.UI;

namespace DREditor.Minigames.Debates.Cycling
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(ICycleUIStateComponent) +
        "\n\n")]
    public class CycleUISprite : MonoBehaviour, ICycleUIStateComponent
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Image bulletImage = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Sprite activeBullet;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Sprite inactiveBullet;


        /// <inheritdoc/>
        public virtual void DisplayBulletActive(bool isActive)
        {
            if (bulletImage != null)
                bulletImage.sprite = isActive ? activeBullet : inactiveBullet;
        }
    }
}