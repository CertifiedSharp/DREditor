//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Minigames.Debates;
using UnityEditor;

namespace DREditor.Minigames.Debates.Editor
{
    /// <summary>
    ///
    /// </summary>
    [InitializeOnLoad]
    public class PhraseTypeList : TypeListBase<PhraseBase>
    {
        static PhraseTypeList()
        {
            if (!SessionState.GetBool("PhraseTypeInitDone", false))
            {
                UpdateList();

                SessionState.SetBool("PhraseTypeInitDone", true);
            }
        }
    }
}