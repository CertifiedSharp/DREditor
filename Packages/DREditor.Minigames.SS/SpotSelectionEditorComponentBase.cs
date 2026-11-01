//Author: Benjamin "Sweden" Jillson : Discord: sweden_gaming For Project Eden's Garden

namespace DREditor.Minigames.SS
{
    /// <summary>
    ///
    /// </summary>
    [System.Serializable]
    public abstract class SpotSelectionEditorComponentBase
    {
        /// <summary>
        ///
        /// </summary>
        protected SpotSelectionBuilder builder;


        protected SpotSelectionEditorComponentBase(SpotSelectionBuilder builder) => this.builder = builder;


        /// <summary>
        ///
        /// </summary>
        public abstract void Display();
    }
}