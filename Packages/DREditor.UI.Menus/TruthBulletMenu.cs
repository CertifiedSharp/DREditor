//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.DialogueMechanics.Investigation;
using DREditor.EventObjects;
using DREditor.Investigations;
using DREditor.Processables;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace DREditor.UI.Menus
{
    /// <summary>
    ///
    /// </summary>
    /// <remarks></remarks>
    [ClassHeader(nameof(MenuModifier) +
        "\n\n")]
    public class TruthBulletMenu : MenuModifier
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected Evidence evidence = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected List<TruthBullet> bullets;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected EvidenceDatabase evidenceDatabase = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected ScrollOptionModifier scrollGroup = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected IntWithEvent currentChapterIndex = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected TruthBulletDataHandler TruthBulletDataHandler = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected GameObject bulletOptionPrefab = null;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected Image bulletImage = null;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected TextMeshProUGUI bulletText = null;


        /// <summary>
        ///
        /// </summary>
        protected virtual void OnEnable() => menu.BeforeStart.AddListener(Load);

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnDisable() => menu.BeforeStart.RemoveListener(Load);

        /// <summary>
        ///
        /// </summary>
        public virtual void Load()
        {
            InitializeEvidence();

            LoadTruthBullets();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeEvidence()
        {
            int chapterIndex = currentChapterIndex.Value;

            if (chapterIndex >= 0 && chapterIndex < evidenceDatabase.Evidences.Count)
                evidence = evidenceDatabase.Evidences[chapterIndex];
            else
                UnityEngine.Debug.LogError("You do not have an evidence asset in the evidence database for the current chapter!");
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void LoadAllBulletsOnEmpty()
        {
            if (TruthBulletDataHandler.foundBullets.Count == 0)
                foreach (TruthBullet bullet in evidence.TruthBullets)
                {
                    TruthBulletDataHandler.AddBullet(bullet.DisplayName);

                    if (bullet.OfType<IBuildableDescription>() is IBuildableDescription collection)
                        foreach (var fragment in collection.GetFragmentNames())
                            TruthBulletDataHandler.AddPiece(fragment);
                }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void LoadTruthBullets()
        {
            LoadAllBulletsOnEmpty();

            InitializeTruthBullets();

            SpawnTruthBulletOptions();

            DisplayTruthBulletOptions();

            AttemptDisplayBulletInfo();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeTruthBullets()
        {
            bullets = new();

            foreach (var foundBullet in TruthBulletDataHandler.foundBullets)
                bullets.Add(evidence.GetBullet(foundBullet));
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void SpawnTruthBulletOptions()
        {
            if (bulletOptionPrefab == null)
                UnityEngine.Debug.LogWarning("ALERT: " +
                        "TBM does not have enough scroll options for the amount of truth bullets for this chapter. " +
                        "\n It is suggested you either set up more Truth Bullet Options or assign a BulletOptionPrefab to the TBM.");
            else if (scrollGroup.OptionCount() < evidence.TruthBullets.Count && 
                evidence.TruthBullets.Count - scrollGroup.OptionCount() is int missingCount)
                for (int i = 0; i < missingCount; i++)
                    InstantiateBulletOptionPrefab();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayTruthBulletOptions()
        {
            for (int i = 0; i < TruthBulletDataHandler.foundBullets.Count; i++)
                scrollGroup.EnableOptionAt(i, bullets[i].DisplayName);

            scrollGroup.DisableUnused(TruthBulletDataHandler.foundBullets.Count);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void InstantiateBulletOptionPrefab()
        {
            GameObject bulletOption = scrollGroup.InstantiateScrollOption(bulletOptionPrefab);

            SelectableUIOption selectableOption = bulletOption.GetComponent<SelectableUIOption>();

            selectableOption.Selection.AddListener(() => DisplayBulletInfo(bulletOption));
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void AttemptDisplayBulletInfo()
        {
            if (menu is SelectableMenuGroupBase selectableMenu)
                DisplayBulletInfo(selectableMenu.EvaluateSelection());
            else
                UnityEngine.Debug.LogWarning($"{ToString()}: ALERT: Loading Bullet Menu did not contain a "
                    + nameof(SelectableMenuGroupBase) + "! Either use one or extend this functionality to not require it.");
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="current"></param>
        protected virtual void DisplayBulletInfo(GameObject current)
        {
            TruthBullet truthBullet = evidence.GetBullet(current.GetComponentInChildren<TextMeshProUGUI>().text);

            DisplayBulletInfo(truthBullet);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="truthBullet"></param>
        protected virtual void DisplayBulletInfo(TruthBullet truthBullet)
        {
            if (truthBullet.Image != null)
                bulletImage.sprite = truthBullet.Image;

            string builtDescription = truthBullet
                .OfType<IBuildableDescription>()?[truthBullet.Description, TruthBulletDataHandler.pieceBullets];

            if (!string.IsNullOrEmpty(builtDescription))
                bulletText.text = builtDescription;
            else
                bulletText.text = truthBullet.Description;
        }
    }
}