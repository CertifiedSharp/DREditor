//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if DOTWEEN
using DG.Tweening;
#endif
using DREditor.Minigames.Debates;
using DREditor.Minigames.Debates.Cycling;
using DREditor.Investigations;
using DREditor.UIHandling;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;

namespace DREditor.Minigames.NSD.Managers
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(UIVisibilityComponent) + ", " + nameof(IUIVisibility) + ", " + nameof(IBulletSendable) +
        "\n\n")]
    public class NSDBulletLoadAnimation : UIVisibilityComponent, IBulletSendable
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected UIVisibilityComponent uiParent = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float resolveBufferDuration = 0.3f;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected Transform loadBulletParent = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected GameObject loadBulletPrefab = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float bulletLoadSpeed = 0.3f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float bulletLoadSpeedMultiplier = 1.25f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Vector3 loadBulletParentPosition = Vector3.zero;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected Vector3 bulletObjectOffset = new(97, 8, 0);

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float doubleSpaceBetweenBullets = 66.7f;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected Image chamberParent = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float loadChamberZ = 66.1f;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float postChamberZ = 80;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected float chamberRotateDuration = 2;


        [Header("")]
        [SerializeField] protected UnityEvent onBulletLoaded;


        /// <summary>
        ///
        /// </summary>
        protected List<GameObject> bulletObjects = new();

        /// <summary>
        ///
        /// </summary>
        protected List<TruthBullet> truthBullets = new();

        /// <summary>
        ///
        /// </summary>
        protected Vector3[] bulletOGPos;

        /// <summary>
        ///
        /// </summary>
        protected Vector3 originalChamberRotation;

        /// <summary>
        ///
        /// </summary>
        protected bool finished = false;


        /// <inheritdoc/>
        public virtual void OnSentBullets(List<TruthBullet> bullets)
        {
            truthBullets = bullets;

            Restart();

            SetBulletParentPosition();

            SpawnBullets();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void Restart()
        {
            foreach (var bullet in bulletObjects)
                Destroy(bullet);

            bulletObjects.Clear();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void SetBulletParentPosition()
        {
            loadBulletParent.transform.localPosition = loadBulletParentPosition;

            if (truthBullets.Count == 1)
                return;

            Vector3 position = loadBulletParent.transform.localPosition;

            float total = 0;

            for (int i = 0; i < truthBullets.Count; i++)
                if (i < truthBullets.Count - 1)
                    total += doubleSpaceBetweenBullets / 2;

            position.y += total;

            loadBulletParent.transform.localPosition = position;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void SpawnBullets()
        {
            for (int i = 0; i < truthBullets.Count; i++)
                bulletObjects.Add(Instantiate(loadBulletPrefab, loadBulletParent));
        }


        /// <inheritdoc/>
        protected override void ShowUI() => StartCoroutine(LoadBulletAnimation());

        /// <inheritdoc/>
        protected override void HideUI() => StartCoroutine(LoadBulletAnimation());

        /// <inheritdoc/>
        protected override IEnumerator WaitForUIToComplete()
        {
            yield return new WaitUntil(() => finished);

            Destroy(gameObject);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator LoadBulletAnimation()
        {
            ShowBulletTrails();

            yield return uiParent.ShowTillComplete();

            yield return ShowChamber();

            yield return LoadBullets();

            yield return PostChamberRotate();

            yield return uiParent.HideTillComplete();

            yield return new WaitForSeconds(resolveBufferDuration);

            finished = true;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator ShowChamber()
        {
            originalChamberRotation = chamberParent.rectTransform.rotation.eulerAngles;

            if (chamberParent.TryGetComponent<UIVisibilityComponent>(out var visibility))
                yield return visibility.ShowTillComplete();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ShowBulletTrails()
        {
            for (int i = 0; i < truthBullets.Count; i++)
                DisplayBulletTrails(i, true);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="i"></param>
        /// <param name="visible"></param>
        protected virtual void DisplayBulletTrails(int i, bool visible)
        {
            if (bulletObjects[i].GetComponentInChildren<UIVisibilityComponent>() is UIVisibilityComponent bulletTrail)
                if (visible)
                    bulletTrail.Show();
                else
                    bulletTrail.Hide();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator LoadBullets()
        {
            bulletOGPos = new Vector3[truthBullets.Count];

            for (int i = 0; i < truthBullets.Count; i++)
            {
                LoadBulletText(i);

                LoadBulletTransform(i);

                RotateChambers(loadChamberZ, bulletLoadSpeed);

                DisplayBulletTrails(i, false);

                onBulletLoaded?.Invoke();

                yield return new WaitForSeconds(bulletLoadSpeed * bulletLoadSpeedMultiplier);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="i"></param>
        protected virtual void LoadBulletText(int i)
        {
            ICycleUITextComponent loadBullet = bulletObjects[i].GetComponentInChildren<ICycleUITextComponent>();

            loadBullet?.UpdateBulletText(truthBullets[i].DisplayName);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="i"></param>
        protected virtual void LoadBulletTransform(int i)
        {
            bulletOGPos[i] = bulletObjects[i].GetComponent<RectTransform>().position;
#if DOTWEEN
            bulletObjects[i].transform.DOMove(bulletOGPos[i] + bulletObjectOffset, bulletLoadSpeed);
#else
            bulletObjects[i].transform.Translate(bulletOGPos[i] + bulletObjectOffset);
#endif
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="initialZMove"></param>
        /// <param name="duration"></param>
        protected virtual void RotateChambers(float initialZMove, float duration)
        {
            originalChamberRotation.z += initialZMove;

            var originalQuaternion = Quaternion.Euler(originalChamberRotation);
#if DOTWEEN
            chamberParent.transform.DORotateQuaternion(originalQuaternion, duration);
#else
            chamberParent.transform.Rotate(originalQuaternion.eulerAngles);
#endif
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual IEnumerator PostChamberRotate()
        {
            RotateChambers(postChamberZ, chamberRotateDuration);

            yield return new WaitForSeconds(chamberRotateDuration);
        }
    }
}