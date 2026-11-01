//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Cameras;
using DREditor.Dialogues;
using DREditor.Doors;
using System;
using UnityEngine;

namespace DREditor.SubAreas
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class SubAreaData : IDialogueHolder
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Space(10)]

        [Header("")]
        [Divider]
        [Tooltip("")]
        public string codeName = "";

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public bool activated = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public bool finishedStart = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public bool finishedAuto = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public bool isWaiting = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public bool isWaitingAuto = false;

        /// <summary>
        ///
        /// </summary>
        [Space(10)]
        [Header("")]
        [Divider]
        [Tooltip("")]
        public Vector3 returnPosition;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public Vector3 returnRotation;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public Vector2 returnAbs;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        public Vector2 returnSmooth;

        /// <summary>
        ///
        /// </summary>
        [Space(10)]
        [Header("")]
        [Divider]
        [Tooltip("")]
        public LockData enterData;
        /// <summary>
        ///
        /// </summary>
        [Space(10)]
        [Tooltip("")]
        public LockData leaveData;


        /// <summary>
        ///
        /// </summary>
        /// <param name="baseData"></param>
        /// <param name="instanceData"></param>
        /// <returns></returns>
        public static SubAreaData MergeInstance(SubAreaData baseData, SubAreaData instanceData)
        {
            SubAreaData mergedData = (SubAreaData)instanceData.Clone();

            mergedData.leaveData = (LockData)baseData.leaveData.Clone();

            mergedData.enterData = (LockData)baseData.enterData.Clone();

            return mergedData;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual object Clone()
        {
            SubAreaData data = new()
            {
                codeName = codeName,
                activated = activated,
                finishedStart = finishedStart,
                finishedAuto = finishedAuto,
                isWaiting = isWaiting,
                isWaitingAuto = isWaitingAuto,
                returnPosition = returnPosition,
                returnRotation = returnRotation,
                returnAbs = returnAbs,
                returnSmooth = returnSmooth,
                leaveData = (LockData)leaveData.Clone(),
                enterData = (LockData)enterData.Clone()
            };

            return data;
        }


        /// <summary>
        ///
        /// </summary>
        /// <param name="codeName"></param>
        /// <param name="leaveData"></param>
        public virtual void SetIDLeaveData(string codeName, LockData leaveData)
        {
            this.codeName = codeName;

            this.leaveData = leaveData;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="activated"></param>
        public virtual void SetActivated(bool activated) => this.activated = activated;

        /// <summary>
        ///
        /// </summary>
        public virtual void FinishedStart()
        {
            finishedStart = true;

            SetIsWaiting(false);
        }

        /// <summary>
        ///
        /// </summary>
        public virtual void FinishedAuto()
        {
            SetIsWaitingAuto(false);

            finishedAuto = true;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="isWaiting"></param>
        public virtual void SetIsWaiting(bool isWaiting) => this.isWaiting = isWaiting;

        /// <summary>
        ///
        /// </summary>
        /// <param name="isWaitingAuto"></param>
        public virtual void SetIsWaitingAuto(bool isWaitingAuto) => this.isWaitingAuto = isWaitingAuto;


        /// <summary>
        ///
        /// </summary>
        /// <param name="cam"></param>
        public virtual void SetReturnValues(UnityEngine.Camera cam)
        {
            returnPosition = cam.transform.position;

            returnRotation = cam.transform.eulerAngles;

            SmoothMouseLook mouse = cam.GetComponent<SmoothMouseLook>();

            returnAbs = mouse.GetAbsolute();

            returnSmooth = mouse.GetSmooth();
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual bool EnterMissingOrUnlocked() => !EnterDataValid() || !enterData.IsLocked();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual bool EnterDataValid() =>
            ReportElse(enterData != null, $"REPORT: {codeName}: Enter Data Not available");

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual bool EnteringPermanentlyLocked() =>
            ReportIf(enterData.locked, $"REPORT: {codeName}: Enter Data is permanently Locked.");

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual bool EnteringUnlocked() =>
            ReportElse(enterData.IsUnlocked(), $"REPORT: {codeName}: Enter Data has been Locked.");

        /// <summary>
        ///
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        protected virtual bool ReportIf(bool condition, string message)
        {
            if (condition)
                Debug.Log(message);

            return condition;
        }

        /// <summary>
        ///
        /// </summary>
        /// <param name="condition"></param>
        /// <param name="message"></param>
        /// <returns></returns>
        protected virtual bool ReportElse(bool condition, string message)
        {
            if (!condition)
                Debug.Log(message);

            return condition;
        }

        /// <inheritdoc/>
        public virtual bool CheckIfSaveDialogue() =>
            (enterData is IDialogueHolder enter && enter.CheckIfSaveDialogue()) ||
            (leaveData is IDialogueHolder leave && leave.CheckIfSaveDialogue());
    }
}