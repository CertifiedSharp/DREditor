//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections.Generic;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;

namespace DREditor.Editor.Dependencies
{
    /// <summary>
    ///
    /// </summary>
    public class InstallationState
    {
        /// <summary>
        ///
        /// </summary>
        public readonly Queue<string> PackagesToInstall = null;

        /// <summary>
        ///
        /// </summary>
        public AddRequest CurrentAddRequest { get; set; }


        public InstallationState(Queue<string> PackagesToInstall) => this.PackagesToInstall = PackagesToInstall;

        
        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public bool PackageQueueInvalid() => PackagesToInstall == null || PackagesToInstall.Count == 0;

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public bool AddRequestInvalid() => CurrentAddRequest == null || !CurrentAddRequest.IsCompleted;

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public bool CurrentRequestSucceded() => CurrentAddRequest.Status == StatusCode.Success;
    }
}