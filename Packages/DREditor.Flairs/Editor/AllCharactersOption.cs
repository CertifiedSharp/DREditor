//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using DREditor.Dialogues;
using DREditor.Editor.Utility;
using System;

namespace DREditor.Flairs.Editor
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class AllCharactersOption : GenerateFlairDialogueOptionBase
    {
        /// <inheritdoc/>
        protected override void GenerateDialogue()
        {
            foreach (IFlairEvent sceneEvent in events)
                AllCharactersOneFlair(sceneEvent);
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="flair"></param>
        protected virtual void AllCharactersOneFlair(IFlairEvent flair)
        {
            Dialogue dia = MakeInstance<Dialogue>();

            dia.Lines.Clear();

            string flairName = flair.GetFlair().GetPrefab().name;

            dia.name = "D_" + flairName + "_Debug";

            foreach (var c in cdb.Characters)
                dia.Lines.Add(CharacterFlairLine(c, flair, flairName));

            BuilderUtility.CreateAsset(dia, targetDirectory);
        }
    }
}
#endif