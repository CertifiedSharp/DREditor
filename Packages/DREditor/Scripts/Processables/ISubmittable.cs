//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using System;

namespace DREditor.Processables
{
    /// <summary>
    ///
    /// </summary>
    public interface ISubmittable
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="onComplete"></param>
        void Submit(Action onComplete);
    }
}