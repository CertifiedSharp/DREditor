//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using DREditor.Items;
using DREditor.Editor.Utility;
using System;
using UnityEditor;
using UnityEngine;

namespace DREditor.Presents.Editor
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public abstract class ReactionImportOption : PresentImportOptionBase
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected ReactionLevelDatabase reactionDatabase;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int reactionLevel;


        /// <inheritdoc/>
        protected override void InitializeDatabases()
        {
            base.InitializeDatabases();

            reactionDatabase = DatabaseUtility.LoadDatabase<ReactionLevelDatabase>();
        }

        /// <inheritdoc/>
        protected override bool InitializationFailed() => base.InitializationFailed() || ReactionIndexInvalid();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool ReactionIndexInvalid()
        {
            bool result = reactionDatabase != null && !reactionDatabase.ReactionIndexValid(reactionLevel);

            if (result)
                UnityEngine.Debug.LogWarning("ALERT: Database missing or the reaction index inputted was not " +
                    "a valid one within the reaction database." +
                    "\nPROPOSAL: Ensure the index of the reaction you're using is valid.");

            return result;
        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="InventoryItem"></param>
        /// <param name="matchingName"></param>
        protected virtual void ApplyReactions(InventoryItem InventoryItem, string matchingName)
        {
            if (InventoryItem is Present present)
                foreach (var data in present.attributes)
                    if (data is StudentReactionCollection collection)
                        collection.SetReaction(matchingName, reactionLevel);

            EditorUtility.SetDirty(InventoryItem);
        }
    }
}
#endif