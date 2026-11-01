//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;

namespace DREditor.Saving
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [Serializable]
    public class Wrapper<T>
    {
        /// <summary>
        ///
        /// </summary>
        public T value;

        public Wrapper(T value) => this.value = value;
    }
}