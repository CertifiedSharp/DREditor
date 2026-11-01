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
    public class AssetEditor<T> : AssetEditorBase<T> where T : UnityEngine.Object
    {
        /// <summary>
        ///
        /// </summary>
        protected bool calledException = false;

        /// <summary>
        ///
        /// </summary>
        protected int undoIndex;

        /// <summary>
        ///
        /// </summary>
        protected int editCount = 0;


        public AssetEditor(AssetEditProcess<T> process) : base(process) { }

        public AssetEditor(List<AssetEditProcess<T>> processes) : base(processes) { }


        protected override void PrepareEditorToEdit()
        {
            SetCalledException(false);

            ClearCount();

            InitializeUndoGroup();
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="calledException"></param>
        protected virtual void SetCalledException(bool calledException) => this.calledException = calledException;

        /// <summary>
        ///
        /// </summary>
        protected virtual void ClearCount() => editCount = 0;

        /// <summary>
        ///
        /// </summary>
        protected virtual void InitializeUndoGroup()
        {
            undoIndex = Undo.GetCurrentGroup();

            Undo.SetCurrentGroupName("Modify Objects");
        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="editProcess"></param>
        protected override void InvokeEditProcess(AssetEditProcess<T> editProcess)
        {
            Undo.RecordObject(editProcess.Target, "Modify Objects");

            editProcess.Action(editProcess.Target);

            IncrementCount();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void IncrementCount() => editCount++;


        /// <summary>
        ///
        /// </summary>
        /// <param name="exception"></param>
        protected override void OnApplicationException(Exception exception)
        {
            SetCalledException(true);

            base.OnApplicationException(exception);

            FailedSoUndo();
        }

        /// <summary>
        ///
        /// </summary>
        protected virtual void FailedSoUndo()
        {
            UnityEngine.Debug.Log("REPORT: Since the Asset Editing Failed the changes made during the process will be reverted.");

            Undo.CollapseUndoOperations(undoIndex);

            Undo.PerformUndo();
        }


        /// <summary>
        ///
        /// </summary>
        protected override void OnEditingCompleted()
        {
            if (calledException)
                return;

            UnityEngine.Debug.Log($"REPORT: Process Resolved. {editCount} assets processed.");

            Undo.CollapseUndoOperations(undoIndex);

            AssetDatabase.SaveAssets();
        }
    }
}