//Idea originally from the Dialogue Event List script Author: SeleniumSoul for DREditor
//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
#if UNITY_EDITOR
using UnityEditor;

namespace DREditor
{
    /// <summary>
    ///
    /// </summary>
    [InitializeOnLoad]
    public class GenericTypeList<T> : TypeListBase<T>
    {
        static GenericTypeList()
        {
            if (!SessionState.GetBool($"{typeof(T).Name}InitDone", false))
            {
                UpdateList();

                SessionState.SetBool($"{typeof(T).Name}InitDone", true);
            }
        }
    }
}
#endif