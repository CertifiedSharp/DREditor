//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if DOTWEEN
using DG.Tweening;
#endif
using DREditor.Minigames.Debates.Mechanics;
using UnityEngine;

namespace DREditor.Minigames.Debates.Cycling
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(ICycleListener) +
        "\n\n")]
    public class BulletChamberUI : MonoBehaviour, ICycleListener
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Transform bulletChamber;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float chamberZRotation = 60;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float chamberDuration = 0.3f;


        /// <inheritdoc/>
        public virtual void OnBulletCycled(bool isUp)
        {
            if (bulletChamber == null)
                return;

            Vector3 chamberRotation = bulletChamber.rotation.eulerAngles;

            if (!isUp)
                chamberRotation.z *= -1;

            chamberRotation.z += chamberZRotation;

            if (bulletChamber != null)
#if DOTWEEN
                bulletChamber.DORotateQuaternion(Quaternion.Euler(chamberRotation), chamberDuration)
                .SetUpdate(true);
#else
                bulletChamber.Rotate(chamberRotation);
#endif
        }
    }
}