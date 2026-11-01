//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.EventObjects;
using DREditor.Processables;

namespace DREditor.SubAreas
{
    /// <summary>
    ///
    /// </summary>
    public interface ISubArea : INameDisplayable, IDialogueHolder, ISelectableObject
    {
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        object Save();

        /// <summary>
        ///
        /// </summary>
        /// <param name="areaData"></param>
        void Load(object areaData);
    }
}