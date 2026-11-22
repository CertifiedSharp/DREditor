//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
#if UNITY_EDITOR
using UnityEditor;

namespace DREditor
{
    /// <summary>
    /// Concrete class to track classes that inherit <see cref="ClassEditorBase"/> in the unity project.
    /// </summary>
    [InitializeOnLoad]
    public class ClassEditorTypeList : TypeListBase<ClassEditorBase>
    {
        static ClassEditorTypeList()
        {
            if (!SessionState.GetBool("ClassEditorTypeInitDone", false))
            {
                UpdateList();

                SessionState.SetBool("ClassEditorTypeInitDone", true);
            }
        }
    }
}
#endif