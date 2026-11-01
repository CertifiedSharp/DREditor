//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using DREditor.Processables;
using UnityEngine;

namespace DREditor.Presents
{
    /// <summary>
    ///
    /// </summary>
    public abstract class PresentSaveBase : IIdentifiable
    {
        /// <summary>
        ///
        /// </summary>
        [Tooltip("")]
        [SerializeField] protected int ID;


        int IIdentifiable.ID => ID;


        protected PresentSaveBase() { }

        protected PresentSaveBase(int ID) => this.ID = ID;
    }
}