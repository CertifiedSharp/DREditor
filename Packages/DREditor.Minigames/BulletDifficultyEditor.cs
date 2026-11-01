//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using DREditor.Investigations;
using DREditor.States;
using DREditor.Editor.Utility;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace DREditor.Minigames
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class BulletDifficultyEditor : DifficultyEditorBase<List<BulletDifficulty>>
    {
        /// <summary>
        ///
        /// </summary>
        protected EvidenceDatabase evidenceDatabase = null;

        /// <summary>
        ///
        /// </summary>
        protected List<BulletDifficulty> bulletDifficulties;

        /// <summary>
        ///
        /// </summary>
        protected List<TruthBullet> inheritedBullets;

        /// <summary>
        ///
        /// </summary>
        protected List<TruthBullet> ownedBullets;

        /// <summary>
        ///
        /// </summary>
        protected List<TruthBullet> difficultyBullets;

        /// <summary>
        ///
        /// </summary>
        protected StateAsset difficulty;

        /// <summary>
        ///
        /// </summary>
        protected int chapterIndex;

        /// <summary>
        ///
        /// </summary>
        protected int ownedIndex;


        /// <summary>
        ///
        /// </summary>
        /// <param name="bulletDifficulties"></param>
        /// <param name="chapterIndex"></param>
        /// <returns></returns>
        public virtual List<BulletDifficulty> DisplaySettings(List<BulletDifficulty> bulletDifficulties, ref int chapterIndex)
        {
            this.chapterIndex = chapterIndex;

            this.bulletDifficulties = bulletDifficulties;

            base.DisplaySettings(bulletDifficulties);

            if (difficultyDatabase == null ||
                !DatabaseUtility
                .InitializeDatabase(evidenceDatabase, out evidenceDatabase))
                return this.bulletDifficulties;

            foldout = EditorGUILayout.Foldout(foldout, "Truth Bullets", largeFoldoutStyle);
            
            if (foldout)
            {
                GUILayout.Space(10);

                chapterIndex = HandyFields.IntField("Current Chapter: ", chapterIndex, 20);

                if (chapterIndex < evidenceDatabase.Evidences.Count)
                {
                    PopulateMissingDifficulties();

                    DisplayDifficulties();
                }
            }

            EditorGUILayout.EndFoldoutHeaderGroup();

            return this.bulletDifficulties;
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void PopulateMissingDifficulties()
        {
            for (int i = 0; i < difficultyDatabase.States.Count; i++)
                if (i >= bulletDifficulties.Count)
                    bulletDifficulties.Add(new(difficultyDatabase.States[i]));
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayDifficulties()
        {
            for (int i = 0; i < difficultyDatabase.States.Count; i++)
                DisplayDifficulty(i);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="i"></param>
        protected virtual void DisplayDifficulty(int i)
        {
            InitializeInheritedBullets(i);

            difficulty = bulletDifficulties[i].difficulty;

            difficultyBullets = bulletDifficulties[i].bullets;

            DisplayPair();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="i"></param>
        protected virtual void InitializeInheritedBullets(int i)
        {
            inheritedBullets = new();

            for (int prev = 0; prev < i; prev++)
                foreach (var bullet in bulletDifficulties[prev].bullets)
                    if (!inheritedBullets.Contains(bullet))
                        inheritedBullets.Add(bullet);
        }

        
        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayPair()
        {
            EditorGUILayout.LabelField("Truth Bullets for: " + difficulty.stateName);

            DisplayInheritedBullets();

            ValidateInheritedBullets();

            DisplayOwnedBullets();

            DisplayAddButton();

            RemoveAllBulletDuplicates();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayInheritedBullets()
        {
            if (inheritedBullets.Count > 0)
            {
                GUI.enabled = false;

                for (int i = 0; i < inheritedBullets.Count; i++)
                {
                    using (new EditorGUILayout.HorizontalScope())
                    {
                        Evidence trialEvidence = evidenceDatabase.Evidences[chapterIndex];

                        int choiceIndex = trialEvidence.TruthBullets.IndexOf(inheritedBullets[i]);

                        EditorGUILayout.IntPopup(choiceIndex, trialEvidence.ToStringArray(), trialEvidence.TointArray());

                        EditorGUILayout.LabelField("(Inherited)", GUILayout.Width(80));
                    }
                }

                GUI.enabled = true;
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void ValidateInheritedBullets()
        {
            for (int i = 0; i < inheritedBullets.Count; i++)
                if (i >= difficultyBullets.Count)
                    difficultyBullets.Add(inheritedBullets[i]);
                else
                    difficultyBullets[i] = inheritedBullets[i];
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayOwnedBullets()
        {
            ownedBullets = new();

            foreach (var bullet in difficultyBullets)
                if (!inheritedBullets.Contains(bullet))
                    ownedBullets.Add(bullet);

            for (int i = 0; i < ownedBullets.Count; i++)
                using (new EditorGUILayout.HorizontalScope())
                    DisplayOwnedBullet(i);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="i"></param>
        protected virtual void DisplayOwnedBullet(int i)
        {
            ownedIndex = i;

            DisplayBullet();

            DisplayRemoveButton();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayBullet()
        {
            InitializeValidBullets(out List<TruthBullet> availableBullets, out List<string> availableNames);

            DisplayValidBullet(availableBullets, availableNames);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="availableBullets"></param>
        /// <param name="availableNames"></param>
        protected virtual void InitializeValidBullets(out List<TruthBullet> availableBullets, out List<string> availableNames)
        {
            Evidence trialEvidence = evidenceDatabase.Evidences[chapterIndex];

            availableBullets = new();

            availableNames = new();

            for (int j = 0; j < trialEvidence.TruthBullets.Count; j++)
            {
                TruthBullet evidenceBullet = trialEvidence.TruthBullets[j];

                if (!difficultyBullets.Contains(evidenceBullet) ||
                    evidenceBullet == ownedBullets[ownedIndex])
                {
                    availableBullets.Add(evidenceBullet);

                    availableNames.Add(trialEvidence.ToStringArray()[j]);
                }
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="availableBullets"></param>
        /// <param name="availableNames"></param>
        protected virtual void DisplayValidBullet(List<TruthBullet> availableBullets, List<string> availableNames)
        {
            int choiceIndex = availableBullets.IndexOf(ownedBullets[ownedIndex]);

            if (choiceIndex == -1 && availableBullets.Count > 0)
                choiceIndex = 0;

            if (availableBullets.Count > 0)
            {
                int newIndex = EditorGUILayout.IntPopup(choiceIndex, availableNames.ToArray(),
                    Enumerable.Range(0, availableBullets.Count).ToArray());

                if (newIndex != choiceIndex)
                {
                    difficultyBullets.Remove(ownedBullets[ownedIndex]);

                    ownedBullets[ownedIndex] = availableBullets[newIndex];

                    difficultyBullets.Add(ownedBullets[ownedIndex]);
                }
            }
            else
                EditorGUILayout.LabelField("No available bullets");
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayRemoveButton()
        {
            if (GUILayout.Button("Remove Bullet"))
                RemoveInheritedBullet();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void RemoveInheritedBullet()
        {
            int difficultyIndex = bulletDifficulties.IndexOf(
                        bulletDifficulties
                            .Where(pair => pair.difficulty == difficulty)
                            .First()
                        );

            for (int x = difficultyIndex; x < bulletDifficulties.Count; x++)
                bulletDifficulties[x].bullets.RemoveAt(ownedIndex);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void DisplayAddButton()
        {
            if (difficultyBullets.Count != evidenceDatabase.Evidences[chapterIndex].TruthBullets.Count &&
                GUILayout.Button("Add Bullet"))
            {
                Evidence trialEvidence = evidenceDatabase.Evidences[chapterIndex];

                TruthBullet bulletToAdd = null;

                foreach (var bullet in trialEvidence.TruthBullets)
                    if (!difficultyBullets.Contains(bullet))
                    {
                        bulletToAdd = bullet;

                        break;
                    }

                AddOwnedBullet(bulletToAdd);
            }
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="bulletToAdd"></param>
        protected virtual void AddOwnedBullet(TruthBullet bulletToAdd)
        {
            if (bulletToAdd == null)
                return;

            difficultyBullets.Add(bulletToAdd);

            int insertIndex = difficultyBullets.IndexOf(bulletToAdd);

            int difficultyIndex = bulletDifficulties
                .IndexOf(bulletDifficulties.Where(pair => pair.difficulty == difficulty).First());

            for (int i = difficultyIndex + 1; i < bulletDifficulties.Count; i++)
                bulletDifficulties[i].bullets.Insert(insertIndex, bulletToAdd);
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void RemoveAllBulletDuplicates()
        {
            for (int i = 0; i < bulletDifficulties.Count; i++)
                bulletDifficulties[i].RemoveDuplicateBullets();
        }
    }
}
#endif