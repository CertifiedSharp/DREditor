//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;

namespace DREditor.Triggers
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public abstract class GenericSubsequent : GenericTriggerBase
    {
        protected GenericSubsequent() { }

        protected GenericSubsequent(string Type) : base(Type) { }


        /// <summary>
        ///
        /// </summary>
        /// <param name="instance"></param>
        /// <returns></returns>
        public abstract GenericSubsequent MergeInstance(GenericSubsequent instance);
    }
}