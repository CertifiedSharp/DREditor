//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Dialogues;
using DREditor.Services;
using System.Collections;
using UnityEngine;

namespace DREditor.DialogueMechanics.Investigation
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <remarks></remarks>
    public abstract class PhaseStartHandler<T> : ServiceHolder<T>, IPhaseStartHandler where T : class
    {
        /// <summary>
        ///
        /// </summary>
        [Header("")]

        [Space]

        [Tooltip("")]
        [SerializeField] protected int gamePhaseID = 0;


        int IPhaseStartHandler.ID { get => gamePhaseID; }


        public abstract IEnumerator OnPhaseStart(StartInvestigation start);

        public abstract void AfterLoadingRoom();
    }
}