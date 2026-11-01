//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System;

namespace DREditor.Maps
{
    /// <summary>
    ///
    /// </summary>
    [Serializable]
    public class IDHolder
    {
        /// <summary>
        ///
        /// </summary>
        public string value = "";

        /// <summary>
        ///
        /// </summary>
        protected bool generating = false;


        /// <summary>
        ///
        /// </summary>
        /// <param name="value"></param>
        public virtual void SetID(string value) => this.value = value;

        /// <summary>
        ///
        /// </summary>
        /// <returns></returns>
        public virtual bool GenerateID()
        {
            if (generating || value != "")
                return false;

            generating = true;

            value = System.Guid.NewGuid().ToString();

            generating = false;

            return true;
        }
    }
}