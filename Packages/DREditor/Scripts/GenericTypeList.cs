//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
//I had the idea for this system after dissecting SeleniumSoul's Dialogue Event List script.
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