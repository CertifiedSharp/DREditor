//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
#if UNITY_EDITOR
using System.Collections.Generic;

namespace DREditor.Editor.Utility
{
    /// <summary>
    ///
    /// </summary>
    public interface INestedEditorCacheable
    {
        /// <summary>
        ///
        /// </summary>
        Dictionary<object, bool> VisibilityCaches { get; }

        /// <summary>
        ///
        /// </summary>
        Dictionary<object, int> IndexCaches { get; }


        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="data"></param>
        /// <returns></returns>
        bool TryGetData<T>(out T data);

        /// <summary>
        ///
        /// </summary>
        /// <param name="obj"></param>
        void AddData(object obj);
    }
}
#endif