//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Saving;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.DialogueMechanics.Investigation
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(IDRESaveable) + ", " + nameof(IDRELoadableManager) + ", " + nameof(IPlayerDataClearer) +
        "\n\n")]
    public class TruthBulletDataHandler : MonoBehaviour, IDRESaveable, IDRELoadableManager, IPlayerDataClearer
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected bool debugLogs = false;


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        public List<string> foundBullets = new();

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public List<string> pieceBullets = new();


        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected string foundKey = "foundBullets";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected string pieceKey = "pieceBullets";
        

        /// <summary>
        ///
        /// </summary>
        /// <param name="name"></param>
        public virtual void AddBullet(string name) => AddBulletObject(foundBullets, name);

        /// <summary>
        ///
        /// </summary>
        /// <param name="name"></param>
        public virtual void AddPiece(string name) => AddBulletObject(pieceBullets, name);

        /// <summary>
        ///
        /// </summary>
        /// <param name="playerBullets"></param>
        /// <param name="name"></param>
        protected virtual void AddBulletObject(List<string> playerBullets, string name)
        {
            if (playerBullets.Contains(name))
                LogAlreadyContains(name);
            else
                playerBullets.Add(name);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="name"></param>
        protected virtual void LogAlreadyContains(string name)
        {
            if (debugLogs)
                UnityEngine.Debug.Log("REPORT: Truth Bullet/Piece: " + name + " has already been added!");
        }

        /// <summary>
        ///
        /// </summary>
        public virtual void ClearTruthBulletData()
        {
            foundBullets.Clear();

            pieceBullets.Clear();
        }


        /// <inheritdoc/>
        public virtual void LoadManager(Dictionary<string, object> saveDatas, bool inChapterSelect)
        {
            foundBullets = saveDatas.ContainsKey(foundKey) ? saveDatas.Get<List<string>>(foundKey) : new();

            pieceBullets = saveDatas.ContainsKey(pieceKey) ? saveDatas.Get<List<string>>(pieceKey) : new();
        }

        /// <inheritdoc/>
        public virtual void Save(Dictionary<string, object> saveDatas)
        {
            if (foundBullets.Count == 0 && pieceBullets.Count == 0)
                return;
            
            saveDatas.Set(foundKey, foundBullets);

            saveDatas.Set(pieceKey, pieceBullets);
        }

        /// <inheritdoc/>
        public virtual void ClearData() => ClearTruthBulletData();
    }
}