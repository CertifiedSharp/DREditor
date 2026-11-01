//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.LocalDialogues;
using DREditor.Progressing.Handling;
using DREditor.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Serialization;

namespace DREditor.Doors
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class LockData : IDialogueHolder
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]
        [Divider]
        [Tooltip("")]
        [FormerlySerializedAs("unlockedDialogue")]
        public List<LocalDialogue> unlockedDialogues = new();

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [FormerlySerializedAs("flagList")]
        public List<string> flags = new();

        /// <summary>
        ///
        /// </summary>
        [Space]

        [Tooltip("")]
        public bool locked = false;

        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [FormerlySerializedAs("lockDialogue")]
        public List<LocalDialogue> lockDialogues = new();


        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual bool IsLocked() => locked || !IsUnlocked();

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual bool IsUnlocked()
        {
            if (flags.Count == 0 && lockDialogues != null && lockDialogues.Count > 0)
                return false;

            var list = flags.ToArray().Where(flagName =>
                ServiceLocator.GetService<FlagTriggerHandlerBase>().IsFlagTriggered(flagName));

            return list.Count() == flags.Count;
        }

        /// <summary>
        ///
        /// </summary>
        public virtual void ClearData()
        {
            unlockedDialogues.Clear();

            flags.Clear();

            lockDialogues.Clear();

            locked = false;
        }

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual object Clone()
        {
            LockData copy = (LockData)MemberwiseClone();

            LockData data = new()
            {
                unlockedDialogues = CloneList(copy.unlockedDialogues),
                lockDialogues = CloneList(copy.lockDialogues),
                flags = CloneList(copy.flags),
                locked = copy.locked
            };

            return data;
        }

        /// <summary>
        ///
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="copy"></param>
        /// <returns></returns>
        /// <remarks></remarks>
        static List<T> CloneList<T>(List<T> copy)
        {
            List<T> dupe = new();

            foreach (T obj in copy)
                dupe.Add(obj);

            return dupe;
        }


        /// <inheritdoc/>
        public virtual bool CheckIfSaveDialogue() => unlockedDialogues.CheckIfSaveDialogue() || lockDialogues.CheckIfSaveDialogue();
    }
}