//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections;
using System.Collections.Generic;

namespace DREditor.Minigames.Debates.Managers
{
    /// <summary>
    ///
    /// </summary>
    public interface IDebateLineManager
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="panels"></param>
        IEnumerator PlayLines<T>(List<T> panels) where T : Panel;

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        Panel GetCurrentPanel();

#if UNITY_EDITOR
        /// <summary>
        ///
        /// </summary>
        /// <param name="panel"></param>
        void PlayTestPanel<T>(T panel) where T : Panel;
#endif
    }
}