//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using DREditor.Characters;
using DREditor.Dialogues;
using DREditor.Editor.Utility;
using System;
using UnityEngine;

namespace DREditor.Flairs.Editor
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class AllFlairsOneCharacterOption : GenerateFlairDialogueOptionBase
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int speakerNumber = -1;


        /// <inheritdoc/>
        protected override void GenerateDialogue() => AllFlairsOneCharacter();

        /// <summary>
        ///
        /// </summary>
        protected virtual void AllFlairsOneCharacter()
        {
            Dialogue dia = MakeInstance<Dialogue>();

            dia.Lines.Clear();

            dia.name = "D_" + cdb.Characters[speakerNumber].FirstName + "_All_Flairs_Debug";

            Character speaker = cdb.Characters[speakerNumber];

            foreach (var f in events)
                dia.Lines.Add(CharacterFlairLine(speaker, f, f.GetFlair().GetPrefab().name));

            BuilderUtility.CreateAsset(dia, targetDirectory);
        }
    }
}
#endif