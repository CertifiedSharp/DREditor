//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Editor.Utility;
using DREditor.Investigations;
using DREditor.Processables;
using DREditor.Progressing;
using DREditor.Progressing.Handling;
using UnityEditor;
using UnityEngine;

namespace DREditor.DialogueMechanics.Investigation.Editor
{
    /// <summary>
    ///
    /// </summary>
    public static class TruthBulletUtility
    {
        /// <summary>
        ///
        /// </summary>
        [MenuItem("DREditor/Truth Bullets/Add All Truth Bullets from current chapter.")]
        public static void GetAllTruthBulletsOnCurrentChapter()
        {
            ProgressionDatabase progressionDatabase = DatabaseUtility.LoadDatabase<ProgressionDatabase>();

            int chapterIndex = progressionDatabase
                .GetChapterIndex(MonoBehaviour.FindAnyObjectByType<ProgressionManagerBase>().GetCurrentChapter());

            TruthBulletDataHandler playerInfo = MonoBehaviour.FindAnyObjectByType<TruthBulletDataHandler>();

            EvidenceDatabase evidenceDatabase = DatabaseUtility.LoadDatabase<EvidenceDatabase>();

            playerInfo.ClearTruthBulletData();

            foreach(var bullet in evidenceDatabase.Evidences[chapterIndex].TruthBullets)
            {
                playerInfo.AddBullet(bullet.DisplayName);

                if (bullet.OfType<IBuildableDescription>() is IBuildableDescription collection &&
                    collection.GetFragmentNames() is string[] fragments)
                    foreach (var piece in fragments)
                        playerInfo.AddPiece(piece);
            }
        }
    }
}