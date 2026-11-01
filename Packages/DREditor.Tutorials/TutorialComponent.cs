//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden
using System.Collections;

namespace DREditor.Tutorials
{
    /// <summary>
    ///
    /// </summary>
    public abstract class TutorialComponent : TutorialComponentBase
    {
        /// <summary>
        ///
        /// </summary>
        protected int currentIndex = 0;


        /// <summary>
        ///
        /// </summary>
        /// <param name="newIndex"></param>
        public abstract IEnumerator ChangePageRoutine(int newIndex);

        /// <summary>
        ///
        /// </summary>
        public abstract void PreResolve();


        /// <summary>
        ///
        /// </summary>
        /// <param name="newIndex"></param>
        public virtual void UpdateCurrentIndex(int newIndex) => currentIndex = newIndex;
    }
}