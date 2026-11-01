//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Minigames.Debates;
using UnityEditor;

namespace DREditor.Minigames.Debates.Editor
{
    /// <summary>
    ///
    /// </summary>
    [InitializeOnLoad]
    public class PhraseModifierList : TypeListBase<IPhraseModifier>
    {
        static PhraseModifierList()
        {
            if (!SessionState.GetBool("PhraseModifierInitDone", false))
            {
                UpdateList();

                SessionState.SetBool("PhraseModifierInitDone", true);
            }
        }
    }
}