//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
#if FMOD_UNITY
using FMODUnity;
#endif
using DREditor.Dialogues;
using System.Collections.Generic;
using UnityEngine;

namespace DREditor.Minigames.Debates
{
    /// <summary>
    ///
    /// </summary>
    /// <typeparam name="T"></typeparam>
    [System.Serializable]
    public abstract class DebateBuilder<T> : DialogueBase<T>, IDebateConverter, IDebateScanner
    {
#if FMOD_UNITY
        /// <summary>
        ///
        /// </summary>
        [EventRef]
        public string StartingMusic;
#else
        /// <summary>
        ///
        /// </summary>
        public AudioClip StartingMusic;
#endif
        /// <summary>
        ///
        /// </summary>
        public List<BulletDifficulty> TruthBullets = new();

        /// <summary>
        ///
        /// </summary>
        public int trialNumber = 0;

        /// <summary>
        ///
        /// </summary>
        public List<TimerDifficulty> times = new();

        /// <summary>
        ///
        /// </summary>
        [SerializeField] protected int skip = 0;

        /// <summary>
        ///
        /// </summary>
        public int Skip { get => skip; set => skip = value; }


        public abstract void Convert();

        public abstract bool Scan(out string log);


        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual int GetPanelCount() => Lines.Count;
    }
}