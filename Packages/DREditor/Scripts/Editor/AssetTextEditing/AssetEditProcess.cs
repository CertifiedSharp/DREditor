//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;

namespace DREditor.Editor.AssetTextEditing
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public class AssetEditProcess<T>
    {
        /// <summary>
        ///
        /// </summary>
        public T Target { get; set; }

        /// <summary>
        ///
        /// </summary>
        public Action<T> Action { get; set; }


        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual bool IsValid() => Target != null && Action != null;
    }
}