//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
//I had the idea for this system after dissecting SeleniumSoul's Dialogue Event List script.
#if UNITY_EDITOR
using UnityEditor;

namespace DREditor
{
    /// <summary>
    /// Tracks a list of classes that inherit that inherit <typeparamref name="T"/> in a unity project.
    /// </summary>
    /// <typeparam name="T">The base type to track the family of.</typeparam>
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