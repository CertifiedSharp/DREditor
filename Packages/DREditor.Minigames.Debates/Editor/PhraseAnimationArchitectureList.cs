//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Minigames.Debates;
using UnityEditor;

namespace DREditor.Minigames.Debates.Editor
{
    /// <summary>
    ///
    /// </summary>
    [InitializeOnLoad]
    public class PhraseAnimationArchitectureList : TypeListBase<IPhraseAnimationArchitecture>
    {
        static PhraseAnimationArchitectureList()
        {
            if (!SessionState.GetBool("PhraseAnimationArchitectureInitDone", false))
            {
                UpdateList();

                SessionState.SetBool("PhraseAnimationArchitectureInitDone", true);
            }
        }
    }
}