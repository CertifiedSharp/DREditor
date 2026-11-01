//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Dialogues.Events;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.Spawnables.Implementation
{
    /// <summary>
    ///
    /// </summary>
    [System.Serializable]
    public class VariableCapsule
    {
        /// <summary>
        ///
        /// </summary>
        /// <remarks></remarks>
        public bool ignoreCondition = false;

        /// <summary>
        ///
        /// </summary>
        public bool showDiaEvents = false;

        /// <summary>
        ///
        /// </summary>
        public List<VariableBase> Conditions = new();

        /// <summary>
        ///
        /// </summary>
        [SerializeReference] 
        public List<IDialogueEvent> DiaEvents = new();


        /// <summary>
        ///
        /// </summary>
        /// <param name="ignoreCondition"></param>
        public virtual void SetIgnoreCondition(bool ignoreCondition) => this.ignoreCondition = ignoreCondition;

        /// <summary>
        ///
        /// </summary>
        /// <param name="showDiaEvents"></param>
        public virtual void SetShowDialogueEvents(bool showDiaEvents) => this.showDiaEvents = showDiaEvents;

        /// <summary>
        ///
        /// </summary>
        /// <param name="DiaEvents"></param>
        public virtual void SetDialogueEvents(List<IDialogueEvent> DiaEvents) => this.DiaEvents = DiaEvents;
    }
}