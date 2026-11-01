//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor.
using System;
using UnityEditor;
using UnityEditor.PackageManager.Requests;
using UnityEngine;

namespace DREditor.Editor.PackageManaging
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class PackageRequester<T>
    {
        /// <summary>
        ///
        /// </summary>
        protected Request<T> request;

        /// <summary>
        ///
        /// </summary>
        protected readonly Action onRequestCompleted;


        public PackageRequester(Action onRequestCompleted) => this.onRequestCompleted = onRequestCompleted;


        /// <summary>
        ///
        /// </summary>
        /// <param name="request"></param>
        public virtual void Import(Request<T> request = null)
        {
            InitializeAddRequest(request);

            HookRequest();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="request"></param>
        protected virtual void InitializeAddRequest(Request<T> request) => this.request = request;

        /// <summary>
        ///
        /// </summary>
        protected virtual void HookRequest() => EditorApplication.update += OnEditorUpdated;

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnEditorUpdated()
        {
            CheckAddRequest();

            if (HasResolved())
                OnRequestCompleted();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void CheckAddRequest() => request.Check(UnhookRequest, OnRequestSucceded, OnRequestFailed);

        /// <summary>
        ///
        /// </summary>
        protected virtual void UnhookRequest() => EditorApplication.update -= OnEditorUpdated;

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnRequestSucceded() => Debug.Log("REPORT: Request Succeded!");

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnRequestFailed() => Debug.LogError($"ALERT: Request Failed. Reason: {request.Error.message}");

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool HasResolved() => request.Processable();

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnRequestCompleted() => onRequestCompleted();
    }
}