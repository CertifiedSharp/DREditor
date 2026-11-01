//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using System.Collections;

namespace DREditor.Dialogues
{
    /// <summary>
    ///
    /// </summary>
    public interface IPhaseStartHandler
    {
        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        int ID { get; }


        /// <summary>
        ///
        /// </summary>
        /// <param name="start"></param>
        /// <returns></returns>
        public IEnumerator OnPhaseStart(StartInvestigation start);

        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        public void AfterLoadingRoom();
    }
}