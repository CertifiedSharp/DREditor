//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using DREditor.Characters;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.Presents.Editor
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class ReactionByIDOption : ReactionImportOption
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [SerializeField] protected Character character;


        /// <inheritdoc/>
        public override void Invoke() => SetReactionByID();

        /// <summary>
        ///
        /// </summary>
        protected virtual void SetReactionByID()
        {
            List<int> ids = GetListOfIDs();

            foreach (int id in ids)
                SetReaction(id);

            Close();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="id"></param>
        protected virtual void SetReaction(int id)
        {
            if (presentDatabase.GetPresentByID(id) is Present present)
                ApplyReactions(present, character.FirstName);
            else
                UnityEngine.Debug.LogWarning("ALERT: Couldn't Find Present With ID: " + id);
        }
    }
}
#endif