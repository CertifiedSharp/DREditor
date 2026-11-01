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
    public class AllSpritesOption : GenerateFlairDialogueOptionBase
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int speakerNumber = -1;


        /// <inheritdoc/>
        protected override void GenerateDialogue()
        {
            foreach (IFlairEvent sceneEvent in events)
                OneCharacterAllSprites(sceneEvent);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="flair"></param>
        protected virtual void OneCharacterAllSprites(IFlairEvent flair)
        {
            Dialogue dia = MakeInstance<Dialogue>();

            dia.Lines.Clear();

            string flairName = flair.GetFlair().GetPrefab().name;

            dia.name = "D_" + cdb.Characters[speakerNumber].FirstName + "_All_Sprites_" + flairName + "_Debug";

            Character speaker = cdb.Characters[speakerNumber];

            for (int i = 0; i < speaker.Sprites.Count; i++)
                dia.Lines.Add(CharacterFlairLine(speaker, flair, flairName, i));

            BuilderUtility.CreateAsset(dia, targetDirectory);
        }
    }
}
#endif