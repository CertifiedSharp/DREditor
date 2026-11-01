//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For DREditor.
using UnityEditor;
using UnityEditor.PackageManager.Requests;

namespace DREditor.Editor.PackageManaging
{
    /// <summary>
    ///
    /// </summary>
    public abstract class RequestPackageWizardBase<T> : ScriptableWizard
    {
        /// <summary>
        ///
        /// </summary>
        protected Request<T> request;

        /// <summary>
        ///
        /// </summary>
        protected bool updateHooked = false;


        protected virtual void OnWizardCreate()
        {
            if (ShouldRequestPackages())
                HookPackageRequest();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool ShouldRequestPackages() => !updateHooked;

        /// <summary>
        ///
        /// </summary>
        protected virtual void HookPackageRequest()
        {
            request = CreateRequest();

            EditorApplication.update += CheckRequestStatus;

            updateHooked = true;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected abstract Request<T> CreateRequest();

        /// <summary>
        ///
        /// </summary>
        protected virtual void CheckRequestStatus() =>
            request.Check(UnhookPackageRequest, OnRetrievingPackagesSucceded, OnRetrievingPackagesFailed);

        /// <summary>
        ///
        /// </summary>
        protected virtual void UnhookPackageRequest()
        {
            EditorApplication.update -= CheckRequestStatus;

            updateHooked = false;
        }

        /// <summary>
        ///
        /// </summary>
        protected abstract void OnRetrievingPackagesSucceded();

        /// <summary>
        ///
        /// </summary>
        protected virtual void OnRetrievingPackagesFailed() =>
            UnityEngine.Debug.LogError($"ALERT: The request to obtain the packages list failed. Reason: {request.Error.message}");
    }
}