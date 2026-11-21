//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor
using System;

namespace DREditor.Processables
{
    /// <summary>
    /// Interface for Submittable/Confirmable processes.
    /// </summary>
    public interface ISubmittable
    {
        /// <summary>
        /// Submit/Confirm process for this script, invoking a call back function on complete.
        /// </summary>
        /// <param name="onComplete">Call back function for when the submit process has been completed.</param>
        void Submit(Action onComplete);
    }
}