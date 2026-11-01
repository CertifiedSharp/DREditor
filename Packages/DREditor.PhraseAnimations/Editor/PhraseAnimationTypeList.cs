//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if UNITY_EDITOR
using UnityEditor;

namespace DREditor.PhraseAnimations.Editor
{
    /// <summary>
    ///
    /// </summary>
    [InitializeOnLoad]
    public class PhraseAnimationTypeList : TypeListBase<IPhraseAnimationType>
    {
        static PhraseAnimationTypeList()
        {
            if (!SessionState.GetBool("PhraseAnimationTypeInitDone", false))
            {
                UpdateList();

                SessionState.SetBool("PhraseAnimationTypeInitDone", true);
            }
        }
    }
}
#endif