//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;
using System.Collections.Generic;
using UnityEditor;

namespace DREditor.Editor.AssetTextEditing
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class AssetEditorBase<T>
    {
        /// <summary>
        ///
        /// </summary>
        private readonly List<AssetEditProcess<T>> processes;


        /// <summary>
        ///
        /// </summary>
        public virtual List<AssetEditProcess<T>> Processes => processes;


        protected AssetEditorBase(AssetEditProcess<T> process) => processes = new() { process };

        protected AssetEditorBase(List<AssetEditProcess<T>> processes) => this.processes = processes;


        /// <summary>
        ///
        /// </summary>
        public virtual void ApplyAssetEditing()
        {
            if (InitializationPassed())
                ApplyProcessesToTargets();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        protected virtual bool InitializationPassed()
        {
            foreach (var editProcess in processes)
                if (!editProcess?.IsValid() ?? true)
                    return false;

            return true;
        }

        /// <summary>
        ///
        /// </summary>
        private void ApplyProcessesToTargets()
        {
            using (new AssetDatabase.AssetEditingScope())
                EditAssets();

            OnEditingCompleted();
        }

        /// <summary>
        ///
        /// </summary>
        private void EditAssets()
        {
            try
            {
                PrepareEditorToEdit();

                InvokeAssetEditing();
            }
            catch (Exception exception)
            {
                OnApplicationException(exception);
            }
        }

        /// <summary>
        ///
        /// </summary>
        protected abstract void PrepareEditorToEdit();

        /// <summary>
        ///
        /// </summary>
        protected virtual void InvokeAssetEditing() => processes.ForEach(InvokeEditProcess);

        /// <summary>
        ///
        /// </summary>
        /// <param name="editProcess"></param>
        protected abstract void InvokeEditProcess(AssetEditProcess<T> editProcess);

        /// <summary>
        ///
        /// </summary>
        /// <param name="exception"></param>
        protected virtual void OnApplicationException(Exception exception)
        {
            UnityEngine.Debug.LogWarning("ALERT: Asset Editing Failed, See below for the reason.");

            UnityEngine.Debug.LogException(exception);
        }

        /// <summary>
        ///
        /// </summary>
        protected abstract void OnEditingCompleted();
    }
}