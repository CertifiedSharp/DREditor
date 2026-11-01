//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.DialogueEventHandling;
using DREditor.Investigations;
using DREditor.Processables;
using UnityEngine;

namespace DREditor.DialogueMechanics.Investigation
{
    /// <summary>
    ///
    /// </summary>
    [ClassHeader(nameof(DialogueEventHandler) +
        "\n\n")]
    public class TruthBulletGetEventHandler : DialogueEventHandler
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected TruthBulletDataHandler bulletDataHandler = null;


        /// <summary>
        ///
        /// </summary>
        protected virtual void Awake() => SetDefaultEventString("TruthBulletGet");


        /// <summary>
        ///
        /// </summary>
        /// <param name="o"></param>
        protected override void OnDialogueEventTriggered(object o = null) => CheckTruthBulletGet((TruthBulletGet)o);

        /// <summary>
        ///
        /// </summary>
        /// <param name="truthBulletData"></param>
        protected virtual void CheckTruthBulletGet(TruthBulletGet truthBulletData)
        {
            if (truthBulletData.truthBullet != null)
                AddTruthBulletOnHandlerFound(truthBulletData);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="truthBulletData"></param>
        protected virtual void AddTruthBulletOnHandlerFound(TruthBulletGet truthBulletData)
        {
            if (bulletDataHandler != null)
            {
                bulletDataHandler.AddBullet(truthBulletData.truthBullet.DisplayName);

                string fragmentDisplayName = truthBulletData.truthBullet
                .OfType<IBuildableDescription>()?[truthBulletData.pieceIndex];

                if (!string.IsNullOrEmpty(fragmentDisplayName))
                    bulletDataHandler.AddPiece(fragmentDisplayName);
            }
            else
                UnityEngine.Debug.LogError($"{ToString()}: Instance of a {typeof(TruthBulletDataHandler)} was not referenced," +
                    $"\nso the player data cannot be updated.");
        }
    }
}