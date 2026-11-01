//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor.
using System;
using UnityEditor.PackageManager;
using UnityEditor.PackageManager.Requests;

namespace DREditor.Editor.PackageManaging
{
    /// <summary>
    ///
    /// </summary>
    public static class RequestUtility
    {
        /// <summary>
        ///
        /// </summary>
        /// <param name="request"></param>
        /// <param name="onProcessable"></param>
        /// <param name="onSucceded"></param>
        /// <param name="onFailed"></param>
        public static void Check(this Request request, Action onProcessable, Action onSucceded, Action onFailed)
        {
            if (!request.Processable())
                return;

            onProcessable();

            if (request.Succeded())
                onSucceded();
            else
                onFailed();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static bool Processable(this Request request) => request.IsCompleted && request.Status != StatusCode.InProgress;

        /// <summary>
        ///
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        public static bool Succeded(this Request request) => request.Status == StatusCode.Success;
    }
}